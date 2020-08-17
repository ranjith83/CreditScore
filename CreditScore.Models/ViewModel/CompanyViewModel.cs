using System;
using System.Collections.Generic;
using System.Text;

namespace CreditScore.Models.ViewModel
{
    public class CompanyViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public int Balance { get; set; }
    }
}
