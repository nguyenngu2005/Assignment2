using BusinessObjects;
using System.Collections.Generic;

namespace Repositories
{
    public interface INewsArticleRepo
    {
        List<NewsArticle> GetNewsArticles();
        NewsArticle? GetNewsArticleById(string id);
        void AddNewsArticle(NewsArticle newsArticle);
        void AddNewsArticle(NewsArticle newsArticle, List<int> tagIds);
        void UpdateNewsArticle(NewsArticle newsArticle);
        void UpdateNewsArticle(NewsArticle newsArticle, List<int> tagIds);
        void DeleteNewsArticle(string id);
    }
}