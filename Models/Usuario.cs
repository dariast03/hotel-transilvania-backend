using System.ComponentModel.DataAnnotations;

namespace HotelTransilvania.Models
{

    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Error, se requiere Correo.")]
        [MinLength(6, ErrorMessage = "El correo electrónico debe tener al menos 6 caracteres.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
        public string Correo { get; set; } = null!;

        [Required(ErrorMessage = "Error, se requiere Contraseña.")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        [DataType(DataType.Password)]
        public string Contraseña { get; set; } = null!;

        [Required(ErrorMessage = "Error, se requiere Rol.")]
        public string Rol { get; set; } = null!;

        [Required(ErrorMessage = "Error, se requiere Estado.")]
        [MinLength(6, ErrorMessage = "El estado debe tener al menos 6 caracteres.")]
        public string Estado { get; set; } = "ACTIVO";
    }
}
