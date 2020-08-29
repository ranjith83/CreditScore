using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using CreditScore.Models;
using CreditScore.Interface;
using CreditScore.Models.ViewModel;
using Microsoft.Extensions.Logging;

namespace CreditScore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CompanyController : ControllerBase
    {
        private ICompanyService _companyService;
        private ILogger<CompanyController> _logger;
        public CompanyController(ICompanyService companyService, ILogger<CompanyController> logger)
        {
            _companyService = companyService;
            _logger = logger;
        }


        [HttpPost("AddCompany")]
        public IActionResult AddUser(CompanyViewModel companyViewModel)
        {
            var response = _companyService.AddCompany(companyViewModel);

            if (response == null)
                return BadRequest(new { message = "Company failed" });

            return Ok(response);

        }

        [HttpGet]
        [Route("GetAllCompany")]
        public IActionResult GetCompany()
        {
            try
            {
                _logger.LogInformation("Get Company detail starts");
                var response = _companyService.GetCompany();

                if (response == null)
                    return BadRequest(new { message = "No Company found!!!" });

                return Ok(response);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(new { message = "No Company found!!!" });
            }
        }


        [HttpGet]
        [Route("GetCompanyForSelect")]
        public IActionResult GetCompanyForSelect()
        {
            var response = _companyService.GetCompanyForSelect();

            if (response == null)
                return BadRequest(new { message = "No Company found!!!" });

            return Ok(response);

        }

       
    }

}
