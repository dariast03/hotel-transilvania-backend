using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelTransilvania.Models
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Error, se requiere Nacionalidad.")]
        [MinLength(2, ErrorMessage = "La longitud mínima de la Nacionalidad debe ser de dos caracteres.")]
        public string Nacionalidad { get; set; } = null!;

        public string? Frecuencia { get; set; }

        public int IdPersona { get; set; }

        [ForeignKey(nameof(IdPersona))]
        public Persona Persona { get; set; } = null!;

        public int IdUsuario { get; set; }

        [ForeignKey(nameof(IdUsuario))]
        public Usuario Usuario { get; set; } = null!;
    }
}

