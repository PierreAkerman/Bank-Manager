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

        [BindProperty(SupportsGet = true)]
        public string SearchWord { get; set; }
        public class CustomerViewModel
        {
            public int Id { get; set; }
            public string PersonalId { get; set; }
            public string Givenname { get; set; }
            public string Surname { get; set; }
            public string Address { get; set; }
            public string City { get; set; }
        }

            public void OnGet(string searchWord, string col = "id", string order = "asc")
            {
                SearchWord = searchWord;
                var c = _context.Customers.AsQueryable();

                if (!string.IsNullOrEmpty(SearchWord))
                    c = c.Where(cust => cust.Givenname.Contains(SearchWord)
                                         || cust.Surname.Contains(SearchWord)
                                         || cust.City.Contains(SearchWord)
                    );

                if (col == "id")
                {
                    if (order == "asc")
                        c = c.OrderBy(cust => cust.Id);
                    else
                        c = c.OrderByDescending(cust => cust.Id);
                }
                else if (col == "nationalid")
                {
                    if (order == "asc")
                        c = c.OrderBy(cust => cust.NationalId);
                    else
                        c = c.OrderByDescending(cust => cust.NationalId);
                }
                else if (col == "customerName")
                {
                    if (order == "asc")
                        c = c.OrderBy(cust => cust.Givenname);
                    else
                        c = c.OrderByDescending(cust => cust.Givenname);
                }
                else if (col == "customerSurname")
                {
                    if (order == "asc")
                        c = c.OrderBy(cust => cust.Surname);
                    else
                        c = c.OrderByDescending(cust => cust.Surname);
                }
                else if (col == "address")
                {
                    if (order == "asc")
                        c = c.OrderBy(cust => cust.Streetaddress);
                    else
                        c = c.OrderByDescending(cust => cust.Streetaddress);
                }
                else if (col == "city")
                {
                    if (order == "asc")
                        c = c.OrderBy(cust => cust.City);
                    else
                        c = c.OrderByDescending(cust => cust.City);
                }

                Customers = c.Take(30).Select(c =>
                new CustomerViewModel
                {
                    Id = c.Id,
                    PersonalId = c.NationalId,
                    Givenname = c.Givenname,
                    Surname = c.Surname,
                    Address = c.Streetaddress,
                    City = c.City,

                }).ToList();
            }   
    }
}