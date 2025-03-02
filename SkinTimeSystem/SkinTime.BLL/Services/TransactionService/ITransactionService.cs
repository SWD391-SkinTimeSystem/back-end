using Microsoft.AspNetCore.Http;
using SkinTime.BLL.Commons;
using SkinTime.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Transaction = SkinTime.DAL.Entities.Transaction;


namespace SkinTime.BLL.Services.TransactionService
{
    public interface ITransactionService
    {
      //  Task<string> CreateTransaction(BookingTransaction bookingTransaction,string returnUrl,string notifyUrl);

        public Task<bool> CallbackPayment(Guid userId,IQueryCollection data,Booking booking,Schedule schedule);

        public Task<bool> CallbackPayment(Guid itemId, object entity, IQueryCollection data);

        public Task<ServiceResult> CallbackTicketPayment(IQueryCollection data, EventTicket ticket);
    }
}
