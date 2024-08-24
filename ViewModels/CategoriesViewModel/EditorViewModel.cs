using System.ComponentModel.DataAnnotations;

namespace ecommerce.ViewModels.CategoriesViewModel
{
    public class EditorViewModel
    {
        [Required(ErrorMessage = "O Titulo é obrigatório")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "O campo deve conter entre 3 e 40 caracteres")]
        public string Title { get; set; } = string.Empty;
    }
}
