using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NguyenNguyenNguRazorPages.Pages
{
    // Bùa hộ mệnh: Ai chưa đăng nhập mà vào đây sẽ bị tự động chuyển hướng sang /Login
    [Authorize]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
            // Tạm thời chưa code gì ở đây, sau này mình sẽ load Dashboard sau
        }
    }
}