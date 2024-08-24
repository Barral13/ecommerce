using ecommerce.Data;
using ecommerce.Models;
using ecommerce.ViewModels.ProductsViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ecommerce.Controllers
{
    [ApiController]
    [Route("v1")]
    [Authorize(Roles = "user")]
    public class ProductController : ControllerBase
    {
        [HttpGet("products")]
        public async Task<IActionResult> GetAllAsync(
            AppDbContext context,
            int page = 0,
            int pageSize = 25)
        {
            try
            {
                var count = await context
                    .Products.AsNoTracking()
                    .CountAsync();

                var products = await context
                    .Products
                    .AsNoTracking()
                    .Include(x => x.Category)
                    .Select(x => new ListProductsViewModel
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Slug = x.Slug,
                        UpdatedAt = x.UpdatedAt,
                        Category = x.Category.Title
                    })
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .OrderByDescending(x => x.UpdatedAt)
                    .ToListAsync();
                return Ok(new
                {
                    total = count,
                    page,
                    pageSize,
                    products
                });
            }
            catch
            {
                return StatusCode(500, "Falha interna no servidor");
            }
        }


        [HttpGet("products/{id:int}")]
        public async Task<IActionResult> DetailAsync(
            AppDbContext context,
            int id)
        {
            try
            {
                var product = await context
                    .Products
                    .AsNoTracking()
                    .Include(x => x.Category)
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (product == null)
                    return NotFound("Conteúdo não encontrado");

                return Ok(product);
            }
            catch
            {
                return StatusCode(500, "Falha interna no servidor");
            }
        }


        [HttpGet("products/category/{category}")]
        public async Task<IActionResult> GetByCategoryAsync(
            string category,
            AppDbContext context,
            int page = 0,
            int pageSize = 25)
        {
            try
            {
                var count = await context.Products.AsNoTracking().CountAsync();
                var products = await context
                    .Products
                    .AsNoTracking()
                    .Include(x => x.Category)
                    .Where(x => x.Category.Title == category)
                    .Select(x => new ListProductsViewModel
                    { 
                        Id = x.Id,
                        Title = x.Title,
                        Slug = x.Slug,
                        UpdatedAt = x.UpdatedAt
                    })
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .OrderByDescending(x => x.UpdatedAt)
                    .ToListAsync();
                return Ok(new
                {
                    total = count,
                    page,
                    pageSize,
                    products
                });
            }
            catch
            {
                return StatusCode(500, "Falha interna no servidor");
            }
        }


        [HttpPost("products")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> PostAsync(
            AppDbContext context,
            EditorProductViewModel model)
        {
            if(!ModelState.IsValid) 
                return BadRequest(ModelState);

            try
            {

                var category = await context
                    .Categories
                    .FirstOrDefaultAsync(x => x.Id == model.CategoryId);


                if (category == null)
                    return NotFound("Categoria não encontrada");

                var existingProduct = await context.Products
                    .FirstOrDefaultAsync(x => x.Slug
                    .ToLower() == model.Slug
                    .ToLower());


                if (existingProduct != null)
                    return Conflict("Um produto com o mesmo slug já existe");

                var product = new Product
                {
                    Title = model.Title,
                    Description = model.Description,
                    Price = model.Price,
                    Slug = model.Slug.ToLower(),
                    Category = category
                };

                await context.Products.AddAsync(product);
                await context.SaveChangesAsync();

                var response = new EditorProductViewModel
                {
                    Id = product.Id,
                    Title = model.Title,
                    Price = model.Price,
                    Slug = model.Slug.ToLower(),
                    CategoryId = category.Id,
                };

                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro ao criar o produto");
            }
        }


        [HttpPut("products/{id:int}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateAsync(
            AppDbContext context,
            EditorProductViewModel model,
            int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var product = await context
                    .Products
                    .Include(x => x.Category)
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (product == null)
                    return NotFound("Produto não encontrado");

                product.Title = model.Title;
                product.Description = model.Description;
                product.Price = model.Price;
                product.Slug = model.Slug;

                if(model.CategoryId > 0)
                {
                    var category = await context.Categories
                        .FirstOrDefaultAsync(x => x.Id == model.CategoryId);

                    if (category == null)
                        return NotFound("Categoria não encontrada");

                    product.Category = category;
                }

                context.Update(product);
                await context.SaveChangesAsync();

                var response = new EditorProductViewModel
                {
                    Id = product.Id,
                    Title = model.Title,
                    Price = model.Price,
                    Slug = model.Slug.ToLower(),
                    CategoryId = product.CategoryId,
                };

                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(500,"Erro interno no servidor");
            }
        }

        [HttpDelete("products/{id:int}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteAsync(
            AppDbContext context,
            int id)
        {
            try
            {
                var product = await context
                    .Products
                    .FirstOrDefaultAsync (x => x.Id == id);

                if (product == null) 
                    return NotFound("Produto não encontrado");

                context.Products.Remove(product);
                await context.SaveChangesAsync();

                return Ok($"Produto excluido com sucesso: " +
                    $"Id: {product.Id}, Title: {product.Title}");
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Não foi possível excluir a categoria");
            }
            catch (Exception)
            {
                return StatusCode(500, "Falha interna no servidor");
            }
        }

    }
}
