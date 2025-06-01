using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TomasosPizzeria.Data.DTOs.IngredientDTOs;
using TomasosPizzeria.Data.Entities;
using TomasosPizzeria.Data.Interfaces;

//namespace TomasosPizzeria.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class IngredientController : ControllerBase
//    {
//        private readonly IIngredientRepository _repo;

//        public IngredientController(IIngredientRepository repo)
//        {
//            _repo = repo;
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetAll()
//        {
//            var ingredients = await _repo.GetAllIngredientsAsync();

//            var ingredientsDto = ingredients.Select(i => new IngredientDto
//            {
//                Id = i.Id,
//                Name = i.Name
//            });

//            return Ok(ingredientsDto);
//        }

//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetById(int id)
//        {
//            var ingredient = await _repo.GetIngredientByIdAsync(id);
//            if (ingredient == null) return NotFound();

//            var ingredientDto = new IngredientDto
//            {
//                Id = ingredient.Id,
//                Name = ingredient.Name
//            };

//            return Ok(ingredientDto);
//        }

//        [HttpPost]
//        public async Task<IActionResult> Create([FromBody] CreateIngredientDto dto)
//        {
//            if (!ModelState.IsValid)
//                return BadRequest(ModelState);

//            var ingredient = new Ingredient
//            {
//                Name = dto.Name
//            };

//            await _repo.AddIngredientAsync(ingredient);
//            await _repo.SaveChangesAsync();

//            var ingredientDto = new IngredientDto
//            {
//                Id = ingredient.Id,
//                Name = ingredient.Name
//            };

//            return CreatedAtAction(nameof(GetById), new { id = ingredient.Id }, ingredientDto);
//        }

//        [HttpPut("{id}")]
//        public async Task<IActionResult> Update(int id, [FromBody] CreateIngredientDto dto)
//        {
//            if (!ModelState.IsValid)
//                return BadRequest(ModelState);

//            var existing = await _repo.GetIngredientByIdAsync(id);
//            if (existing == null) return NotFound();

//            existing.Name = dto.Name;

//            _repo.UpdateIngredient(existing);
//            await _repo.SaveChangesAsync();
//            return NoContent();
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> Delete(int id)
//        {
//            var existing = await _repo.GetIngredientByIdAsync(id);
//            if (existing == null) return NotFound();

//            await _repo.DeleteIngredientAsync(id);
//            await _repo.SaveChangesAsync();
//            return NoContent();
//        }
//    }
//}
