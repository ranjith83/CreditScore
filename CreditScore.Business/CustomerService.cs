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
using CsvHelper.Configuration;

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

        public List<Customer> ReadAndInsertCustomer(string filePath, long customerId)
        {
            try
            {

                return ReadCSV(filePath, customerId);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private List<Customer> ReadCSV(string filePath, long companyId)
        {


            IEnumerable<CustomerViewModel> customerViewModel = null;
            List<Customer> customers = new List<Customer>();
            using (var reader = new StreamReader(filePath))
            {
                CsvConfiguration configuration = new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = true,HeaderValidated = null };

                var csvReader = new CsvReader(reader, configuration); // CultureInfo.InvariantCulture);
                // csvReader.Configuration.PrepareHeaderForMatch = (header, index) => header.ToLower();
                //csvReader.Configuration.HeaderValidated = null; // (header, index) => header.ToLower();
                csvReader.Configuration.RegisterClassMap<CustomerCSVMap>();
                csvReader.Configuration.HeaderValidated = null;
                csvReader.Configuration.MissingFieldFound = null;
                customerViewModel = csvReader.GetRecords<CustomerViewModel>();

                foreach (var customerModel in customerViewModel)
                {
                    var customer = new Customer();
                    customer.FirstName = customerModel.FirstName;
                    customer.SurName = customerModel.SurName;
                    customer.Cell = Convert.ToInt64(customerModel.Cell);
                    customer.IDNumber = customerModel.IdNumber;
                    customer.CompanyId = companyId;
                    customer.CreatedDate = DateTime.Now;
                    //customer.CreatedBy = 1;

                    customers.Add(customer);
                    //_context.Customer.Add(customer);
                }
                _context.BulkInsert(customers);
                //_context.SaveChanges();

            }

            return customers;
        }


        public Tuple<long, long> IsCompanyBalanceAvailable(string username)
        {
            Tuple<long, long> returnVal = null;

            var userCompany = (from company in _context.Company
                               join user in _context.User on company.Id equals user.CompanyId
                               where user.UserName == username
                               select new { company.Balance, companyId = company.Id, UserId = user.Id, user.PasswordHash }).FirstOrDefault();

            if (userCompany != null && userCompany.PasswordHash != String.Empty)
                if (userCompany.Balance >= 1)
                    returnVal = Tuple.Create(userCompany.companyId, userCompany.UserId);
            //   if (_userService.VerifyPassword(userCompany.PasswordHash, password))


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

        public long GetUserScore(long userID)
        {
            long score = 0;
            if (userID > 0)
            {
                score = (from inq in _context.CreditInquires
                             where inq.UserID == userID
                             orderby inq.CreatedDate descending
                             select inq.Score).FirstOrDefault();

            }
            return score;
        }
        public List<CreditInquiresViewModel> GetUserCredits(long userID)
        {
            List<CreditInquiresViewModel> creditInquires = null;
            if (userID > 0)
            {
              var  creditInq = (from inq in _context.CreditInquires
                         where inq.UserID == userID
                         select inq).ToList();

                creditInquires = _mapper.Map<List<CreditInquiresViewModel>>(creditInq);

            }
            return creditInquires;
        }

        public List<CustomerViewModel> GetAllCustomer()
        {
            List<CustomerViewModel> customerVM = null;
            var customer = (from cust in _context.Customer
                            select cust).ToList();

            customerVM = _mapper.Map<List<CustomerViewModel>>(customer);

            return customerVM;
        }

        public CustomerViewModel GetCustomer(string idNumber)
        {
            CustomerViewModel customerVM = null;
            var customer = (from cust in _context.Customer
                            where cust.IDNumber == idNumber
                            select cust).SingleOrDefault();

            customerVM = _mapper.Map<CustomerViewModel>(customer);

            return customerVM;
        }

        public bool AddCustomer(CustomerViewModel customerViewModel)
        {
           var customer = _mapper.Map<Customer>(customerViewModel);

            _context.Customer.Add(customer);
            var custInsert = _context.SaveChanges();
            return custInsert > 0;
        }

        public List<CreditInquiresViewModel> GetUserReport(long userID)
        {
            List<CreditInquiresViewModel> creditInquires = null;
            if (userID > 0)
            {
                List<CreditInquires> creditInq = (from inq in _context.CreditInquires
                                 join cust in  _context.Customer  on inq.CustomerID equals cust.Id
                                 where inq.UserID == userID
                                 select new CreditInquires { 
                                     Customer = cust,
                                     BatchID = inq.BatchID,
                                     Description1 = inq.Description1,
                                     Description2 = inq.Description2,
                                     Description3 = inq.Description3,
                                     Description4 = inq.Description4,
                                     Description5 = inq.Description5,
                                     Score = inq.Score,
                                     Success = inq.Success,
                                     ReasonCode1 = inq.ReasonCode1,
                                     ReasonCode2 = inq.ReasonCode2,
                                     ReasonCode3 = inq.ReasonCode3,
                                     ReasonCode4 = inq.ReasonCode4,
                                     ReasonCode5 = inq.ReasonCode5
                                 }).ToList();

                creditInquires = _mapper.Map<List<CreditInquiresViewModel>>(creditInq);

            }
            return creditInquires;
        }

    }
}
