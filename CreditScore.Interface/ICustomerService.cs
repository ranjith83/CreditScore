using CreditScore.Models;
using CreditScore.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CreditScore.Interface
{
   public interface ICustomerService
    {
        List<Customer> ReadAndInsertCustomer(string filePath, long customerId);

        Tuple<long, long> IsCompanyBalanceAvailable(string username);

        bool UpdateCompanyBalance(string username);
        List<CreditInquiresViewModel> AddCustomerInquiry(ScoreDataViewModel scoreDataViewModel, long customerID, long userID, long batchId);

        long GetUserScore(long userID);

        List<CreditInquiresViewModel> GetUserCredits(long userID);

        List<CustomerViewModel> GetAllCustomer();

        bool AddCustomer(CustomerViewModel customerViewModel);

        List<CreditInquiresViewModel> GetUserReport(long userID);

        CustomerViewModel GetCustomer(string idNumber);
    }
}
