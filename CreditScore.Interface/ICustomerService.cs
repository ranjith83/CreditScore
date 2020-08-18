using CreditScore.Models;
using CreditScore.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CreditScore.Interface
{
   public interface ICustomerService
    {
        List<Customer> ReadAndInsertCustomer(string filePath);

        Tuple<long, long> IsCompanyBalanceAvailable(string username, string password);

        bool UpdateCompanyBalance(string username);
        List<CreditInquiresViewModel> AddCustomerInquiry(ScoreDataViewModel scoreDataViewModel, long customerID, long userID, long batchId);
    }
}
