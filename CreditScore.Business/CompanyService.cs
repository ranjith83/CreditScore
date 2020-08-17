using CreditScore.Interface;
using System.Linq;
using System;
using System.Collections.Generic;
using CreditScore.Models;
using System.Security.Claims;
using CreditScore.Models.ViewModel;

namespace CreditScore.Business
{
    public class CompanyService : ICompanyService
    {
        private readonly DatabaseContext _context;

        public CompanyService(DatabaseContext context)
        {
            _context = context;
        }

       
        public Company AddCompany(CompanyViewModel companyViewModel)
        {
            if (companyViewModel == null)
                return null;

            Company company = new Company();
            company.Name = companyViewModel.Name;
            company.Telephone = companyViewModel.Telephone;
            company.Address = companyViewModel.Address;
            company.Balance = companyViewModel.Balance;
            company.CreatedBy = 1;

            _context.Company.Add(company);
            _context.SaveChanges();

            return company;

        }

        

       
    }
}
