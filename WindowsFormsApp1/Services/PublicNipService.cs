using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InvoiceApp.Services
{
    public class PublicNipService
    {
        private readonly Dictionary<string, CompanyData> fakeDatabase = new Dictionary<string, CompanyData>
        {
            { "1234567890", new CompanyData { Name = "Firma Testowa", Surname ="Abacki", Email ="abacki123@gmail.com", Street = "Ogrodowa 1", City = "Warszawa", PostalCode = "00-001", PhoneNumber = "123 456 789" } },
            { "9876543210", new CompanyData { Name = "Druga Firma", Surname ="Cabacki", Email ="cabacki987@gmail.com", Street = "Druga 2", City = "Kraków", PostalCode = "31-002", PhoneNumber = "987 654 321" } }
        };


        public Task<CompanyData> GetCompanyData(string nip)
        {
            if (fakeDatabase.TryGetValue(nip, out var companyData))
            {
                return Task.FromResult(companyData);
            }

            throw new Exception("Company not found in local database.");
        }
    }
}
