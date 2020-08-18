using System;
using System.Collections.Generic;
using System.Text;

namespace CreditScore.Models.ViewModel
{

    public class ScoreDataViewModel
    {
        public string idNumber { get; set; }
        public List<Result> results { get; set; }
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
    
}
