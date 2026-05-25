using BusinessObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories;
using System;
using System.Collections.Generic;

namespace NguyenNguyenNguRazorPages.Pages.Staff
{
    // Bắt buộc quyền Staff mới được truy cập 
    [Authorize(Roles = "Staff")]
    public class CategoryManagementModel : PageModel
    {
        private readonly ICategoryRepo _categoryRepo;

        public CategoryManagementModel(ICategoryRepo categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public List<Category> Categories { get; set; } = new List<Category>();

        [BindProperty]
        public Category FormCategory { get; set; } = new Category();

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        public void OnGet()
        {
            var list = _categoryRepo.GetCategories() ?? new List<Category>();
            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                list = list.FindAll(c => 
                    (c.CategoryName != null && c.CategoryName.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (c.CategoryDesciption != null && c.CategoryDesciption.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
                );
            }
            Categories = list;
        }

        public IActionResult OnPostSave()
        {
            try
            {
                if (FormCategory.CategoryId == 0)
                {
                    // Tạo mới
                    _categoryRepo.AddCategory(FormCategory);
                    TempData["Message"] = "Thêm chuyên mục mới thành công!";
                }
                else
                {
                    // Cập nhật
                    _categoryRepo.UpdateCategory(FormCategory);
                    TempData["Message"] = "Cập nhật chuyên mục thành công!";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi khi lưu: " + ex.Message;
            }

            return RedirectToPage();
        }

        public IActionResult OnPostDelete(short id)
        {
            try
            {
                _categoryRepo.DeleteCategory(id);
                TempData["Message"] = "Xóa chuyên mục thành công!";
            }
            catch (Exception ex)
            {
                // Bắt lỗi Exception được ném ra từ DAO nếu Category đang chứa bài viết
                TempData["Error"] = ex.Message;
            }

            return RedirectToPage();
        }
    }
}