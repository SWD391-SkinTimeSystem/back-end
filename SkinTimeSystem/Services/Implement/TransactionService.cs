using AutoMapper;
using BusinessObject.Entities;
using BusinessObject.Enum;
using BusinessObject.Schedule;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Repositories.Interface;
using Repositories.UnitOfWork;
using Services.Commons;
using Services.Commons.DTOs.Booking;
using Services.Commons.DTOs.Transaction;
using Services.Interfaces;
using Services.PaymentSetting;

namespace Services.Implement
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICache _cache;
        private readonly VNPay _vnPay;
        private readonly ZaloPay _zaloPay;

        public TransactionService(IMapper mapper,ICache cache, IUnitOfWork unitOfWork, VNPay vNPay, ZaloPay zaloPay)
        {
            _unitOfWork = unitOfWork;
            _vnPay = vNPay;
            _zaloPay = zaloPay;
            _cache = cache;
            _mapper = mapper;
        }

        public async Task<string> CallbackPayment(string redisKey, IQueryCollection data)
        {
            string jsonData = await _cache.GetAsync<string>(redisKey);
            Guid key = Guid.Parse(redisKey);
            var bookingDto = JsonConvert.DeserializeObject<BookingServiceWithIdDTO>(jsonData);
            

            var bank = Enum.TryParse(bookingDto.PaymentMethod, true, out PaymentMethod pm) && Enum.IsDefined(pm) ? pm : (PaymentMethod?)null;
            bool isSuccess = true;

            if (bank == PaymentMethod.VnPay)
            {
                isSuccess = await HandleVnPayCallback(data, key);
            }
            else if (bank == PaymentMethod.ZaloPay)
            {
                isSuccess = await HandleZaloPayCallback(data, key);
            }

            if (!isSuccess)
            {
                return bookingDto.FailureURL;
            }
            var booking = _mapper.Map<Booking>(bookingDto);
            await _unitOfWork.Bookings.CreateBookingAndSchedule(booking,bookingDto.ServiceHour);
            await _cache.DeleteAsync<string>(redisKey);
            return  bookingDto.ReturnURL;
        }


        //public async Task<ServiceResult> CallbackTicketPayment(IQueryCollection data, EventTicket ticket)
        //{
        //    if (data.ContainsKey("vnp_BankCode"))
        //    {
        //        if (!await HandleVnPayCallback(data))
        //        {
        //            return ServiceResult.Failed(ServiceError.ValidationFailed("Error with VnPay")); ;
        //        }
        //    }

        //    if (data.ContainsKey("bankcode")
        //           && (data["bankode"].ToString() == "" || data["bankode"].ToString() == "zalopayapp" || data["bankode"].ToString() == "CC")
        //       )
        //    {
        //        bool zaloPayResult = await HandleZaloPayCallback(data);
        //        if (!zaloPayResult)
        //        {
        //            return ServiceResult.Failed(ServiceError.ValidationFailed("Error with ZaloPay"));
        //        }
        //    }

        //    var addedTicket = await _unitOfWork.Repository<EventTicket>().AddAsync(ticket);
        //    await _unitOfWork.Complete();

        //    return ServiceResult.Success(addedTicket);
        //}



        #region VNPAY
        private async Task<bool> HandleVnPayCallback(IQueryCollection data, Guid id)
        {
            var dto = new VnPayTransactionDTO
            {
                TransactionTime = data["vnp_PayDate"],
                Amount = decimal.Parse(data["vnp_Amount"]) / 100,
                TransactionCode = data["vnp_TxnRef"],
                TransactionReference = data["vnp_TxnRef"]
            };

            var transaction = _mapper.Map<Transaction>(dto);
            transaction.Id = id;

            await _unitOfWork.Repository<Transaction>().AddAsync(transaction);
            await _unitOfWork.Complete();

            return true;
        }


        private async Task<bool> CallBackVnPay(
            string vnp_TxnRef,
            string vnp_SecureHash,
            IQueryCollection request
        )
        {
            try
            {
                _vnPay.AddResponseDataFromQueryString(request);
                await _vnPay.ValidateSignature(vnp_TxnRef, vnp_SecureHash);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region ZALOPAY
        private async Task<bool> HandleZaloPayCallback(IQueryCollection data, Guid key)
        {
            var dto = new ZaloPayTransactionDTO
            {
                Amount = decimal.Parse(data["amount"]),
                TransactionReference = data["apptransid"],
                Status = data["status"]
            };

            var transaction = _mapper.Map<Transaction>(dto);
            transaction.Id = key;
            transaction.TransactionCode = await _zaloPay.GetZaloPayTransactionIdAsync(data["apptransid"]);

            await _unitOfWork.Repository<Transaction>().AddAsync(transaction);
            await _unitOfWork.Complete();

            return transaction.Status == PaymentStatus.Success;
        }


        #endregion
        public Task<bool> CallbackPayment(Guid itemId, object entity, IQueryCollection data)
        {
            throw new NotImplementedException();
        }


        public string DeterminePaymentService(IQueryCollection data)
        {
            if (data.ContainsKey("vnp_BankCode"))
            {
                return "vnpay";
            }
            else if (data.ContainsKey("bankcode"))
            {
                return "zalopay";
            }
            else
            {
                return "unknown";
            }
        }

        public async Task<ServiceResult> CallbackRefundPayment(IQueryCollection data)
        {
            switch (DeterminePaymentService(data))
            {
                case "vnpay":
                    return ServiceResult.Success();
                case "zalopay":
                    return ServiceResult.Success();
                default:
                    return ServiceResult.Failed(ServiceError.ValidationFailed("Unknown payment type"));
            }
        }

        public Task<string> CreateTransaction(System.Transactions.Transaction bookingTransaction, string returnUrl, string notifyUrl)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult<bool>> RefundPayment(Guid idTransaction)
        {

            var transacion = await _unitOfWork.Repository<Transaction>().FindAsync(tr => tr.Id == idTransaction);
           if(transacion.Method == PaymentMethod.VnPay)
            {
               await _vnPay.CreateVnPayRefund(transacion);
            }
            if(transacion.Method == PaymentMethod.ZaloPay){
                await _zaloPay.CreateZaloPayRefund(transacion);
            }
           return ServiceResult<bool>.Success(true);
        }


    }
}
