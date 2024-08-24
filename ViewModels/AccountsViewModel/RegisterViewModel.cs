using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ecommerce.ViewModels.AccountsViewModel
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Campo 'Nome' é obrigatório.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Campo 'E-mail' é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Campo 'Password' é obrigatório.")]
        [MaxLength(36, ErrorMessage = "Capacidade maxima de 36 caracteres.")]
        [MinLength(6, ErrorMessage = "Minimo obrigatório de 6 caracteres.")]
        public string Password { get; set; } = string.Empty;
        [JsonIgnore]
        public int RoleId { get; set; }
    }
}
