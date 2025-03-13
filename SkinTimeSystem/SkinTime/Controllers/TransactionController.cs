using AutoMapper;
using BusinessObject.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Interfaces;
using SharedLibrary.EmailUtilities;
using SharedLibrary.TokenUtilities;
using SkinTime.DTOs.Booking;
using SkinTime.DTOs.Ticket;
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
        private readonly IDatabase _database;
        public TransactionController(IDatabase database, IMapper mapper, IEmailUtilities emailUtils, ITokenUtilities tokenUtils, ITransactionService
 service)
        : base(mapper, emailUtils, tokenUtils)
        {
            _database = database;
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> TransactionCallback( string redisKey)
        {


            var data = Request.Query;


            var paymentResult = await _service.CallbackPayment(redisKey,data);
          //  await _database.DeleteAsync(redis);
            //if (paymentResult)
            //{
            //    return Redirect(bookingData.ReturnURL);
            //}
            //else
            //{
            //    return Redirect(bookingData.FailureURL);
            //}
            return Ok();
        }

        [HttpGet("ticket-callback")]
        public async Task<IActionResult> TicketTransactionCallback(string redis)
        {
            var ticketData = await _database.GetAsync<TicketRegistrationCacheModel>(redis);

            if (ticketData == null)
            {
                return NotFound();
            }

            var data = Request.Query;

            EventTicket ticket = _mapper.Map<EventTicket>(ticketData);

            var paymentResult = await _service.CallbackTicketPayment(data, ticket);
            await _database.DeleteAsync(redis);

            if (paymentResult.IsSuccess)
            {
                return Redirect(ticketData.SuccessCallbackUrl);
            }
            else
            {
                return Redirect(ticketData.FailureCallbackUrl);
            }
        }
    }
}
