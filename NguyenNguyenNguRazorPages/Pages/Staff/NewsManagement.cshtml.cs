using BusinessObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using NguyenNguyenNguRazorPages.Hubs;
using Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NguyenNguyenNguRazorPages.Pages.Staff
{
    [Authorize(Roles = "Staff")]
    public class NewsManagementModel : PageModel
    {
        private readonly INewsArticleRepo _newsRepo;
        private readonly ICategoryRepo _categoryRepo;
        private readonly ITagRepo _tagRepo; // Tiêm ITagRepo
        private readonly IHubContext<NewsHub> _hubContext; // Tiêm SignalR Hub vào đây

        public NewsManagementModel(INewsArticleRepo newsRepo, ICategoryRepo categoryRepo, ITagRepo tagRepo, IHubContext<NewsHub> hubContext)
        {
            _newsRepo = newsRepo;
            _categoryRepo = categoryRepo;
            _tagRepo = tagRepo;
            _hubContext = hubContext;
        }

        public List<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Tag> Tags { get; set; } = new List<Tag>(); // Danh sách Tag để hiển thị chọn

        [BindProperty]
        public NewsArticle FormArticle { get; set; } = new NewsArticle();

        [BindProperty]
        public List<int> SelectedTagIds { get; set; } = new List<int>(); // Lưu danh sách Tag được chọn

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        public void OnGet()
        {
            var list = _newsRepo.GetNewsArticles() ?? new List<NewsArticle>();
            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                list = list.FindAll(n => 
                    (n.NewsTitle != null && n.NewsTitle.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (n.Headline != null && n.Headline.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (n.NewsSource != null && n.NewsSource.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
                );
            }
            NewsArticles = list;
            var allCategories = _categoryRepo.GetCategories() ?? new List<Category>();
            Categories = allCategories.FindAll(c => c.IsActive == true);
            Tags = _tagRepo.GetTags() ?? new List<Tag>();
        }

        public async Task<IActionResult> OnPostSaveAsync(bool IsEditMode)
        {
            try
            {
                string? accountIdClaim = User.FindFirst("AccountId")?.Value;
                short staffId = string.IsNullOrEmpty(accountIdClaim) ? (short)0 : short.Parse(accountIdClaim);

                if (!IsEditMode)
                {
                    // TẠO MỚI TIN TỨC
                    FormArticle.CreatedById = staffId;
                    FormArticle.CreatedDate = DateTime.Now;
                    FormArticle.NewsStatus = true; // Mặc định kích hoạt công khai

                    _newsRepo.AddNewsArticle(FormArticle, SelectedTagIds);
                    TempData["Message"] = "Đăng bài viết mới thành công!";
                }
                else
                {
                    // CẬP NHẬT TIN TỨC
                    var existingArticle = _newsRepo.GetNewsArticleById(FormArticle.NewsArticleId);
                    if (existingArticle != null)
                    {
                        existingArticle.NewsTitle = FormArticle.NewsTitle;
                        existingArticle.Headline = FormArticle.Headline;
                        existingArticle.NewsContent = FormArticle.NewsContent;
                        existingArticle.NewsSource = FormArticle.NewsSource;
                        existingArticle.CategoryId = FormArticle.CategoryId;
                        existingArticle.NewsStatus = FormArticle.NewsStatus;
                        existingArticle.UpdatedById = staffId;
                        existingArticle.ModifiedDate = DateTime.Now;

                        _newsRepo.UpdateNewsArticle(existingArticle, SelectedTagIds);
                        TempData["Message"] = "Cập nhật bài viết thành công!";
                    }
                }

                // 🔥 BẮN TÍN HIỆU REAL-TIME: Báo cho toàn bộ các máy đang mở web biết để tự load lại dữ liệu
                await _hubContext.Clients.All.SendAsync("LoadNewsRealtime");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra: " + ex.Message;
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            try
            {
                _newsRepo.DeleteNewsArticle(id);
                TempData["Message"] = "Đã xóa bài báo khỏi hệ thống!";

                // 🔥 BẮN TÍN HIỆU REAL-TIME SAU KHI XÓA
                await _hubContext.Clients.All.SendAsync("LoadNewsRealtime");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Không thể xóa bài viết: " + ex.Message;
            }
            return RedirectToPage();
        }
    }
}