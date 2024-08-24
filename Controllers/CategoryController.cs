using ecommerce.Data;
using ecommerce.Extensions;
using ecommerce.Models;
using ecommerce.ViewModels;
using ecommerce.ViewModels.CategoriesViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace ecommerce.Controllers
{
    [ApiController]
    [Route("v1")]
    [Authorize(Roles = "user")]
    public class CategoryController : ControllerBase
    {
        [HttpGet("categories")]
        public IActionResult GetAllAsync(
            IMemoryCache cache,
            AppDbContext context)
        {
            try
            {
                var categories = cache.GetOrCreate("CategoriesCache", entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
                    return GetCategories(context);
                });

                return Ok(new ResultViewModel<List<Category>>(categories));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Category>("Falha interna no servidor"));
            }
        }
        private List<Category> GetCategories(AppDbContext context)
        {
            return context.Categories.ToList();
        }


        [HttpGet("categories/{id:int}")]
        public async Task<IActionResult> GetByIdAsync(
            int id,
            AppDbContext context)
        {
            try
            {
                var category = await context
                    .Categories
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (category == null)
                    return NotFound(new ResultViewModel<Category>("Conteúdo não encontrado"));

                return Ok(new
                {
                    category.Id,
                    category.Title
                });
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Category>("Falha interna no servidor"));
            }
        }


        [HttpPost("categories")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> PostAsync(
            EditorViewModel model,
            AppDbContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));

            try
            {
                var category = new Category
                {
                    Title = model.Title,
                };

                await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();

                return Created($"categories/{category.Id}", new
                {
                    category.Id,
                    category.Title
                });
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Category>("Não foi possível criar a categoria"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Category>("Falha interna no servidor"));
            }
        }


        [HttpPut("categories/{id:int}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateAsync(
            int id,
            EditorViewModel model,
            AppDbContext context)
        {
            try
            {
                var category = await context
                    .Categories
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (category == null)
                    return NotFound(new ResultViewModel<Category>("Conteúdo não encontrado"));

                category.Title = model.Title;

                context.Categories.Update(category);
                await context.SaveChangesAsync();

                return Ok(new
                {
                    category.Id,
                    category.Title
                });
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Category>("Não foi possível atualizar a categoria"));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<Category>("Falha interna no servidor"));
            }
        }


        [HttpDelete("categories/{id:int}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteAsynx(
            int id,
            AppDbContext context)
        {
            try
            {
                var category = await context
                    .Categories
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (category == null)
                    return NotFound(new ResultViewModel<Category>("Conteúdo não encontrado"));

                context.Categories.Remove(category);
                await context.SaveChangesAsync();

                return Ok($"Categoria excluida com sucesso: " +
                    $"Id: {category.Id}, Title: {category.Title}");
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Category>("Não foi possível excluir a categoria"));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<Category>("Falha interna no servidor"));
            }
        }
    }
}
