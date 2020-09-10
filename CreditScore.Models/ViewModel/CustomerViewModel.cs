using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using CsvHelper.Configuration;
using CsvHelper;

namespace CreditScore.Models.ViewModel
{
    
    public class CustomerViewModel
    {
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public string Cell { get; set; }

        public string IdNumber { get; set; }

        public string Address { get; set; }

        public long CompanyId { get; set; }

    }
}
