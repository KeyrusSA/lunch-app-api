using API.Entities;

namespace API.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> GetOrderByUser(string user);
        Task<Order> AddOrder(Order order);
        Task<bool> DeleteOrder(string user, DateTime date);
    }
}
