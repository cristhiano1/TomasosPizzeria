using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TomasosPizzeria.Data.Entities;
using TomasosPizzeria.Data.Interfaces;

using System.Linq;
using TomasosPizzeria.Data.DTOs.CategoryDTOs;

namespace TomasosPizzeria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _repo;

        public CategoryController(ICategoryRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _repo.GetAllCategoriesAsync();

            // Mapear a DTO para no exponer entidades completas
            var categoriesDto = categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name
            });

            return Ok(categoriesDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _repo.GetCategoryByIdAsync(id);
            if (category == null) return NotFound();

            var categoryDto = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };

            return Ok(categoryDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = new Category
            {
                Name = dto.Name
            };

            await _repo.AddCategoryAsync(category);
            await _repo.SaveChangesAsync();

            var categoryDto = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };

            return CreatedAtAction(nameof(GetById), new { id = category.Id }, categoryDto);
        }

        //[HttpPut("{id}")]
        //public async Task<IActionResult> Update(int id, [FromBody] CreateCategoryDto dto)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    var existing = await _repo.GetCategoryByIdAsync(id);
        //    if (existing == null) return NotFound();

        //    existing.Name = dto.Name;

        //    _repo.UpdateCategory(existing);
        //    await _repo.SaveChangesAsync();

        //    return NoContent();
        //}

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _repo.GetCategoryByIdAsync(id);
            if (existing == null) return NotFound();

            await _repo.DeleteCategoryAsync(id);
            await _repo.SaveChangesAsync();

            return NoContent();
        }
    }
}

