using API.Classes;
using API.Entities;

namespace API.Interfaces
{
    public interface IMenuRepository
    {
        Task<List<MenuItem>> GetAllMenuItems();
        Task<MenuItem> AddMenuItem(MenuItem menuItem);
        Task<bool> DeleteMenuItem(MenuItem menuItem);
        Task<MenuItem> UpdateMenuItem(MenuItem menuItem);
    }
}
