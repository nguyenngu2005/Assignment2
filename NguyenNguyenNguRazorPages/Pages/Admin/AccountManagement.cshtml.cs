using BusinessObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NguyenNguyenNguRazorPages.Pages.Admin
{
    [Authorize(Roles = "Admin")] // Chặn tất cả các role khác truy cập trực tiếp qua URL
    public class AccountManagementModel : PageModel
    {
        private readonly ISystemAccountRepo _accountRepo;

        public AccountManagementModel(ISystemAccountRepo accountRepo)
        {
            _accountRepo = accountRepo;
        }

        public List<SystemAccount> Accounts { get; set; } = new List<SystemAccount>();

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        [BindProperty]
        public SystemAccount FormAccount { get; set; } = new SystemAccount();

        public void OnGet()
        {
            var list = _accountRepo.GetAccounts();

            // Thực hiện hành động Tìm kiếm bằng LINQ nếu có từ khóa
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                list = list.Where(a =>
                    (a.AccountName != null && a.AccountName.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (a.AccountEmail != null && a.AccountEmail.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }

            Accounts = list;
        }

        // Xử lý cả Thêm mới và Cập nhật dựa vào ID ẩn truyền lên từ Popup
        public IActionResult OnPostSave()
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Dữ liệu nhập vào không hợp lệ!";
                return RedirectToPage();
            }

            try
            {
                if (FormAccount.AccountId == 0)
                {
                    // Sinh mã ID ngẫu nhiên hoặc tăng tự động nếu DB chưa cấu hình Identity cụ thể
                    Random rnd = new Random();
                    FormAccount.AccountId = (short)rnd.Next(1000, 9999);

                    _accountRepo.AddAccount(FormAccount);
                    TempData["Message"] = "Thêm tài khoản mới thành công!";
                }
                else
                {
                    _accountRepo.UpdateAccount(FormAccount);
                    TempData["Message"] = "Cập nhật tài khoản thành công!";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra: " + ex.Message;
            }

            return RedirectToPage();
        }

        // Xử lý Xóa tài khoản có kèm ID
        public IActionResult OnPostDelete(int id)
        {
            try
            {
                _accountRepo.DeleteAccount(id);
                TempData["Message"] = "Xóa tài khoản thành công!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Không thể xóa tài khoản này: " + ex.Message;
            }

            return RedirectToPage();
        }
    }
}