using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CreditScore.Models.ViewModel
{
    
    public class CustomerViewModel
    {
        [Index(0)]
        public string FirstName { get; set; }
        [Index(1)]
        public string SurName { get; set; }
        [Index(2)]
        public string Cell { get; set; }

        [Index(3)]
        public string IdNumber { get; set; }

    }
}
