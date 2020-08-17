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

namespace CreditScore.Business
{
    public class CustomerService : ICustomerService
    {
        private readonly DatabaseContext _context;
        private readonly IUserService _userService;

        //private readonly AppSettings _appSettings;

        public CustomerService(DatabaseContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
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


        public bool IsCompanyBalanceAvailable(string username, string password)
        {


            var userCompany = (from company in _context.Company
                               join user in _context.User on company.Id equals user.CompanyId
                               where user.UserName == username
                               select new { company.Balance, user.PasswordHash }).FirstOrDefault();

            if (userCompany != null && userCompany.PasswordHash != String.Empty)
                if (_userService.VerifyPassword(userCompany.PasswordHash, password))
                    if (userCompany.Balance >= 1)
                        return true;

            return false;
        }
    }
}
