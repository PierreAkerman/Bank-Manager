using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BankStartWeb.Data;
using BankStartWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BankStartWeb.Pages.Admin
{
    public class RegisterUserModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IAdminService _adminService;

        public RegisterUserModel(ApplicationDbContext context, IAdminService adminService)
        {
            _context = context;
            _adminService = adminService;
        }

        [Required]
        [EmailAddress]
        [BindProperty]
        public string Email { get; set; }
        [Required]
        [BindProperty]
        public string Password { get; set; }
        [Required]
        [BindProperty]
        public string[] SelectedRoles { get; set; }
        public List<SelectListItem> Roles { get; set; }

        public void UpdateRoles()
        {
            Roles = _context.Roles.Select(e => new SelectListItem()
            {
                Text = e.Name,
                Value = e.Name,

            }).ToList();
        }

        public void OnGet()
        {
            UpdateRoles();
        }

        public IActionResult OnPost(string email, string password, string[] roles)
        {
            if (!ModelState.IsValid)
            {
                UpdateRoles();
                return Page();
            }

            _adminService.CreateUser(email, password, roles);

            return RedirectToPage("/Index");
        }
    }
}
