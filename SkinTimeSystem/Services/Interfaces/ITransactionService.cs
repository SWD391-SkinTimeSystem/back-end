using BusinessObject.Entities;
using Microsoft.AspNetCore.Http;
using Services.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;



namespace Services.Interfaces
{
    public interface ITransactionService
    {
        Task<string> CreateTransaction(System.Transactions.Transaction bookingTransaction, string returnUrl, string notifyUrl);

        public Task<bool> CallbackPayment(string redisKey,IQueryCollection data);

        public Task<bool> CallbackPayment(Guid itemId, object entity, IQueryCollection data);

        public Task<ServiceResult> CallbackTicketPayment(IQueryCollection data, EventTicket ticket);

        public Task<ServiceResult> CallbackRefundPayment(IQueryCollection data);
    }
}
