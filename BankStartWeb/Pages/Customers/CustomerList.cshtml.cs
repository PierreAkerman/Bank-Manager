using BankStartWeb.Data;
using BankStartWeb.Infrastructure.Paging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BankStartWeb.Pages.Customers
{
    public class CustomerListModel : PageModel
    {
        public List<CustomerViewModel> Customers = new();
        private readonly ApplicationDbContext _context;

        public CustomerListModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public string SearchWord { get; set; }
        public string SortOrder { get; set; }
        public string SortCol { get; set; }
        public int PageNo { get; set; }
        public int TotalPageCount { get; set; }

        public class CustomerViewModel
        {
            public int Id { get; set; }
            public string NationalId { get; set; }
            public string Givenname { get; set; }
            public string Surname { get; set; }
            public string Address { get; set; }
            public string City { get; set; }
        }

        public void OnGet(string searchWord, string col = "Id", string order = "asc", int pageno = 1)
        {
            SearchWord = searchWord;
            SortCol = col;
            SortOrder = order;
            PageNo = pageno;

            var customer = _context.Customers.AsQueryable();
            if (!string.IsNullOrEmpty(SearchWord))
                customer = customer.Where(cust => cust.Givenname.Contains(SearchWord)
                                                ||cust.Surname.Contains(SearchWord)
                                                ||cust.City.Contains(SearchWord)
                );

            customer = customer.OrderBy(col, order == "asc" ? ExtensionMethods.QuerySortOrder.Asc : ExtensionMethods.QuerySortOrder.Desc);

            var pageResult = customer.GetPaged(PageNo, 50);
            TotalPageCount = pageResult.PageCount;

            Customers = pageResult.Results.Select(customer => new CustomerViewModel
            {
                Id = customer.Id,
                NationalId = customer.NationalId,
                Givenname = customer.Givenname,
                Surname = customer.Surname,
                Address = customer.Streetaddress,
                City = customer.City

            }).ToList();


            //if (col == "id")
            //{
            //    if (order == "asc")
            //        c = c.OrderBy(cust => cust.Id);
            //    else
            //        c = c.OrderByDescending(cust => cust.Id);
            //}
            //else if (col == "nationalid")
            //{
            //    if (order == "asc")
            //        c = c.OrderBy(cust => cust.NationalId);
            //    else
            //        c = c.OrderByDescending(cust => cust.NationalId);
            //}
            //else if (col == "customerName")
            //{
            //    if (order == "asc")
            //        c = c.OrderBy(cust => cust.Givenname);
            //    else
            //        c = c.OrderByDescending(cust => cust.Givenname);
            //}
            //else if (col == "customerSurname")
            //{
            //    if (order == "asc")
            //        c = c.OrderBy(cust => cust.Surname);
            //    else
            //        c = c.OrderByDescending(cust => cust.Surname);
            //}
            //else if (col == "address")
            //{
            //    if (order == "asc")
            //        c = c.OrderBy(cust => cust.Streetaddress);
            //    else
            //        c = c.OrderByDescending(cust => cust.Streetaddress);
            //}
            //else if (col == "city")
            //{
            //    if (order == "asc")
            //        c = c.OrderBy(cust => cust.City);
            //    else
            //        c = c.OrderByDescending(cust => cust.City);
            //}
        }   
    }
}