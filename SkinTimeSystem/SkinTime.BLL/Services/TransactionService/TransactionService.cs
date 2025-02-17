using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SkinTime.BLL.Commons;
using SkinTime.DAL.Entities;
using SkinTime.DAL.Enum;
using SkinTime.DAL.Interfaces;
using SkinTime.DAL.Payment.VnPay;
using SkinTime.DAL.Payment.ZaloPay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Transactions;

namespace SkinTime.BLL.Services.TransactionService
{
    public class TransactionService : ITransactionService
    {
        private readonly VNPay _vnPay;
        private readonly ZaloPay _zaloPay;
        private readonly IUnitOfWork _unitOfWork;
        public TransactionService(IUnitOfWork unitOfWork,VNPay vNPay,ZaloPay zaloPay)
        {
            _unitOfWork = unitOfWork;
            _vnPay = vNPay;
            _zaloPay = zaloPay;
        }
        public async Task<ServiceResult<BookingTransaction>> CreateTransaction(Guid userId, BookingTransaction transaction, string returnUrl)
        {
            try
            {
                ServiceResult<string> paymentResult;

                switch (transaction.PaymentMethod.ToUpperInvariant())
                {
                    case nameof(Bank.VNPAY):
                        paymentResult = await CreateVNPayOrder(transaction.Amount, returnUrl);
                        break;
                    case nameof(Bank.ZALOPAY):
                        paymentResult = await CreateZaloPayOrder(transaction.Amount, returnUrl);
                        break;
                    default:
                        throw new InvalidOperationException($"Unsupported payment bank: {transaction.PaymentMethod}");
                }

                if (paymentResult.IsFailed)
                {
                    return ServiceResult<BookingTransaction>.Failed(paymentResult.Error);
                }

                // Lưu giao dịch vào database
                transaction.Id = Guid.NewGuid();
                transaction.Status = "Pending"; // Đánh dấu đang xử lý thanh to

                await _unitOfWork.Repository<BookingTransaction>().AddAsync(transaction);
                await _unitOfWork.Complete();

                return ServiceResult<BookingTransaction>.Success(transaction);
            }
            catch (Exception ex)
            {
                return ServiceResult<BookingTransaction>.Failed(new ServiceError("Error creating transaction", ex.Message));
            }
        }


        #region VNPAY
        private async Task<string> CreateVNPayOrder(decimal? amount, string returnUrl)
        {
            string ipAddress = await _vnPay.GetIpAddress();
            await _vnPay.ConfigureRequest(amount, returnUrl, ipAddress);
            return await _vnPay.CreatePaymentUrlAsync();
        }
        #endregion

        #region ZALOPAY
        private async Task<string> CreateZaloPayOrder(decimal? amount, string returnUrl)
        {
            var response = await _zaloPay.CreateZaloPayQrOrderAsync(amount, returnUrl);
            if (response.TryGetValue("order_url", out var orderUrl))
            {
                return orderUrl; // URL to redirect user for ZaloPay QR code
            }
            throw new Exception("Failed to create ZaloPay order.");
        }
        #endregion


        //#region Response (Callback)
        //public async Task<bool> CallbackPayment(IQueryCollection data, AppUser user, double amount)
        //{
        //    try
        //    {
        //        if (data.ContainsKey("token"))
        //        {
        //            bool paypalResult = await HandlePayPalCallback(data, user, amount);
        //            if (!paypalResult)
        //            {
        //                return false;
        //            }
        //        }
        //        if (data.ContainsKey("vnp_BankCode"))
        //        {
        //            bool vnPayResult = await HandleVnPayCallback(data, user, amount);
        //            if (!vnPayResult)
        //            {
        //                return false;
        //            }
        //        }
        //        if (
        //            data.ContainsKey("bankcode")
        //            && (
        //                data["bankode"].ToString() == ""
        //                || data["bankode"].ToString() == "zalopayapp"
        //                || data["bankode"].ToString() == "CC"
        //            )
        //        )
        //        {
        //            bool zaloPayResult = await HandleZaloPayCallback(data, user, amount);
        //            if (!zaloPayResult)
        //            {
        //                return false;
        //            }
        //        }

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error during payment processing: {ex.Message}");
        //        return false;
        //    }
        //}


        //#region VNPAY
        //private async Task<bool> HandleVnPayCallback(
        //    IQueryCollection data,
        //    AppUser user,
        //    double amount
        //)
        //{
        //    bool vnp_ResponseCode =
        //        data.ContainsKey("vnp_ResponseCode") && data["vnp_ResponseCode"] == "00";
        //    var status = vnp_ResponseCode
        //        ? TransferInfosStatus.Completed.ToString()
        //        : TransferInfosStatus.Failed.ToString();
        //    var vnp_TxnRef = data["vnp_TxnRef"].ToString();
        //    var vnp_SecureHash = data["vnp_SecureHash"].ToString();
        //    if (!string.IsNullOrEmpty(vnp_TxnRef) && !string.IsNullOrEmpty(vnp_SecureHash))
        //    {
        //        var checkVNPay = await CallBackVnPay(vnp_TxnRef, vnp_SecureHash, data);
        //        await SaveTransaction(
        //            DateTime.Now,
        //            amount,
        //            status,
        //            TransferInfosType.Deposit.ToString(),
        //            user.Id,
        //            user
        //        );
        //        if (!checkVNPay || !vnp_ResponseCode)
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            await UpdateBalance(user.Id, amount);
        //            return true;
        //        }
        //    }
        //    return false;
        //}

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
        //        Console.WriteLine($"An error occurred: {ex.Message}");
        //        return false;
        //    }
        //}
        //#endregion

        //#region ZALOPAY
        //private async Task<bool> HandleZaloPayCallback(
        //    IQueryCollection data,
        //    AppUser user,
        //    double amount
        //)
        //{
        //    bool statusZalo = data.ContainsKey("status") && data["status"] == "1";
        //    var status = statusZalo
        //        ? TransferInfosStatus.Completed.ToString()
        //        : TransferInfosStatus.Failed.ToString();
        //    var checksumZaloPay = data["checksum"].ToString();
        //    if (!string.IsNullOrEmpty(checksumZaloPay))
        //    {
        //        bool checksum = await _zaloPay.HandleZaloPayCallback(data);
        //        await SaveTransaction(
        //            DateTime.Now,
        //            amount,
        //            status,
        //            TransferInfosType.Deposit.ToString(),
        //            user.Id,
        //            user
        //        );
        //        if (!checksum || !statusZalo)
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            await UpdateBalance(user.Id, amount);
        //            return true;
        //        }
        //    }
        //    return false;
        //}
        //#endregion

        //public async Task UpdateBalance(string userId, double? amount)
        //{
        //    var user = await _context.Users.FindAsync(userId);
        //    if (user != null && amount.HasValue)
        //    {
        //        user.BalanceWallet += amount.Value;
        //        await _context.SaveChangesAsync();
        //    }
        //}

        //public async Task<bool> SaveTransaction(
        //    DateTime createDay,
        //    double amount,
        //    string status,
        //    string type,
        //    string appUserId,
        //    AppUser user
        //)
        //{
        //    try
        //    {
        //        var transaction = new TransferInfo
        //        {
        //            Id = Guid.NewGuid(),
        //            CreatedDate = createDay,
        //            Amount = amount,
        //            Status = status,
        //            Type = type,
        //            AppUserId = appUserId,
        //            User = user,
        //        };

        //        await _context.TransferInfos.AddAsync(transaction);
        //        await SetIdTransaction(transaction.Id);
        //        return await _context.SaveChangesAsync() > 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"An error occurred: {ex.Message}");
        //        return false;
        //    }
        //}
        //#endregion
        public Task<ServiceResult<List<Transaction>>> GetAllTransaction()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<Transaction>> GetTransaction(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<BookingTransaction>> CreateTransaction(BookingTransaction Transaction)
        {
            throw new NotImplementedException();
        }
    }
}
