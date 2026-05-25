using BusinessObjects;
using System.Collections.Generic;

namespace Repositories
{
    public interface ICategoryRepo
    {
        List<Category> GetCategories();
        Category? GetCategoryById(short id);
        void AddCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(short id);
    }
}