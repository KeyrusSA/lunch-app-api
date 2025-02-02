using API.Classes;
using API.Entities;

namespace API.Interfaces
{
    public interface IMenuRepository
    {
        Task<List<MenuItem>> GetAllMainMenuItemsByDate(DateTime date);
        Task<List<MenuItem>> GetAllSideMenuItemsByDate(DateTime date);
        Task<MenuItem> AddMenuItem(MenuItem menuItem);
        Task<bool> DeleteMenuItem(int id);
        Task<MenuItem> UpdateMenuItem(MenuItem menuItem);
        Task<MenuItem> GetMenuItemByUsername(int id);
    }
}
