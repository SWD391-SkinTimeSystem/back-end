using Cursus.Core.Options.PaymentSetting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SkinTime.DAL.Entities;
using SkinTime.DAL.Enum;
using SkinTime.DAL.Enum.Schedule;
using SkinTime.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.BLL.Services.TransactionService
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly VNPay _vnPay;
        private readonly ZaloPay _zaloPay;

        public TransactionService(IUnitOfWork unitOfWork, VNPay vNPay, ZaloPay zaloPay)
        {
            _unitOfWork = unitOfWork;
            _vnPay = vNPay;
            _zaloPay = zaloPay;
        }

        public async Task<bool> CallbackPayment(Guid userId, IQueryCollection data, Booking booking, Schedule schedule)
        {
            if (data.ContainsKey("vnp_BankCode"))
            {
                bool vnPayResult = await HandleVnPayCallback(data);
                if (!vnPayResult)
                {
                    return false;
                }
            }

            if (
                   data.ContainsKey("bankcode")
                   && (
                       data["bankode"].ToString() == ""
                       || data["bankode"].ToString() == "zalopayapp"
                       || data["bankode"].ToString() == "CC"
                   )
               )
            {
                bool zaloPayResult = await HandleZaloPayCallback(data);
                if (!zaloPayResult)
                {
                    return false;
                }
            }
            await AddBookingData(userId, booking, schedule);
            return true;

        }

        public async Task AddBookingData(Guid userId, Booking booking, Schedule schedule)
        {

            var service = _unitOfWork.Repository<Service>().GetById(booking.ServiceId);
            booking.Id = Guid.NewGuid();
            booking.CustomerId = userId;
            booking.TotalPrice = service.Price;
            await _unitOfWork.Repository<Booking>().AddAsync(booking);


            var serviceDetails = service.ServiceDetailNavigation
                                        .Where(sd => !sd.IsDetele)
                                        .OrderBy(sd => sd.Step)
                                        .ToList();

            foreach (var serviceDetail in serviceDetails)
            {
                var newSchedule = new Schedule
                {
                    Id = Guid.NewGuid(),
                    BookingId = booking.Id,
                    ServiceDetailId = serviceDetail.Id,
                    Status = ScheduleStatus.NotStarted,
                    ReservedStartTime = schedule.ReservedStartTime,
                    ReservedEndTime = schedule.ReservedStartTime.Add(TimeSpan.FromMinutes(serviceDetail.Duration)),
                    Date = schedule.Date.AddDays(serviceDetail.DateToNextStep),
                };

                await _unitOfWork.Repository<Schedule>().AddAsync(newSchedule);
                newSchedule.Date = newSchedule.Date.AddDays(serviceDetail.DateToNextStep);
            }

            await _unitOfWork.Complete();

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
            decimal amount = Decimal.Parse(data["vnp_Amount"]!);
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

        public async Task<string> CreateTransaction(BookingTransaction bookingTransaction, string returnUrl, string notifyUrl)
        {
            return bookingTransaction.PaymentMethod switch
            {
              (BankEnum.VNPAY) => await _vnPay.CreateVNPayOrder(bookingTransaction.Amount, returnUrl),
                (BankEnum.ZALOPAY) => await _zaloPay.CreateZaloPayOrder(bookingTransaction.Amount, returnUrl),
                _ => throw new InvalidOperationException(
                    "Unsupported payment bank: " 
                ),
            };
        }
    }
}
