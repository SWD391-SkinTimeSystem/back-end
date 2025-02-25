using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
