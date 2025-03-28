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
        Task<string> CallbackPayment(string redisKey,IQueryCollection data);
        Task<string> CallbackPaymentTicket(string redisKey, IQueryCollection data);
        Task<bool> RefundPayment(Guid idTransaction);
        Task<string> QuerryTransaction(Guid idTransaction);
    }
}
