using BusinessObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories;
using System.Collections.Generic;
using System.Linq;

namespace NguyenNguyenNguRazorPages.Pages.News
{
    // Mở cổng công khai: Cho phép tất cả mọi người đọc tin tức (kể cả khách chưa đăng nhập)
    public class IndexModel : PageModel
    {
        private readonly INewsArticleRepo _newsRepo;

        public IndexModel(INewsArticleRepo newsRepo)
        {
            _newsRepo = newsRepo;
        }

        public List<NewsArticle> ActiveNewsList { get; set; } = new List<NewsArticle>();

        public void OnGet()
        {
            // Lấy toàn bộ bài viết, lọc ra những bài Active và sắp xếp giảm dần theo ngày tạo
            var allArticles = _newsRepo.GetNewsArticles();
            if (allArticles != null)
            {
                ActiveNewsList = allArticles
                    .Where(n => n.NewsStatus == true)
                    .OrderByDescending(n => n.CreatedDate)
                    .ToList();
            }
        }
    }
}