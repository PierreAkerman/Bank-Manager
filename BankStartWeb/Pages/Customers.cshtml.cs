using BankStartWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BankStartWeb.Pages
{
    public class CustomersModel : PageModel
    {
        public List<CustomerViewModel> Customers = new();

        private readonly ApplicationDbContext _context;

        public CustomersModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public class CustomerViewModel
        {
            public int Id { get; set; }
            public string Givenname { get; set; }
            public string Surname { get; set; }
            public string City { get; set; }
            public string Country { get; set; }
        }

        

        public void OnGet()
        {
            Customers = _context.Customers.Take(30).Select(c =>
            new CustomerViewModel
            {
                Id = c.Id,
                Givenname = c.Givenname,
                Surname = c.Surname,
                City = c.City,
                Country = c.Country
            }).ToList();
        }
    }
}