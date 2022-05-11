using BankStartWeb.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BankStartWeb.Pages.Admin
{
    public class UpdateUserModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public UpdateUserModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public string Id { get; set; }
        [BindProperty]
        public string Email { get; set; }
        public List<SelectListItem> AllRoles { get; set; } = new List<SelectListItem>();
        [BindProperty]
        public List<string> Roles { get; set; } = new List<string>() { };

        public void OnGet(string id)
        {
            var user = _userManager.FindByIdAsync(id).Result;
            Email = user.Email;

            foreach (var role in _context.Roles)
            {
                AllRoles.Add(new SelectListItem
                {
                    Text = role.Name,
                    Value = role.Name
                });
                if (_context.UserRoles.Any(e => e.RoleId == role.Id && e.UserId == user.Id))
                {
                    Roles.Add(role.Name);
                }
            }
        }

        public IActionResult OnPost(string id)
        {
            var user = _userManager.FindByIdAsync(id).Result;

            foreach (var role in _context.Roles)
            {
                AllRoles.Add(new SelectListItem
                {
                    Text = role.Name,
                    Value = role.Name
                });
            }

            if (ModelState.IsValid)
            {
                user.Email = Email;
                foreach (var r in _context.UserRoles.Where(u => u.UserId == id))
                {
                    _context.UserRoles.Remove(r);
                    
                }
                _context.SaveChanges();
                _userManager.AddToRolesAsync(user, Roles).Wait();

                return RedirectToPage("/Index");
            }
            return Page();
        }
    }
}
