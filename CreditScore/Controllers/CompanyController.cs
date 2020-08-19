﻿using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        [Route("GetAllCompany")]
        public IActionResult GetCompany()
        {
            var response = _companyService.GetCompany();

            if (response == null)
                return BadRequest(new { message = "No Company found!!!" });

            return Ok(response);

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
