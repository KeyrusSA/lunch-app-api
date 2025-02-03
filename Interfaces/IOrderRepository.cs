using API.Entities;

namespace API.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> GetOrderByUser(string user);
        Task<Order> AddOrder(Order order);
        Task<Order> UpdateOrder(Order order);
        Task<bool> DeleteOrder(Order order);
    }
}
