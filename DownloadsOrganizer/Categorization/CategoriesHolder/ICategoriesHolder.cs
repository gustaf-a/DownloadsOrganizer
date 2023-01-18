using DownloadsOrganizer.Data;

namespace DownloadsOrganizer.Categorization.CategoriesHolder;

public interface ICategoriesHolder
{
    public List<Category> GetCategories();

    public Category GetEmptyCategory();
    public Category GetUnknownCategory();
}
