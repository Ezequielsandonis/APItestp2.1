using System.ComponentModel.DataAnnotations;

namespace APItestp2._1.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "El correo debe ser valido")]
        public string? Correo { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        public string? Contrasenia { get; set; }

        [Display(Name = "Mantener la sesion activa")]
        public bool MantenerActivo { get; set; }
    }
}
