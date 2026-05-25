using BusinessObjects;
using DataAccessObjects;
using System.Collections.Generic;

namespace Repositories
{
    public class CategoryRepo : ICategoryRepo
    {
        public List<Category> GetCategories() => CategoryDAO.Instance.GetCategories();
        public Category? GetCategoryById(short id) => CategoryDAO.Instance.GetCategoryById(id);
        public void AddCategory(Category category) => CategoryDAO.Instance.AddCategory(category);
        public void UpdateCategory(Category category) => CategoryDAO.Instance.UpdateCategory(category);
        public void DeleteCategory(short id) => CategoryDAO.Instance.DeleteCategory(id);
    }
}