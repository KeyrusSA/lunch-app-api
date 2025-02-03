using API.Classes;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;

namespace API.Data
{
    public class MenuRepository: IMenuRepository
    {
        protected DataContext _dataContext;
        protected DbSet<MenuItem> dbSet;


        public MenuRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
            dbSet = _dataContext.Set<MenuItem>();
        }

        public async Task<List<MenuItem>> GetAllMenuItems()
        {
            List<MenuItem> menuItems = await dbSet.OrderByDescending(x => x.Date).ToListAsync();
            return menuItems;
        }

        public async Task<MenuItem> UpdateMenuItem(MenuItem menuItem)
        {
            try
            {
                dbSet.Update(menuItem);
                await _dataContext.SaveChangesAsync();
                return menuItem;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return menuItem;
            }
        }

        public async Task<MenuItem> AddMenuItem(MenuItem menuItem)
        {
            await dbSet.AddAsync(menuItem);
            _dataContext.SaveChanges();
            return menuItem;
        }

        public async Task<bool> DeleteMenuItem(int id)
        {
            try
            {
                var item = await dbSet.FindAsync(id);
                dbSet.Remove(item);
                await _dataContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<MenuItem>> GetAllMainMenuItemsByDate(DateTime date)
        {
            var result = await dbSet
                .Where(x => (x.Date == date.ToString("yyyy-MM-dd") || x.Date == "Any") && x.IsMainMeal == true)
                .ToListAsync();

            return result.Count > 0 ? result : null;
        }

        public async Task<List<MenuItem>> GetAllSideMenuItemsByDate(DateTime date) 
        {
            var result = await dbSet
                .Where(x => (x.Date == date.ToString("yyyy-MM-dd") || x.Date == "Any") && x.IsMainMeal == true)
                .ToListAsync();

            return result.Count > 0 ? result : null;
        }

        public Task<MenuItem> GetMenuItemByUsername(int id)
        {
            throw new NotImplementedException();
        }
    }
}
