using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelTransilvania.Models
{
    public class Reserva
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaFin { get; set; }
        [Required]
        public string Estado { get; set; }
        public int IdCliente { get; set; }
        [ForeignKey(nameof(IdCliente))]
        public Cliente Cliente { get; set; }
        public int IdHabitacion { get; set; }
        [ForeignKey(nameof(IdHabitacion))]
        public Habitacion Habitacion { get; set; }
    }
}
