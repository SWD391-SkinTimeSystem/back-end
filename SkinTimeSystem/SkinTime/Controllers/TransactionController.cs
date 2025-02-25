using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SharedLibrary.EmailUtilities;
using SharedLibrary.TokenUtilities;
using SkinTime.BLL.Services.BookingService;
using SkinTime.BLL.Services.TransactionService;
using SkinTime.DAL.Entities;
using SkinTime.Extensions;
using SkinTime.Models;
using StackExchange.Redis;
using System.Net;
using System.Transactions;
using Transaction = SkinTime.DAL.Entities.Transaction;

namespace SkinTime.Controllers
{
    [Route("api/transaction")]
    [ApiController]
    public class TransactionController : BaseController
    {
        private readonly ITransactionService _service;
        private readonly IDatabase _database;
        public TransactionController(IDatabase database,IMapper mapper, IEmailUtilities emailUtils, ITokenUtilities tokenUtils, ITransactionService
 service)
        : base(mapper, emailUtils, tokenUtils)
        {
            _database = database;
            _service = service;
        }
        //[HttpPost]
        //public async Task<IActionResult> CreateTransaction([FromBody] TransactionModel transaction)
        //{
        //    var bookingTransaction = _mapper.Map<BookingTransaction>(transaction);
        //    var approvalUrl = await _service.CreateTransaction(bookingTransaction, transaction.returnUrl,transaction.notifyUrl);
        //    return Redirect(approvalUrl);
        //} 

        [HttpGet]
        public async Task<IActionResult> TransactionCallback(string redis)
        {
            var bookingData = await _database.GetAsync<BookingServiceModel>(redis);


            var data = Request.Query;
            //if (string.IsNullOrEmpty(token))
            //    return BadRequest("Token is missing.");
            //var tokenData = _tokenUtils.GetDataDictionaryFromJwt(token);

            //// Parse từng thuộc tính từ token
            //Guid userId = Guid.Parse(tokenData.GetValueOrDefault("UserId"));
            //Guid serviceId = Guid.Parse(tokenData.GetValueOrDefault("ServiceId"));
            //Guid therapistId = Guid.Parse(tokenData.GetValueOrDefault("TherapistId"));
            //DateTime serviceDate = DateTime.Parse(tokenData.GetValueOrDefault("ServiceDate"));


            //TimeOnly serviceHour = TimeOnly.Parse(tokenData.GetValueOrDefault("ServiceHour"));

            //var returnUrl = tokenData.GetValueOrDefault("ReturnURL", "");
            //var voucherCode = tokenData.GetValueOrDefault("VoucherCode", "");
            //var failureUrl = tokenData.GetValueOrDefault("FailureURL", "");
            //var paymentMethod = tokenData.GetValueOrDefault("PaymentMethod", "");

            //var bookingData = new BookingServiceModel
            //{
            //    ServiceId = serviceId,
            //    ServiceDate = serviceDate,
            //    ServiceHour = serviceHour,
            //    TherapistId = therapistId,
            //    ReturnURL = returnUrl,
            //    VoucherCode = voucherCode,
            //    FailureURL = failureUrl,
            //    PaymentMethod = paymentMethod
            //};

            var bookingDTO = _mapper.Map<Booking>(bookingData);
            var scheduleDTO = _mapper.Map<Schedule>(bookingData);

            var paymentResult = await _service.CallbackPayment(userId, data, bookingDTO, scheduleDTO);

            if (paymentResult)
            {
                return Redirect(returnUrl);
            }
            else
            {
                return Redirect(failureUrl);
            }

            return Ok(); 
        }

    }
}
