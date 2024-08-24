using System.ComponentModel.DataAnnotations;

namespace ecommerce.ViewModels.AccountsViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Informe o E-mail.")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Insira sua senha.")]
        public string Password { get; set; } = string.Empty;
    }
}
