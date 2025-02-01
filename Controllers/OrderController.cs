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
        public async Task<string> AddOrder([FromBody] Order order)
        {
            try
            {
                await _orderRepository.AddOrder(order);
                return "Order Added Successfully!";
            }
            catch (Exception ex)
            {
                return $"An error occurred while trying to add order";
            }
        }

        [HttpDelete("DeleteOrder/{date}")]
        public async Task<string> DeleteMenuItem([FromRoute] string date, [FromBody] string user)
        {
            try
            {

                await _orderRepository.DeleteOrder(user, DateTime.Parse(date));
                return "Item Deleted Successfully!";
            }
            catch (Exception ex)
            {
                return $"An error occurred while trying to delete item";
            }
        }
    }
}