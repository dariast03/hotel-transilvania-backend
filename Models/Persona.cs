using System.ComponentModel.DataAnnotations;

namespace HotelTransilvania.Models
{
    public class Persona
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Error, se requiere Nombre.")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Error, se requiere Apellido.")]
        public string Apellido { get; set; }
        [Required(ErrorMessage = "Error, se requiere NroDocumento.")]
        public string NroDocumento { get; set; }
        [Required(ErrorMessage = "Error, se requiere TipoDocumento.")]
        public string TipoDocumento { get; set; }
        [Required(ErrorMessage = "Error, se requiere Telefono.")]
        public string Telefono { get; set; }
        [Required(ErrorMessage = "Error, se requiere Direccion.")]
        public string Direccion { get; set; }

    }
}
