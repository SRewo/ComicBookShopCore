using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComicBookShopCore.Services.Order;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ComicBookShopCore.WebAPI.Controllers
{
    [Route("api/order")]
    public class OrderController : ODataController
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [EnableQuery]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Employee")]
        public async Task<ActionResult<IEnumerable<OrderBasicDto>>> GetOrders()
        {
            var result = await _orderService.OrderListAsync();
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize]
        [EnableQuery]
        public async Task<ActionResult<OrderDetailsDto>> GetOrderDetails(int id)
        {
            var result = await _orderService.OrderDetailsAsync(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> PutOrder([FromBody] OrderInputDto order)
        {
            var result = await _orderService.AddOrderAsync(order);

            if (!result.Any())
                return Ok();

            var errors = new ModelStateDictionary();
            foreach (var error in result) errors.AddModelError(error.Key, error.Value);
            return ValidationProblem(errors);
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Employee")]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            try
            {
                await _orderService.RemoveOrderAsync(id);
                return NoContent();
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
        }
    }
}