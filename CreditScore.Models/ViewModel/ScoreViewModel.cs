using System;
using System.Collections.Generic;
using System.Text;

namespace CreditScore.Models.ViewModel
{
    public class ScoreViewModel
    {
        public bool transactionCompleted { get; set; }
        public bool hasErrors { get; set; }
        public string errorCode { get; set; }
        public string errorDescription { get; set; }
        public string returnData { get; set; }
    }

    public class Reason
    {
        public string reasonCode { get; set; }
        public string reasonDescription { get; set; }
    }

    public class Result
    {
        public string resultType { get; set; }
        public string score { get; set; }
        public List<Reason> reasons { get; set; }
    }

    public class ReturnData
    {
        public string idNumber { get; set; }
        public List<Result> results { get; set; }
    }


}
