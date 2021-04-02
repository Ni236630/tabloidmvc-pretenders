using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface ICategoryRepository
    {
        List<Category> GetAll();
        void AddCategory(Category category);

        void DeleteCategory(int categoryId);

        Category GetCategoryById(int id);

        void UpdateCategory(Category category);
    }
}