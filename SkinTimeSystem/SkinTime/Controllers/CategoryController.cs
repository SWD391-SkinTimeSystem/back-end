using AutoMapper;
using BusinessObject.Entities;
using Microsoft.AspNetCore.Mvc;
using Services.Commons;
using Services.Interfaces;
using SharedLibrary.EmailUtilities;
using SharedLibrary.TokenUtilities;
using API.Model;
using Services.Commons.DTOs.Category;

namespace SkinTime.Controllers
{
    [Route("api/category")]
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _category;
        public CategoryController(ICategoryService category, IMapper mapper, IEmailUtilities emailUtilities, ITokenUtilities tokenUtilities) : base(mapper, emailUtilities, tokenUtilities)
        {
            _category = category;
        }
        [HttpPost]
        public async Task<ActionResult> CreateCategory(CategoryDTO category)
        {
            return await HandleServiceCall<ApiResponse>(async () =>
            {
                //var result = await _service.CreateNewFeedback(_mapper.Map<Feedback>(feedback));
                //if (result.IsFailed)
                //{
                //    return result;
                //}
                return ServiceResult.Success(new ApiResponse(true, "Successfully added the feedback"));
            });
        }
        [HttpGet("/list-services/{id}")]
        public async Task<ActionResult> GetServiceByCategory(Guid id)
        {
            return await HandleServiceCall<ApiResponse>(async () =>
            {
                //var result = await _service.CreateNewFeedback(_mapper.Map<Feedback>(feedback));
                //if (result.IsFailed)
                //{
                //    return result;
                //}
                return ServiceResult.Success(new ApiResponse(true, "Successfully added the feedback"));
            });
        }
    }
}
