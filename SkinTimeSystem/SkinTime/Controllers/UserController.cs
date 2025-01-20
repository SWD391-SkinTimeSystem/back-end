using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SkinTime.BLL.Services.UserService;
using SkinTime.DAL.Entities;
using SkinTime.Helpers;
using SkinTime.Models;

namespace SkinTime.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class userController : Controller
    {
        private readonly IUserService _services;
        private readonly IMapper _mapper;
        public userController(IUserService services, IMapper mapper)
        {
            _services = services;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var allService = await _services.GetAllUsers();
            var user = _mapper.Map<List<UserAddWithRole>>(allService);
            return Ok(user);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {

            var user = await _services.GetUser(id);

            if (user == null)
            {
                return NotFound("thằng này méo tồn tại");
            }
            return Ok(_mapper.Map<UserAdd>(user));
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserAddTest userAdd)
        {
           var user = _mapper.Map<User>(userAdd);
            await _services.CrateUser(user);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            await _services.DeleteUser(id);
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpadteUser(string id,[FromBody] UserAdd user)
        {           
            var userUpdate = _mapper.Map<User>(user);
            await _services.UpadteUser(id,userUpdate);
            return Ok();
        }
    }
}
