using System.ComponentModel.DataAnnotations;

namespace APItestp2._1.Models
{
    // Persona y sus atributos con validaciones 
    public class Persona
    {
        public int PersonaId { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, ErrorMessage = "El campo nombre debe tener maximo (1) 50 caracteres.")]
        public string? Nombre { get; set; }

        public int Dni {  get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        [DataType(DataType.Date)]
        public DateOnly FechaNacimiento { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "El correo debe ser valido")]
        [StringLength(100, ErrorMessage = "El campo nombre debe tener maximo (1) 100 caracteres.")]
        public string? Correo { get; set; }



        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "El campo contraseña debe tener maximo (1) 100 caracteres.")]
        public string Contrasenia { get; set; }

   
      
        public string? Token { get; set; }



        public DateTime? FechaExpiracion { get; set; }
        public bool Estado { get; set; }
    }
}
