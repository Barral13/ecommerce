using ecommerce.Data;
using ecommerce.Models;
using ecommerce.Services;
using ecommerce.ViewModels;
using ecommerce.ViewModels.AccountsViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;

namespace ecommerce.Controllers
{
    [ApiController]
    [Route("v1")]
    public class AccountController : ControllerBase
    {
        [HttpPost("accounts")]
        public async Task<IActionResult> Post(
            AppDbContext context,
            RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new User
            {
                Username = model.Name,
                Email = model.Email,
                PasswordHash = PasswordHasher.Hash(model.Password)
            };

            try
            {
                var userRole = await context
                    .Roles
                    .FirstOrDefaultAsync(x => x.Name == "User");

                if (userRole == null)
                    return NotFound(new ResultViewModel<Category>("Conteúdo não encontrado"));

                context.Attach(userRole);
                user.Roles.Add(userRole);

                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();

                return Ok(new
                {
                    user.Id,
                    user.Username,
                    user.Email,
                    Roles = user.Roles.Select(x => x.Name)
                });
            }
            catch (DbUpdateException)
            {
                return StatusCode(400, "Email já cadastrado.");
            }
            catch (Exception)
            {
                return BadRequest("Erro interno do servidor.");
            }
        }


        [HttpPost("accounts/login")]
        public async Task<IActionResult> Login(
            AppDbContext context,
            LoginViewModel model,
            TokenService tokenService)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await context
                .Users
                .AsNoTracking()
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Email == model.Email);

            if (user == null)
                return StatusCode(401, "Usuário ou senha inválidos.");

            if (!PasswordHasher.Verify(user.PasswordHash, model.Password))
                return StatusCode(401, "Usuário ou senha inválidos.");

            try
            {
                var token = tokenService.GenerateToken(user);
                return Ok(token);
            }
            catch (Exception)
            {
                return StatusCode(500, "Falha interna do servidor.");
            }
        }
    }
}
