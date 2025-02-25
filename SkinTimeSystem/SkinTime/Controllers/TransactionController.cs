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

        [HttpGet]
        public async Task<IActionResult> TransactionCallback(string redis)
        {
            var bookingData = await _database.GetAsync<BokingServiceWithIdModel>(redis);


            var data = Request.Query;
            
            var bookingDTO = _mapper.Map<Booking>(bookingData);
            var scheduleDTO = _mapper.Map<Schedule>(bookingData);

            var paymentResult = await _service.CallbackPayment(bookingData.UserId, data, bookingDTO, scheduleDTO);
            await _database.DeleteAsync(redis);
            if (paymentResult)
            {
                return Redirect(bookingData.ReturnURL);
            }
            else
            {
                return Redirect(bookingData.FailureURL);
            }
        }
=======
using SkinTime.BLL.Services.TransactionService;
using SkinTime.DAL.Entities;
using SkinTime.Models;
using System.Transactions;

namespace SkinTime.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class transactionController : ControllerBase
    {
        private readonly ITransactionService _service;
        private readonly IMapper _mapper;
        public transactionController(ITransactionService transactionService,IMapper mapper)
        {
            _service = transactionService;
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] TransactionModel transaction)
        {
            var bookingTransaction = _mapper.Map<BookingTransaction>(transaction);
            var approvalUrl = await _service.CreateTransaction(bookingTransaction, transaction.returnUrl,transaction.notifyUrl);
            return Redirect(approvalUrl);
        } 
    }
}
