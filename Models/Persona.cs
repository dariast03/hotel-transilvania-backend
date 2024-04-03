using System.ComponentModel.DataAnnotations;

namespace HotelTransilvania.Models
{
    public class Persona
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Error, se requiere Nombre.")]
       // [MinLength(2, ErrorMessage = "El nombre debe tener al menos 2 caracteres.")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "Error, se requiere Apellido.")]
       // [MinLength(2, ErrorMessage = "El apellido debe tener al menos 2 caracteres.")]
        public string Apellido { get; set; } = null!;

        [Required(ErrorMessage = "Error, se requiere NroDocumento.")]
       // [MinLength(6, ErrorMessage = "El número de documento debe tener al menos 6 caracteres.")]
        public string NroDocumento { get; set; } = null!;

        [Required(ErrorMessage = "Error, se requiere TipoDocumento.")]
       // [MinLength(2, ErrorMessage = "El tipo de documento debe tener al menos 2 caracteres.")]
        public string TipoDocumento { get; set; } = null!;

        [Required(ErrorMessage = "Error, se requiere Telefono.")]
       // [MinLength(8, ErrorMessage = "El número de teléfono debe tener al menos 8 caracteres.")]
        public string Telefono { get; set; } = null!;

        [Required(ErrorMessage = "Error, se requiere Direccion.")]
       // [MinLength(5, ErrorMessage = "La dirección debe tener al menos 5 caracteres.")]
        public string Direccion { get; set; } = null!;
    }
}
