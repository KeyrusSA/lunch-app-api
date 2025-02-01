using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class OrderRepository: IOrderRepository
    {
        protected DataContext _dataContext;
        protected DbSet<Order> dbSet;


        public OrderRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
            dbSet = _dataContext.Set<Order>();
        }

        public async Task<Order> GetOrderByUser(string user)
        {
            Order order = await dbSet.Where(x => x.User.ToUpper() == user.ToUpper()).FirstOrDefaultAsync();
            return order;
        }

        public async Task<Order> AddOrder(Order order)
        {
            await dbSet.AddAsync(order);
            _dataContext.SaveChanges();
            return order;
        }

        public async Task<bool> DeleteOrder(string user, DateTime date)
        {
            try
            {
                var item = await dbSet.Where(x => x.User.ToUpper() == user && x.Date == date).FirstOrDefaultAsync();
                dbSet.Remove(item);
                await _dataContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
