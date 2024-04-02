using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelTransilvania.Models
{
    public class Habitacion
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Error, se requiere Codigo.")]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "Error, se requiere Nombre.")]
        [StringLength(50, ErrorMessage = "Error, el nombre no puede tener más de 80 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Error, se requiere ubicacion.")]
        public string Ubicacion { get; set; }
        [Required(ErrorMessage = "Error, se requiere Tipo.")]
        public string Tipo { get; set; }
        [Required(ErrorMessage = "Error, se requiere Capacidad.")]
        public int Capacidad { get; set; }
        [Required(ErrorMessage = "Error, se requiere Precio.")]
        public float Precio { get; set; }
        [Required(ErrorMessage = "Error, se requiere Imagen.")]
        public string Imagen { get; set; }
        [Required(ErrorMessage = "Error, se requiere Nro de Cuartos.")]
        public int NroCuartos { get; set; }
        [Required(ErrorMessage = "Error, se requiere Nro de Baños.")]
        public int NroBaños { get; set; }
        [Required(ErrorMessage = "Error, se requiere Nro de Camas.")]
        public int NroCamas { get; set; }
        [Required(ErrorMessage = "Error, se requiere Wifi.")]
        public bool Wifi { get; set; }
        [Required(ErrorMessage = "Error, se requiere Estado.")]
        public string Estado { get; set; }


    }
}
