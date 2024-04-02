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
        [MinLength(5, ErrorMessage = "La longitud es de minio 5")]
        public string Estado { get; set; }
        public int IdRecepcionista { get; set; }
        [ForeignKey(nameof(IdRecepcionista))]
        public Recepcionista Recepcionista { get; set; }
    }
}
