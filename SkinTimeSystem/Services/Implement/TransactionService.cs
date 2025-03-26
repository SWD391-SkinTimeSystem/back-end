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
using Services.Commons.DTOs.Ticket;
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


            PaymentMethod? bank = Enum.IsDefined(typeof(PaymentMethod), bookingDto.PaymentMethod)
                ? Enum.Parse<PaymentMethod>(bookingDto.PaymentMethod, true)
                : null;

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

        #region VNPAY
        private async Task<bool> HandleVnPayCallback(IQueryCollection data, Guid id)
        {
            bool vnp_ResponseCode =
               data.ContainsKey("vnp_ResponseCode") && data["vnp_ResponseCode"] == "00";
            PaymentStatus status = vnp_ResponseCode
                ? PaymentStatus.Success
                : PaymentStatus.Failed;
            var vnPayTransactionDTO = new VnPayTransactionDTO
            {
                TransactionTime = DateTime.Now,
                Paydate = data["vnp_PayDate"]!,
                Amount = decimal.Parse(data["vnp_Amount"]!) / 100,
                TransactionCode = data["vnp_TxnRef"]!,
                TransactionReference = data["vnp_TxnRef"]!,
                Status = status,
            };

            var transaction = _mapper.Map<Transaction>(vnPayTransactionDTO);
            transaction.Id = id;

            await _unitOfWork.Repository<Transaction>().AddAsync(transaction);
            await _unitOfWork.Complete();
            if (vnp_ResponseCode)
            {
                return true;
            }
           return false;
        }


        //private async Task<bool> CallBackVnPay(
        //    string vnp_TxnRef,
        //    string vnp_SecureHash,
        //    IQueryCollection request
        //)
        //{
        //    try
        //    {
        //        _vnPay.AddResponseDataFromQueryString(request);
        //        await _vnPay.ValidateSignature(vnp_TxnRef, vnp_SecureHash);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}
        #endregion

        #region ZALOPAY
        private async Task<bool> HandleZaloPayCallback(IQueryCollection data, Guid key)
        {
            bool statusZalo = data.ContainsKey("status") && data["status"] == "1";
            PaymentStatus status = statusZalo
                ? PaymentStatus.Success
                : PaymentStatus.Failed;
            var dto = new ZaloPayTransactionDTO
            {
                Amount = decimal.Parse(data["amount"]!),
                TransactionCode = data["apptransid"]!,
                Status = status!
            };

            var transaction = _mapper.Map<Transaction>(dto);
            transaction.Id = key;
            transaction.PayDate = DateTime.Now.ToString();
            transaction.TransactionReference = await _zaloPay.GetZaloPayTransactionIdAsync(data["apptransid"]);

            await _unitOfWork.Repository<Transaction>().AddAsync(transaction);
            await _unitOfWork.Complete();

            if (statusZalo)
            {
                return true;
            }
            return false;
        }


        #endregion


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

        public async Task<string> CallbackPaymentTicket(string redisKey, IQueryCollection data)
        {
            string jsonData = await _cache.GetAsync<string>(redisKey);
            Guid key = Guid.Parse(redisKey);
            var ticketRegistration = JsonConvert.DeserializeObject<TicketRegistrationCacheDTO>(jsonData);


            PaymentMethod? bank = Enum.IsDefined(typeof(PaymentMethod), ticketRegistration.PaymentMethod)
                ? Enum.Parse<PaymentMethod>(ticketRegistration.PaymentMethod, true)
                : null;

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
                return ticketRegistration.FailureCallbackUrl;
            }
            var ticket = _mapper.Map<EventTicket>(ticketRegistration);
            var addedTicket = await _unitOfWork.Repository<EventTicket>().AddAsync(ticket);
            await _cache.DeleteAsync<string>(redisKey);
            return ticketRegistration.SuccessCallbackUrl;
        }
    }
}
