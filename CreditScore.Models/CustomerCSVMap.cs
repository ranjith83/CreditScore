using CreditScore.Models.ViewModel;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CreditScore.Models
{
    public sealed class CustomerCSVMap : ClassMap<CustomerViewModel>
    {
        public CustomerCSVMap()
        {
            Map(m => m.FirstName).Name("FirstName");
            Map(m => m.SurName).Name("SurName");
            Map(m => m.Cell).Name("cell");
            Map(m => m.IdNumber).Name("IdNumber");
           // Map(m => m.FirstName).Name("Address");
        }
    }
   
}
