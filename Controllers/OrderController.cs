using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("Order")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpGet("GetOrderByUser")]
        public async Task<Order> GetOrderByUser(string user)
        {
            try
            {
                return await _orderRepository.GetOrderByUser(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost("AddOrder")]
        public async Task<string> AddOrder([FromBody] OrderDTO order)
        {
            try
            {
                Order userOrder = new Order()
                {
                    Caterer = order.Caterer,
                    User = order.User,
                    DayOfWeek = order.DayOfTheWeek,
                    Date = order.OrderDate == null ? DateTime.Now.Date : (DateTime)order.OrderDate,
                    Main = order.Main,
                    Side = order.Side
                };
                await _orderRepository.AddOrder(userOrder);
                return "Order Added Successfully!";
            }
            catch (Exception ex)
            {
                return $"An error occurred while trying to add order";
            }
        }
        [HttpPost("UpdateOrder")]
        public async Task<string> UpdateOrder([FromBody] OrderDTO order)
        {
            try
            {
                Order userOrder = new Order()
                {
                    Caterer = order.Caterer,
                    User = order.User,
                    DayOfWeek = order.DayOfTheWeek,
                    Date = order.OrderDate == null ? DateTime.Now.Date : (DateTime)order.OrderDate,
                    Main = order.Main,
                    Side = order.Side
                };
                await _orderRepository.UpdateOrder(userOrder);
                return "Order Updated Successfully!";
            }
            catch (Exception ex)
            {
                return $"An error occurred while trying to add order";
            }
        }

        [HttpDelete("DeleteOrder")]
        public async Task<string> DeleteMenuItem([FromBody] OrderDTO order)
        {
            try
            {
                Order userOrder = new Order()
                {
                    User = order.User,
                    Date = order.OrderDate == null ? DateTime.Now.Date : (DateTime)order.OrderDate,
                };
                await _orderRepository.DeleteOrder(userOrder);
                return "Order Deleted Successfully!";
            }
            catch (Exception ex)
            {
                return $"An error occurred while trying to delete item";
            }
        }
    }
}