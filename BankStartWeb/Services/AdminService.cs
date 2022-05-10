using BankStartWeb.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BankStartWeb.Services
{
    public interface IAdminService
    {
        void CreateUser(string email, string password, string[] roles);
    }

    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminService(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public void CreateUser(string email, string password, string[] roles)
        {
            if (_userManager.FindByEmailAsync(email).Result != null) return;

            var user = new IdentityUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
            };

            _userManager.CreateAsync(user, password).Wait();
            _userManager.AddToRolesAsync(user, roles).Wait();
        }
    }
}