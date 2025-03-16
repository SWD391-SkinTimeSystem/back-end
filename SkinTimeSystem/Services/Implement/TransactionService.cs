using AutoMapper;
using BusinessObject.Entities;
using BusinessObject.Enum;
using BusinessObject.Schedule;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Repositories.Interface;
using Repositories.UnitOfWork;
using Services.Commons;
using Services.Commons.DTOs.Booking;
using Services.Interfaces;
using Services.PaymentSetting;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var bookingDto = JsonConvert.DeserializeObject<BookingServiceWithIdDTO>(jsonData);
            

            var bank = Enum.TryParse(bookingDto.PaymentMethod, true, out PaymentMethod pm) && Enum.IsDefined(pm) ? pm : (PaymentMethod?)null;
            bool isSuccess = true;

            if (bank == PaymentMethod.VnPay)
            {
                isSuccess = await HandleVnPayCallback(data);
            }
            else if (bank == PaymentMethod.ZaloPay)
            {
                isSuccess = await HandleZaloPayCallback(data);
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


        public async Task<ServiceResult> CallbackTicketPayment(IQueryCollection data, EventTicket ticket)
        {
            if (data.ContainsKey("vnp_BankCode"))
            {
                if (!await HandleVnPayCallback(data))
                {
                    return ServiceResult.Failed(ServiceError.ValidationFailed("Error with VnPay")); ;
                }
            }

            if (data.ContainsKey("bankcode")
                   && (data["bankode"].ToString() == "" || data["bankode"].ToString() == "zalopayapp" || data["bankode"].ToString() == "CC")
               )
            {
                bool zaloPayResult = await HandleZaloPayCallback(data);
                if (!zaloPayResult)
                {
                    return ServiceResult.Failed(ServiceError.ValidationFailed("Error with ZaloPay"));
                }
            }

            var addedTicket = await _unitOfWork.Repository<EventTicket>().AddAsync(ticket);
            await _unitOfWork.Complete();

            return ServiceResult.Success(addedTicket);
        }

       

        #region VNPAY
        private async Task<bool> HandleVnPayCallback(IQueryCollection data)
        {
            if (!data.ContainsKey("vnp_ResponseCode") ||
                !data.ContainsKey("vnp_TxnRef") ||
                !data.ContainsKey("vnp_SecureHash") ||
                !data.ContainsKey("vnp_OrderInfo") ||
                !data.ContainsKey("vnp_Amount"))
            {
                return false;
            }

            bool isSuccess = data["vnp_ResponseCode"] == "00";
            PaymentStatus status = isSuccess ? PaymentStatus.Success : PaymentStatus.Failed;
            Guid transactionID = Guid.Parse(data["vnp_TxnRef"]!);
            decimal amount = decimal.Parse(data["vnp_Amount"]!);
            amount /= 100;
            var paymentMethod = PaymentMethod.VnPay;

            bool isValidVNPay = await CallBackVnPay(data["vnp_TxnRef"], data["vnp_SecureHash"], data);

            await CreateTransaction(transactionID, paymentMethod, amount, status, false, data["vnp_TxnRef"]);

            return isSuccess && isValidVNPay;
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
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }
        #endregion

        #region ZALOPAY
        private async Task<bool> HandleZaloPayCallback(IQueryCollection data)
        {
            if (!data.ContainsKey("status") || !data.ContainsKey("checksum") || !data.ContainsKey("amount") || !data.ContainsKey("apptransid"))
                return false;

            bool isSuccess = data["status"] == "1";
            decimal amount = decimal.Parse(data["amount"]);
            string transactionCode = data["apptransid"];
            var status = isSuccess ? PaymentStatus.Success : PaymentStatus.Failed;

            // Xác thực checksum
            bool isValidChecksum = await _zaloPay.HandleZaloPayCallback(data);

            // Luôn tạo giao dịch
            var transactionID = Guid.NewGuid();
            await CreateTransaction(transactionID, PaymentMethod.ZaloPay, amount, status, false, transactionCode);

            if (isValidChecksum && isSuccess)
            {
                return true;
            }

            return false;
        }

        #endregion
        public async Task<bool> CreateTransaction(
       Guid transactionId,
       PaymentMethod paymentMethod,
       decimal amount,
       PaymentStatus paymentStatus,
       bool isRefund,
       string? transactionCode
   )
        {
            var transaction = new Transaction
            {
                Id = transactionId,
                IsRefundTransaction = isRefund,
                Amount = amount,
                Method = paymentMethod,
                Status = paymentStatus,
                TransactionTime = DateTime.Now,
                TransactionCode = transactionCode
            };
            await _unitOfWork.Repository<Transaction>().AddAsync(transaction);
            await _unitOfWork.Complete();
            return true;
        }

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

        public Task<string> RefundPayment(Guid id, string returnAction, string name, decimal amount)
        {

           return _zaloPay.CreateZaloPayRefund(amount ,returnAction, name);
        }


        public Task<string> RefundPaymentvnpay()
        {
            return _vnPay.CreateVNPayRefundOrder();
        }
    }
}
