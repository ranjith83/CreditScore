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
        private readonly string iDNumber;
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
            iDNumber = _configuration.GetValue<string>("AppSettings:IdNumber");
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

        [HttpPost("InvokeScore")]
        public async Task<IActionResult> InvokeScore(AuthenticateRequest authenticateRequest )
        {

            object isBalance = null;

            if( _customerService.IsCompanyBalanceAvailable(authenticateRequest.Username, authenticateRequest.Password))
                isBalance = await InvokeScoreAPI(authenticateRequest.Username, authenticateRequest.Password);

            return Ok(isBalance);
        }

        private async Task<object> InvokeScoreAPI(string userName, string password)
        {
            ScoreViewModel jsonResponse = null;

            try
            {
                var builder = new UriBuilder($"{scoreServiceURL}/{userName}/{password}/{myOrigin}/{scoreApiVersion}/{resultType}/{iDNumber}");
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
                client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

                //var stringTask = client.GetStringAsync("/95268-1/devtest/QATEST/2.0/json/2903245665081");
                var response = await client.GetStringAsync(url);
                jsonResponse = JsonConvert.DeserializeObject<ScoreViewModel>(response);
                var jsonReturnRes = JsonConvert.DeserializeObject<ReturnData>(jsonResponse.returnData); 
                //response.EnsureSuccessStatusCode();
                //string responseBody = await response.Content.ReadAsync<ScoreViewModel>();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return jsonResponse;
        }
    }
}
