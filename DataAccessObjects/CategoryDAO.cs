using BusinessObjects;
using DataAccessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessObjects
{
    public class CategoryDAO
    {
        private static CategoryDAO? instance = null;
        private static readonly object instanceLock = new object();

        private CategoryDAO() { }

        public static CategoryDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new CategoryDAO();
                    }
                    return instance;
                }
            }
        }

        public List<Category> GetCategories()
        {
            using var context = new FunewsManagementContext();
            return context.Categories.ToList();
        }

        public Category? GetCategoryById(short id)
        {
            using var context = new FunewsManagementContext();
            return context.Categories.SingleOrDefault(c => c.CategoryId == id);
        }

        public void AddCategory(Category category)
        {
            using var context = new FunewsManagementContext();
            context.Categories.Add(category);
            context.SaveChanges();
        }

        public void UpdateCategory(Category category)
        {
            using var context = new FunewsManagementContext();
            context.Categories.Update(category);
            context.SaveChanges();
        }

        public void DeleteCategory(short id)
        {
            using var context = new FunewsManagementContext();

            // Logic bắt buộc: Không cho xóa nếu Category đã có NewsArticle
            bool hasArticles = context.NewsArticles.Any(n => n.CategoryId == id);
            if (hasArticles)
            {
                throw new Exception("Không thể xóa Category này vì đang chứa News Article!");
            }

            var category = context.Categories.SingleOrDefault(c => c.CategoryId == id);
            if (category != null)
            {
                context.Categories.Remove(category);
                context.SaveChanges();
            }
        }
    }
}