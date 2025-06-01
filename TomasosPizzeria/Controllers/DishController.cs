using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TomasosPizzeria.Data.Entities;
using TomasosPizzeria.Data.Interfaces;
using System.Linq;
using TomasosPizzeria.Data.DTOs.DishDTOs;

namespace TomasosPizzeria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DishController : ControllerBase
    {
        private readonly IDishRepository _repo;

        public DishController(IDishRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var dishes = await _repo.GetAllDishesAsync();

            var dishesDto = dishes.Select(d => new DishDto
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description,
                Price = d.Price,
                CategoryId = d.CategoryId,
                CategoryName = d.Category?.Name // si tienes la propiedad Category cargada
            });

            return Ok(dishesDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var dish = await _repo.GetDishByIdAsync(id);
            if (dish == null) return NotFound();

            var dishDto = new DishDto
            {
                Id = dish.Id,
                Name = dish.Name,
                Description = dish.Description,
                Price = dish.Price,
                CategoryId = dish.CategoryId,
                CategoryName = dish.Category?.Name
            };

            return Ok(dishDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDishDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dish = new Dish
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                CategoryId = dto.CategoryId
            };

            await _repo.AddDishAsync(dish);
            await _repo.SaveChangesAsync();

            var dishDto = new DishDto
            {
                Id = dish.Id,
                Name = dish.Name,
                Description = dish.Description,
                Price = dish.Price,
                CategoryId = dish.CategoryId,
                CategoryName = null // opcional, puedes cargar la categoría si quieres
            };

            return CreatedAtAction(nameof(GetById), new { id = dish.Id }, dishDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateDishDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = await _repo.GetDishByIdAsync(id);
            if (existing == null) return NotFound();

            existing.Name = dto.Name;
            existing.Description = dto.Description;
            existing.Price = dto.Price;
            existing.CategoryId = dto.CategoryId;
            // Las relaciones complejas como Ingredients no se actualizan directamente aquí por simplicidad

            _repo.UpdateDish(existing);
            await _repo.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _repo.GetDishByIdAsync(id);
            if (existing == null) return NotFound();

            await _repo.DeleteDishAsync(id);
            await _repo.SaveChangesAsync();

            return NoContent();
        }
    }
}
