using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HotelTransilvania.Models
{
    public class Promocion
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Error, se requiere Descuento.")]
        public float Descuento { get; set; }
        public int IdCliente { get; set; }
        [ForeignKey(nameof(IdCliente))]
        public Cliente Cliente { get; set; }

        public bool AplicaDescuento()
        {
            if (Cliente.Frecuencia == "Frecuente")
            {
                return true;
            }
            return false;
        }
    }
}
