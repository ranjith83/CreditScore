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

       
        public Company AddUpdateCompany(CompanyViewModel companyViewModel)
        {
            if (companyViewModel == null)
                return null;

            var company = _context.Company.SingleOrDefault(s => s.Id == companyViewModel.Id);
            
            if (company == null)
                company = new Company();

            company.Id = companyViewModel.Id;
            company.Name = companyViewModel.Name;
            company.Telephone = companyViewModel.Telephone;
            company.Address = companyViewModel.Address;
            company.Balance = companyViewModel.Balance;
            company.CreatedBy = 1;

            if (company.Id <= 0)
                _context.Company.Add(company);
           
            _context.SaveChanges();

            return company;
        }

        public List<Company> GetCompany()
        {
            var company = (from comp in _context.Company
                           select comp)
                            .ToList();
                           
            return company;

        }

        public dynamic GetCompanyForSelect()
        {
            var company = (from comp in _context.Company
                           select new { comp.Id,comp.Name})
                           .ToList();

            return company;
        }


    }
}
