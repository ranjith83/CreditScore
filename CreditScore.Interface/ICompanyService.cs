﻿using CreditScore.Models;
using CreditScore.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CreditScore.Interface
{
    public interface ICompanyService
    {
        Company AddCompany(CompanyViewModel companyViewModel);

        List<Company> GetCompany();

        dynamic GetCompanyForSelect();
    }
}
