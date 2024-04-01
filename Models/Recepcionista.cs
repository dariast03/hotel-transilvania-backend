using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelTransilvania.Models
{
    public class Recepcionista
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Error, se requiere Cargo.")]
        [MinLength(2, ErrorMessage = "El cargo debe tener al menos 2 caracteres.")]
        public string Cargo { get; set; } = null!;

        public int IdPersona { get; set; }

        [ForeignKey(nameof(IdPersona))]
        public Persona Persona { get; set; } = null!;

        public int IdUsuario { get; set; }

        [ForeignKey(nameof(IdUsuario))]
        public Usuario Usuario { get; set; } = null!;
    }
}
