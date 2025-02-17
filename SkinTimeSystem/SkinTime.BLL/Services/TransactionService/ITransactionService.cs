using SkinTime.BLL.Commons;
using SkinTime.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace SkinTime.BLL.Services.TransactionService
{
    public interface ITransactionService
    {
        Task<ServiceResult<BookingTransaction>> CreateTransaction(BookingTransaction Transaction);
        Task<ServiceResult<Transaction>> GetTransaction(Guid id);
        Task<ServiceResult<List<Transaction>>> GetAllTransaction();
    }
}
