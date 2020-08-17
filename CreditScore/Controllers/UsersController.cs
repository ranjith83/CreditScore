using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using CreditScore.Models;
using CreditScore.Interface;

namespace CreditScore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var response = _userService.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }

        //[Authorize]
        //[HttpGet]
        //public IActionResult GetAll()
        //{
        //    var users = _userService.GetAll();
        //    return Ok(users);
        //}

        [HttpPost("AddUser")]
        public IActionResult AddUser(UserDetail userDetail)
        {
            var response = _userService.AddUsers(userDetail);

            if (response == null)
                return BadRequest(new { message = "Username added successfully" });

            return Ok(response);

        }

        [HttpGet]
        [Route("GetUserByCompany/{companyID}")]
        public IActionResult Get(long companyID)
        {
            var response = _userService.GetUsers(companyID);

            if (response == null)
                return BadRequest(new { message = "No Users found!!!" });

            return Ok(response);

        }

       
    }
}
