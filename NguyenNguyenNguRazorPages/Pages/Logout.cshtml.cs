using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace NguyenNguyenNguRazorPages.Pages
{
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult>
        OnGet()
        {
            // Xóa Cookie đăng nhập khỏi trình duyệt
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Đá người dùng về lại trang Login
            return RedirectToPage("/Login");
        }
    }
}