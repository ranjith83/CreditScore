using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using CreditScore.Models;
using CreditScore.Interface;
using CreditScore.Models.ViewModel;

namespace CreditScore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CompanyController : ControllerBase
    {
        private ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }


        [HttpPost("AddCompany")]
        public IActionResult AddUser(CompanyViewModel companyViewModel)
        {
            var response = _companyService.AddCompany(companyViewModel);

            if (response == null)
                return BadRequest(new { message = "Company failed" });

            return Ok(response);

        }

        //[HttpGet]
        //[Route("GetUserByCompany/{companyID}")]
        //public IActionResult Get(long companyID)
        //{
        //    var response = _userService.GetUsers(companyID);

        //    if (response == null)
        //        return BadRequest(new { message = "No Users found!!!" });

        //    return Ok(response);

        //}

       
    }
}
