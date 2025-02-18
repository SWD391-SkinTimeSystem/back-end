using SkinTime.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.BLL.Services.TransactionService
{
    public class TransactionService : ITransactionService
    {
        public Task<BookingTransaction> CreateTransaction(BookingTransaction bookingTransaction, string returnUrl, string notifyUrl)
        {
            throw new NotImplementedException();
        }
    }
}
