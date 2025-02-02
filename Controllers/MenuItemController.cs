using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using System.IO;
using API.Entities;
using System.Text.RegularExpressions;
using API.Classes;
using API.Data;
using Microsoft.AspNetCore.Identity;
using API.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Globalization;

namespace API.Controllers
{
    [ApiController]
    [Route("MenuItem")]
    public class MenuItemController : ControllerBase
    {
        private readonly IMenuRepository _menuRepository;

        public MenuItemController(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        [HttpGet("GetAllMainMenuItems")]
        public async Task<List<MenuItem>> GetAllMainMenuItems(DateTime date)
        {
            try
            {
                return await _menuRepository.GetAllMainMenuItemsByDate(date);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("GetAllSideMenuItems")]
        public async Task<List<MenuItem>> GetAllSideMenuItems(DateTime date)
        {
            try
            {
                return await _menuRepository.GetAllSideMenuItemsByDate(date);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost("AddMenuItem")]
        public async Task<string> AddMenuItem([FromBody] MenuItem item)
        {
            try
            {
                await _menuRepository.AddMenuItem(item);
                return "Item Added Successfully!";
            }
            catch (Exception ex)
            {
                return $"An error occurred while trying to add item";
            }
        }


        [HttpPut("UpdateMenuItem")]
        public async Task<string> UpdateMenuItem([FromBody] MenuItem item)
        {
            try
            {
                await _menuRepository.UpdateMenuItem(item);
                return "Item Updated Successfully!";
            }
            catch (Exception ex)
            {
                return $"An error occurred while trying to update item";
            }
        }

        [HttpDelete("DeleteMenuItem/{id}")]
        public async Task<string> DeleteMenuItem([FromRoute] int id)
        {
            try
            {
                await _menuRepository.DeleteMenuItem(id);
                return "Item Deleted Successfully!";
            }
            catch (Exception ex)
            {
                return $"An error occurred while trying to delete item";
            }
        }

        [HttpGet("GetMenuItemById")]
        public async Task<MenuItem> GetMenuItemByUsername([FromRoute] int id)
        {
            try
            {
                return await _menuRepository.GetMenuItemByUsername(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
