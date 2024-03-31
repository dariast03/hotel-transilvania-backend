using System.ComponentModel.DataAnnotations;

namespace HotelTransilvania.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Error, se requiere Correo.")]
        public string Correo { get; set; }
        [Required(ErrorMessage = "Error, se requiere Contraseña.")]
        public string Contraseña { get; set; }
        [Required(ErrorMessage = "Error, se requiere Rol.")]
        public string Rol { get; set; }
        [Required(ErrorMessage = "Error, se requiere Estado.")]
        public string Estado { get; set; }
    }
}
