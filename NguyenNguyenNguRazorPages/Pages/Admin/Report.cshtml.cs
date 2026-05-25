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
    // Bắt buộc chỉ Admin mới được vào xem báo cáo
    [Authorize(Roles = "Admin")]
    public class ReportModel : PageModel
    {
        private readonly INewsArticleRepo _newsRepo;

        public ReportModel(INewsArticleRepo newsRepo)
        {
            _newsRepo = newsRepo;
        }

        public List<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();

        [BindProperty(SupportsGet = true)]
        public DateTime? StartDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? EndDate { get; set; }

        public void OnGet()
        {
            // Lấy toàn bộ tin tức từ Database
            var query = _newsRepo.GetNewsArticles().AsQueryable();

            // 1. Lọc theo StartDate (Lớn hơn hoặc bằng)
            if (StartDate.HasValue)
            {
                query = query.Where(n => n.CreatedDate >= StartDate.Value);
            }

            // 2. Lọc theo EndDate (Nhỏ hơn hoặc bằng)
            if (EndDate.HasValue)
            {
                query = query.Where(n => n.CreatedDate <= EndDate.Value);
            }

            // 3. Sắp xếp giảm dần theo ngày tạo (để bài viết mới nhất lên đầu)
            NewsArticles = query.OrderByDescending(n => n.CreatedDate).ToList();
        }
    }
}