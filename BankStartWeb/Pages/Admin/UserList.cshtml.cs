using BankStartWeb.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.EntityFrameworkCore;

namespace BankStartWeb.Pages.Admin
{
    public class UserListModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public UserListModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public class UserViewModel
        {
            public string UserName { get; set; }
            public string Email { get; set; }
            public List<string> Roles { get; set; }
        }

        public List<UserViewModel> Users { get; set; }
        public void OnGet()
        {
            Users = _context.Users.Select(e => 
                new UserViewModel
                {
                    UserName = e.UserName,
                    Email = e.Email,
                    //Roles = (List<string>)_context.Roles.Select(r => r.Name)

                }).ToList();

            
            
        }
    }
}
