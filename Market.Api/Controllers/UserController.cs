using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Market.Api.Models;
using Market.Core.Entities;
using Market.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Market.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateModel model)
        {
            var user = _userService.Authenticate(model.UserName, model.Password);

            if (user == null)
            {
                return BadRequest(new { message = "User name and/or password are incorrect!" });
            }

            return Ok(user);
        }

        //[AllowAnonymous]
        //[HttpPost("register")]
        //public async Task<IActionResult> RegisterUser([FromBody] User user)
        //{
        //}
    }
}