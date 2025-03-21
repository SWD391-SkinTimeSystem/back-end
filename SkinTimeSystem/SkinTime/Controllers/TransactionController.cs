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
        public async Task<IActionResult> TransactionCallback(string redisKey)
        {
                var data = Request.Query;
                var url = await _service.CallbackPayment(redisKey, data);
                return  Redirect(url); 
        }
        [HttpPost]
        public async Task<IActionResult> RefundTransaction([FromBody] Guid idTransaction)
        {
            return await HandleApiCallAsync(async () =>
            {
                return ServiceResult.Success(await _service.RefundPayment(idTransaction));
            }); 
        }
    }
}
