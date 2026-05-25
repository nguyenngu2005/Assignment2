using BusinessObjects;
using DataAccessObjects;
using System.Collections.Generic;

namespace Repositories
{
    public class NewsArticleRepo : INewsArticleRepo
    {
        public List<NewsArticle> GetNewsArticles() => NewsArticleDAO.Instance.GetNewsArticles();
        public NewsArticle? GetNewsArticleById(string id) => NewsArticleDAO.Instance.GetNewsArticleById(id);
        public void AddNewsArticle(NewsArticle newsArticle) => NewsArticleDAO.Instance.AddNewsArticle(newsArticle);
        public void AddNewsArticle(NewsArticle newsArticle, List<int> tagIds) => NewsArticleDAO.Instance.AddNewsArticle(newsArticle, tagIds);
        public void UpdateNewsArticle(NewsArticle newsArticle) => NewsArticleDAO.Instance.UpdateNewsArticle(newsArticle);
        public void UpdateNewsArticle(NewsArticle newsArticle, List<int> tagIds) => NewsArticleDAO.Instance.UpdateNewsArticle(newsArticle, tagIds);
        public void DeleteNewsArticle(string id) => NewsArticleDAO.Instance.DeleteNewsArticle(id);
    }
}