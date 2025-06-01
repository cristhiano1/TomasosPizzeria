using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TomasosPizzeria.Data.Entities;
using TomasosPizzeria.Data.Interfaces;

//namespace TomasosPizzeria.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class OrderItemController : ControllerBase
//    {
//        private readonly IOrderItemRepository _repo;

//        public OrderItemController(IOrderItemRepository repo)
//        {
//            _repo = repo;
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetAll()
//        {
//            var items = await _repo.GetAllOrderItemsAsync();
//            return Ok(items);
//        }

//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetById(int id)
//        {
//            var item = await _repo.GetOrderItemByIdAsync(id);
//            if (item == null) return NotFound();
//            return Ok(item);
//        }

//        [HttpPost]
//        public async Task<IActionResult> Create(OrderItem orderItem)
//        {
//            await _repo.AddOrderItemAsync(orderItem);
//            await _repo.SaveChangesAsync();
//            return CreatedAtAction(nameof(GetById), new { id = orderItem.Id }, orderItem);
//        }

//        [HttpPut("{id}")]
//        public async Task<IActionResult> Update(int id, OrderItem updated)
//        {
//            var existing = await _repo.GetOrderItemByIdAsync(id);
//            if (existing == null) return NotFound();

//            existing.DishId = updated.DishId;
//            existing.Quantity = updated.Quantity;
//            existing.OrderId = updated.OrderId;

//            _repo.UpdateOrderItem(existing);
//            await _repo.SaveChangesAsync();
//            return NoContent();
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> Delete(int id)
//        {
//            await _repo.DeleteOrderItemAsync(id);
//            await _repo.SaveChangesAsync();
//            return NoContent();
//        }
//    }
//}
