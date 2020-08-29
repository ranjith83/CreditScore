using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Web;
using CreditScore.Interface;
using CreditScore.Models;
using CreditScore.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CreditScore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerService _customerService;
        private readonly IConfiguration _configuration;
        private readonly string scoreServiceURL;
        private readonly string userName;
        private readonly string password;
        private readonly string myOrigin;
        private readonly string scoreApiVersion;
        private readonly string resultType;

        //private readonly IUserService _users;
        public CustomerController(ILogger<CustomerController> logger, ICustomerService customerService,
            IConfiguration configuration)
        {
            _logger = logger;
            _customerService = customerService;
            _configuration = configuration;

            scoreServiceURL = _configuration.GetValue<string>("AppSettings:ScoreServiceURL");
            userName = _configuration.GetValue<string>("AppSettings:UserName");
            password = _configuration.GetValue<string>("AppSettings:Password");
            resultType = _configuration.GetValue<string>("AppSettings:ResultType");
            myOrigin = _configuration.GetValue<string>("AppSettings:MyOrigin");
            scoreApiVersion = _configuration.GetValue<string>("AppSettings:ScoreAPIVersion");

        }

        [HttpPost("upload"), DisableRequestSizeLimit]
        public IActionResult Upload()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources", "CustomerCSV");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fileSplit = fileName.Split('.');
                    var renamefileName = String.Join('.', fileSplit.SkipLast(1)) + "_" + DateTime.Now.ToString("yyyyMMddTHHmmss") + "." + fileName.Split('.').Last();
                    var fullPath = Path.Combine(pathToSave, renamefileName);
                    var dbPath = Path.Combine(folderName, renamefileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    var response =_customerService.ReadAndInsertCustomer(fullPath);

                    return Ok(response);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        private static readonly HttpClient client = new HttpClient();

        [HttpPost("invokeCreditScore")]
        public async Task<IActionResult>InvokeCreditScore(ScoreAPIRequest scoreAPIRequest)
        {

            List<CreditInquiresViewModel> creditInquiresViewModels = null;
            ScoreDataViewModel scoreDataView = null;

            var userCompany = _customerService.IsCompanyBalanceAvailable(scoreAPIRequest.Username);

            if (userCompany == null)
                return BadRequest(new { message = "No Balance available!!" });

            scoreDataView = await InvokeScoreAPI(scoreAPIRequest);

            if (scoreDataView != null && _customerService.UpdateCompanyBalance(scoreAPIRequest.Username))
                creditInquiresViewModels = _customerService.AddCustomerInquiry(scoreDataView, userCompany.Item1, userCompany.Item2, 12012);

            return Ok(creditInquiresViewModels);
        }

        private async Task<ScoreDataViewModel> InvokeScoreAPI(ScoreAPIRequest scoreAPI)
        {
            ScoreDataViewModel scoreDataViewModel = null;
            ScoreJsonViewModel scoreJsonViewModel = null;
            try
            {
                var builder = new UriBuilder($"{scoreServiceURL}/{userName}/{password}/{myOrigin}/{scoreApiVersion}/{resultType}/{scoreAPI.IdNumber}");
                //var builder = new UriBuilder(scoreServiceURL);
                builder.Port = 9443;
                //var query = HttpUtility.ParseQueryString(builder.Query);
                //query["pUsername"] = userName;
                //query["pPassword"] = password;
                //query["pMyOrigin"] = myOrigin;
                //query["pVersion"] = scoreApiVersion;
                //query["pResultType"] = resultType;
                //query["pIdNumber"] = iDNumber;

                // builder.Query = query.ToString();
                string url = builder.ToString();

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
                client.DefaultRequestHeaders.Add("User-Agent", ".NET Repository");

                //var stringTask = client.GetStringAsync("/95268-1/devtest/QATEST/2.0/json/2903245665081");
                var response = await client.GetStringAsync(url);
                if (response != null)
                {
                    scoreJsonViewModel = JsonConvert.DeserializeObject<ScoreJsonViewModel>(response);
                    if (!scoreJsonViewModel.hasErrors)
                        scoreDataViewModel = JsonConvert.DeserializeObject<ScoreDataViewModel>(scoreJsonViewModel.returnData);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return scoreDataViewModel;
        }

        [HttpGet]
        [Route("GetUserScore/{userId}")]
        public IActionResult GetUserScore(long userId)
        {

            var response = _customerService.GetUserScore(userId);
            return Ok(response);
        }

        [HttpGet]
        [Route("GetUserCredits/{userId}")]
        public IActionResult GetUserCredits(long userId)
        {
            if (userId <= 0)
                return BadRequest(new { message = "User Id is null" });

            var response = _customerService.GetUserCredits(userId);

            if(response == null)
                return BadRequest(new { message = "No records found" });

            return Ok(response);
        }

        [HttpGet]
        [Route("GetAllCustomer/")]
        public IActionResult GetAllCustomer()
        {
            var response = _customerService.GetAllCustomer();

            if (response == null)
                return BadRequest(new { message = "No records found" });

            return Ok(response);
        }


        [HttpPost("AddCustomer")]
        public IActionResult AddUser(CustomerViewModel customerViewModel)
        {
            var response = _customerService.AddCustomer(customerViewModel);

            if (!response)
                return BadRequest(new { message = "Customer failed to add" });

            return Ok(response);

        }

        [HttpGet]
        [Route("GetUserReports/{userId}")]
        public IActionResult GetUserReports(long userId)
        {
            if (userId <= 0)
                return BadRequest(new { message = "User Id is null" });

            var response = _customerService.GetUserReport(userId);

            if (response == null)
                return BadRequest(new { message = "No records found" });

            return Ok(response);
        }
    }
}
