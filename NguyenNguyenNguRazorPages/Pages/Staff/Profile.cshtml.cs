using BusinessObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories;
using System;

namespace NguyenNguyenNguRazorPages.Pages.Staff
{
    // Cho phép cả Staff và Lecturer vào xem Profile
    [Authorize(Roles = "Staff,Lecturer")]
    public class ProfileModel : PageModel
    {
        private readonly ISystemAccountRepo _accountRepo;
        private readonly INewsArticleRepo _newsRepo; // Tiêm INewsArticleRepo

        public ProfileModel(ISystemAccountRepo accountRepo, INewsArticleRepo newsRepo)
        {
            _accountRepo = accountRepo;
            _newsRepo = newsRepo;
        }

        [BindProperty]
        public SystemAccount AccountProfile { get; set; } = new SystemAccount();

        // Danh sách lưu trữ lịch sử viết bài của nhân viên
        public List<NewsArticle> MyNewsArticles { get; set; } = new List<NewsArticle>();

        public IActionResult OnGet()
        {
            // Trích xuất ID người dùng từ Claims Cookie
            var accountIdClaim = User.FindFirst("AccountId")?.Value;
            if (string.IsNullOrEmpty(accountIdClaim))
            {
                return RedirectToPage("/Login");
            }

            int accountId = int.Parse(accountIdClaim);

            // Lấy thông tin tài khoản từ DB
            var account = _accountRepo.GetAccountById(accountId);
            if (account == null)
            {
                return RedirectToPage("/Login");
            }

            AccountProfile = account;

            // Nếu người dùng có vai trò là Staff, tải lịch sử đăng bài
            if (User.IsInRole("Staff"))
            {
                var allArticles = _newsRepo.GetNewsArticles();
                if (allArticles != null)
                {
                    MyNewsArticles = allArticles
                        .Where(n => n.CreatedById == accountId)
                        .OrderByDescending(n => n.CreatedDate)
                        .ToList();
                }
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            try
            {
                // Gọi DB để lấy tài khoản gốc (để tránh người dùng F12 sửa ID bậy bạ)
                var existingAccount = _accountRepo.GetAccountById(AccountProfile.AccountId);
                if (existingAccount != null)
                {
                    // Chỉ cập nhật những trường được phép
                    existingAccount.AccountName = AccountProfile.AccountName;
                    existingAccount.AccountPassword = AccountProfile.AccountPassword;

                    _accountRepo.UpdateAccount(existingAccount);
                    TempData["Message"] = "Cập nhật hồ sơ cá nhân thành công!";
                }
                else
                {
                    TempData["Error"] = "Không tìm thấy tài khoản!";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra: " + ex.Message;
            }

            // Load lại trang với dữ liệu mới
            return RedirectToPage();
        }
    }
}