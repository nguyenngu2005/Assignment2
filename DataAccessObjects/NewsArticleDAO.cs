using BusinessObjects;
using DataAccessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessObjects
{
    public class NewsArticleDAO
    {
        private static NewsArticleDAO? instance = null;
        private static readonly object instanceLock = new object();

        private NewsArticleDAO() { }

        public static NewsArticleDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new NewsArticleDAO();
                    }
                    return instance;
                }
            }
        }

        public List<NewsArticle> GetNewsArticles()
        {
            using var context = new FunewsManagementContext();
            // Include thêm Category và các Tag gắn liền với bài viết
            return context.NewsArticles.Include(n => n.Category).Include(n => n.Tags).ToList();
        }

        public NewsArticle? GetNewsArticleById(string id)
        {
            using var context = new FunewsManagementContext();
            return context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.Tags)
                .SingleOrDefault(n => n.NewsArticleId == id);
        }

        public void AddNewsArticle(NewsArticle newsArticle)
        {
            AddNewsArticle(newsArticle, new List<int>());
        }

        public void AddNewsArticle(NewsArticle newsArticle, List<int> tagIds)
        {
            using var context = new FunewsManagementContext();
            if (tagIds != null && tagIds.Any())
            {
                var tags = context.Tags.Where(t => tagIds.Contains(t.TagId)).ToList();
                foreach (var tag in tags)
                {
                    newsArticle.Tags.Add(tag);
                }
            }
            context.NewsArticles.Add(newsArticle);
            context.SaveChanges();
        }

        public void UpdateNewsArticle(NewsArticle newsArticle)
        {
            UpdateNewsArticle(newsArticle, new List<int>());
        }

        public void UpdateNewsArticle(NewsArticle newsArticle, List<int> tagIds)
        {
            using var context = new FunewsManagementContext();
            var existingArticle = context.NewsArticles
                .Include(n => n.Tags)
                .SingleOrDefault(n => n.NewsArticleId == newsArticle.NewsArticleId);

            if (existingArticle != null)
            {
                existingArticle.NewsTitle = newsArticle.NewsTitle;
                existingArticle.Headline = newsArticle.Headline;
                existingArticle.NewsContent = newsArticle.NewsContent;
                existingArticle.NewsSource = newsArticle.NewsSource;
                existingArticle.CategoryId = newsArticle.CategoryId;
                existingArticle.NewsStatus = newsArticle.NewsStatus;
                existingArticle.UpdatedById = newsArticle.UpdatedById;
                existingArticle.ModifiedDate = newsArticle.ModifiedDate;

                // Cập nhật quan hệ Many-to-Many với Tag
                existingArticle.Tags.Clear();
                if (tagIds != null && tagIds.Any())
                {
                    var tags = context.Tags.Where(t => tagIds.Contains(t.TagId)).ToList();
                    foreach (var tag in tags)
                    {
                        existingArticle.Tags.Add(tag);
                    }
                }

                context.SaveChanges();
            }
        }

        public void DeleteNewsArticle(string id)
        {
            using var context = new FunewsManagementContext();
            var article = context.NewsArticles.Include(n => n.Tags).SingleOrDefault(n => n.NewsArticleId == id);
            if (article != null)
            {
                // Xóa các liên kết tag trước nếu có
                article.Tags.Clear();
                context.NewsArticles.Remove(article);
                context.SaveChanges();
            }
        }
    }
}