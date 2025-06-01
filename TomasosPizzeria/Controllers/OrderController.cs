using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TomasosPizzeria.Data.DTOs.OrderDTOs;
using TomasosPizzeria.Data.Entities;
using TomasosPizzeria.Data.Interfaces;

namespace TomasosPizzeria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IDishRepository _dishRepo;

        public OrderController(IOrderRepository orderRepo, IDishRepository dishRepo)
        {
            _orderRepo = orderRepo;
            _dishRepo = dishRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _orderRepo.GetAllOrdersAsync();

            var ordersDto = new List<OrderDto>();
            foreach (var order in orders)
            {
                var dishesDto = new List<DishDto>();

                // Cambié order.OrderItems a order.Items
                if (order.Items != null)
                {
                    foreach (var item in order.Items)
                    {
                        var dish = item.Dish;
                        if (dish != null)
                        {
                            dishesDto.Add(new DishDto
                            {
                                Id = dish.Id,
                                Name = dish.Name,
                                Description = dish.Description,
                                Price = dish.Price,
                                CategoryName = dish.Category?.Name ?? "",
                                Ingredients = dish.Ingredients?.Select(i => i.Name).ToList() ?? new List<string>()
                            });
                        }
                    }
                }

                ordersDto.Add(new OrderDto
                {
                    Id = order.Id,
                    OrderDate = order.CreatedAt,
                    TotalPrice = order.TotalPrice,
                    Dishes = dishesDto
                });
            }

            return Ok(ordersDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _orderRepo.GetOrderByIdAsync(id);
            if (order == null) return NotFound();

            var dishesDto = new List<DishDto>();

            // Cambié order.OrderItem a order.Items
            if (order.Items != null)
            {
                foreach (var item in order.Items)
                {
                    var dish = item.Dish;
                    if (dish != null)
                    {
                        dishesDto.Add(new DishDto
                        {
                            Id = dish.Id,
                            Name = dish.Name,
                            Description = dish.Description,
                            Price = dish.Price,
                            CategoryName = dish.Category?.Name ?? "",
                            Ingredients = dish.Ingredients?.Select(i => i.Name).ToList() ?? new List<string>()
                        });
                    }
                }
            }

            var orderDto = new OrderDto
            {
                Id = order.Id,
                OrderDate = order.CreatedAt,
                TotalPrice = order.TotalPrice,
                Dishes = dishesDto
            };

            return Ok(orderDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(List<int> DishIds)
        {
              
        decimal totalPrice = 0;
            var items = new List<OrderItem>();


            foreach (var dishId in DishIds)
            {
                var dish = await _dishRepo.GetDishByIdAsync(dishId);
                if (dish == null)
                {
                    return BadRequest($"Dish with ID {dishId} does not exist.");
                }

                totalPrice += dish.Price; // Sumar el precio del plato al total

                items.Add(new OrderItem  // Cambié OrderItems a Items           
                {       
                    DishId = dish.Id,
                    //Dish = dish
                });
            }

            var order = new Order
            {
                CreatedAt = DateTime.UtcNow,
                TotalPrice = totalPrice,
                UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value),
                Items = items
            };

            await _orderRepo.AddOrderAsync(order);
            await _orderRepo.SaveChangesAsync();

            var dishesDto = order.Items.Select(oi => new DishDto  // Cambié OrderItems a Items
            {
                Id = oi.Dish.Id,
                Name = oi.Dish.Name,
                Description = oi.Dish.Description,
                Price = oi.Dish.Price,
                CategoryName = oi.Dish.Category?.Name ?? "",
                Ingredients = oi.Dish.Ingredients?.Select(i => i.Name).ToList() ?? new List<string>()

            }).ToList();

            var orderDto = new OrderDto
            {
                Id = order.Id,
                OrderDate = order.CreatedAt,
                TotalPrice = order.TotalPrice,
                Dishes = dishesDto
            };

            return CreatedAtAction(nameof(GetById), new { id = order.Id }, orderDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, OrderUpdateDto dto)
        {
            var existing = await _orderRepo.GetOrderByIdAsync(id);
            if (existing == null) return NotFound();

            // Limpiar los ítems existentes
            existing.Items.Clear();

            foreach (var dishId in dto.DishIds)
            {
                var dish = await _dishRepo.GetDishByIdAsync(dishId);
                if (dish == null)
                {
                    return BadRequest($"Dish with ID {dishId} does not exist.");
                }

                existing.Items.Add(new OrderItem
                {
                    DishId = dish.Id,
                    Order = existing,
                    Dish = dish
                });
            }

            _orderRepo.UpdateOrder(existing); // Llama al método sincrónico de tu repo
            await _orderRepo.SaveChangesAsync(); // Guarda cambios en la DB

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _orderRepo.GetOrderByIdAsync(id);
            if (existing == null) return NotFound();

            await _orderRepo.DeleteOrderAsync(id);
            await _orderRepo.SaveChangesAsync();
            return NoContent();
        }
    }

}
