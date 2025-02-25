using Cursus.Core.Options.PaymentSetting;
using SkinTime.DAL.Entities;
using SkinTime.DAL.Enum;
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

        public TransactionService(IUnitOfWork unitOfWork,VNPay vNPay,ZaloPay zaloPay)
        {
            _unitOfWork = unitOfWork;
            _vnPay = vNPay;
            _zaloPay = zaloPay;
        }
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
