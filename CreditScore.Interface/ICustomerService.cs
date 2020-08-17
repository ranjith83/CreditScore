using CreditScore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CreditScore.Interface
{
   public interface ICustomerService
    {
        List<Customer> ReadAndInsertCustomer(string filePath);

        bool IsCompanyBalanceAvailable(string username, string password);
    }
}
