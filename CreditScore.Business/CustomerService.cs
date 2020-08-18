using CreditScore.Interface;
using CreditScore.Models;
using CreditScore.Models.ViewModel;
using CsvHelper;
using EFCore.BulkExtensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using AutoMapper;

namespace CreditScore.Business
{
    public class CustomerService : ICustomerService
    {
        private readonly DatabaseContext _context;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        //private readonly AppSettings _appSettings;

        public CustomerService(DatabaseContext context, IUserService userService, IMapper mapper)
        {
            _context = context;
            _userService = userService;
            _mapper = mapper;
        }

        public List<Customer> ReadAndInsertCustomer(string filePath)
        {
            try
            {

                return ReadCSV(filePath);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private List<Customer> ReadCSV(string filePath)
        {


            IEnumerable<CustomerViewModel> customerViewModel = null;
            List<Customer> customers = new List<Customer>();
            using (var reader = new StreamReader(filePath))
            {
                var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
                customerViewModel = csvReader.GetRecords<CustomerViewModel>();

                foreach (var customerModel in customerViewModel)
                {
                    var customer = new Customer();
                    customer.FirstName = customerModel.FirstName;
                    customer.SurName = customerModel.SurName;
                    customer.Cell = Convert.ToInt64(customerModel.Cell);
                    customer.IDNumber = customerModel.IdNumber;
                    //customer.CreatedDate = DateTime.Now;
                    //customer.CreatedBy = 1;

                    customers.Add(customer);
                    //_context.Customer.Add(customer);
                }
                _context.BulkInsert(customers);
                //_context.SaveChanges();

            }

            return customers;
        }


        public Tuple<long, long> IsCompanyBalanceAvailable(string username, string password)
        {
            Tuple<long, long> returnVal = null;

            var userCompany = (from company in _context.Company
                               join user in _context.User on company.Id equals user.CompanyId
                               where user.UserName == username
                               select new { company.Balance, companyId = company.Id, UserId = user.Id, user.PasswordHash }).FirstOrDefault();

            if (userCompany != null && userCompany.PasswordHash != String.Empty)
                if (_userService.VerifyPassword(userCompany.PasswordHash, password))
                    if (userCompany.Balance >= 1)
                        returnVal = Tuple.Create(userCompany.companyId, userCompany.UserId);

            return returnVal;
        }

        public bool UpdateCompanyBalance(string username)
        {

            var userCompany = (from company in _context.Company
                               join user in _context.User on company.Id equals user.CompanyId
                               where user.UserName == username
                               select company).FirstOrDefault();

            userCompany.Balance -= 1;
            _context.SaveChanges();
            return true;
        }

        public List<CreditInquiresViewModel> AddCustomerInquiry(ScoreDataViewModel scoreDataViewModel, long customerID, long userID, long batchId)
        {
            List<CreditInquiresViewModel> creditInquiresViewModels = new List<CreditInquiresViewModel>();
            try
            {
                if (scoreDataViewModel.results.Count > 0)
                {
                    foreach (var inquiryData in scoreDataViewModel.results)
                    {
                        CreditInquiresViewModel creditInquiresVM = new CreditInquiresViewModel();
                        Type creditInquiryType = typeof(CreditInquiresViewModel);

                        creditInquiresVM.Score = Convert.ToInt32(inquiryData.score);
                        creditInquiresVM.Success = true;
                        creditInquiresVM.CustomerID = customerID;
                        creditInquiresVM.UserID = userID;
                        creditInquiresVM.BatchID = batchId;

                        int i = 1;
                        foreach (var reason in inquiryData.reasons)
                        {
                            PropertyInfo reasonCode = creditInquiryType.GetProperty("ReasonCode" + i);
                            reasonCode.SetValue(creditInquiresVM, inquiryData.reasons[i-1].reasonCode);

                            PropertyInfo reasonDescription = creditInquiryType.GetProperty("Description" + i);
                            reasonCode.SetValue(creditInquiresVM, inquiryData.reasons[i-1].reasonDescription);

                            i++;
                        }
                        creditInquiresViewModels.Add(creditInquiresVM);
                        var creditInquiredMap = _mapper.Map<CreditInquires>(creditInquiresVM);

                        _context.CreditInquires.Add(creditInquiredMap);
                    }

                    _context.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                return creditInquiresViewModels;
            }


            return creditInquiresViewModels;
        }
    }
}
