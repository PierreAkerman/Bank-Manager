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
            public string Id { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            public List<string> Roles { get; set; } = new List<string>();
        }

        public List<UserViewModel> Users { get; set; }
        public void OnGet()
        {
            Users = _context.Users.Select(e => 
                new UserViewModel
                { 
                    Id = e.Id,
                    UserName = e.UserName,
                    Email = e.Email,

                }).ToList();

            foreach (var user in Users)
            {
                foreach (var role in _context.Roles)
                {
                    if(_context.UserRoles.Any(e=>e.RoleId == role.Id && e.UserId == user.Id))
                    {
                        user.Roles.Add(role.Name);
                    }
                }
            }
            
        }
    }
}
