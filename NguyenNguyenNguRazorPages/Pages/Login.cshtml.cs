using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Repositories;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NguyenNguyenNguRazorPages.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ISystemAccountRepo _accountRepo;
        private readonly IConfiguration _configuration;

        // Tiêm Dependency
        public LoginModel(ISystemAccountRepo accountRepo, IConfiguration configuration)
        {
            _accountRepo = accountRepo;
            _configuration = configuration;
        }

        [BindProperty]
        public string Email { get; set; } = "";

        [BindProperty]
        public string Password { get; set; } = "";

        public IActionResult OnGet()
        {
            // Nếu đã đăng nhập, đá thẳng vào trang Index
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Index");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            // 1. Kiểm tra tài khoản Admin từ appsettings.json
            string? adminEmail = _configuration["AdminAccount:Email"];
            string? adminPassword = _configuration["AdminAccount:Password"];

            if (Email == adminEmail && Password == adminPassword)
            {
                await SignInUser(Email, "Admin", "0");
                return RedirectToPage("/Index");
            }

            // 2. Kiểm tra tài khoản trong Database
            var account = _accountRepo.GetAccountByEmail(Email);
            if (account != null && account.AccountPassword == Password)
            {
                // Role: 1 = Staff, 2 = Lecturer
                string role = account.AccountRole == 1 ? "Staff" : (account.AccountRole == 2 ? "Lecturer" : "Member");
                await SignInUser(Email, role, account.AccountId.ToString());
                return RedirectToPage("/Index");
            }

            // 3. Đăng nhập thất bại
            ModelState.AddModelError("LoginError", "Email hoặc Mật khẩu không chính xác!");
            return Page();
        }

        private async Task SignInUser(string email, string role, string accountId)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role),
                new Claim("AccountId", accountId)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
        }
    }
}