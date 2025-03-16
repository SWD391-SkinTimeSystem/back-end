using AutoMapper;
using BusinessObject.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        public async Task<IActionResult> TransactionCallback(string redisKey)
        {
            var data = Request.Query;
            var url  = await _service.CallbackPayment(redisKey, data);

            return Redirect(url);
        }


        //[HttpGet("ticket-callback")]
        //public async Task<IActionResult> TicketTransactionCallback(string redisKey)
        //{
        //    var ticketData = await _database.GetAsync<TicketRegistrationCacheModel>(redis);

        //    if (ticketData == null)
        //    {
        //        return NotFound();
        //    }

        //    var data = Request.Query;

        //    EventTicket ticket = _mapper.Map<EventTicket>(ticketData);

        //    var paymentResult = await _service.CallbackTicketPayment(data, ticket);
        //    await _database.DeleteAsync(redis);

        //    if (paymentResult.IsSuccess)
        //    {
        //        return Redirect(ticketData.SuccessCallbackUrl);
        //    }
        //    else
        //    {
        //        return Redirect(ticketData.FailureCallbackUrl);
        //    }
        //}
        [HttpPost]
        public async Task<IActionResult> RefundTransaction( bool isBooking, Guid id, string name , decimal amount)
        {
            var returnAction = Url.Action("TransactionCallback", "Transaction", null, Request.Scheme);
            var refundUrl = await _service.RefundPayment(  id, returnAction, name, amount);
            return Ok(refundUrl);
        }

        [HttpPost("/vnpay-refund")]
        public async Task<IActionResult> RefundTransactionVNPAY()
        {
            var refundUrl = await _service.RefundPaymentvnpay();
            return Ok(refundUrl);
        }

    }
}
