using System;
using System.Collections.Generic;
using System.Text;

namespace CreditScore.Models.ViewModel
{
    public class ScoreJsonViewModel
    {
        public bool transactionCompleted { get; set; }
        public bool hasErrors { get; set; }
        public string errorCode { get; set; }
        public string errorDescription { get; set; }
        public string returnData { get; set; }
    }



}
