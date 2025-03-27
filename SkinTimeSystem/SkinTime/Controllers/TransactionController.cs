using AutoMapper;
using BusinessObject.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Commons;
using Services.Commons.DTOs.Transaction;
using Services.Interfaces;
using SharedLibrary.EmailUtilities;
using SharedLibrary.TokenUtilities;
using SkinTime.Extensions;
using StackExchange.Redis;
using System.Net;
using System.Transactions;

namespace SkinTime.Controllers
{
    [Route("api/transaction")]
    [ApiController]
    public class TransactionController : BaseController
    {
        private readonly ITransactionService _service; 
        public TransactionController(IDatabase database, IMapper mapper, IEmailUtilities emailUtils, ITokenUtilities tokenUtils, ITransactionService
 service)
        : base(mapper, emailUtils, tokenUtils)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> TransactionCallback(string key)
        {
                var data = Request.Query;
                var url = await _service.CallbackPayment(key, data);
                return  Redirect(url); 
        }
        [HttpPost("refund")]
        public async Task<IActionResult> RefundTransaction([FromBody] Guid idTransaction)
        {
            return await HandleApiCallAsync(async () =>
            {
                return await _service.RefundPayment(idTransaction);
            });
        }
        [HttpGet("transaction-query")]
        public async Task<IActionResult> QuerryTransaction(Guid idTransaction)
        {
            return await HandleApiCallAsync(async () =>
            {
                return await _service.QuerryTransaction(idTransaction);
            });
        }
        [HttpGet("ticket-callback")]
        public async Task<IActionResult> TicketTransactionCallback(string key)
        {
            var data = Request.Query;
            var url = await _service.CallbackPaymentTicket(key, data);
            return Redirect(url);
        }
    }
}
