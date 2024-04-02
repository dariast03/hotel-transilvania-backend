using HotelTransilvania.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelTransilvania.Context
{
    public class RegisterLoginContext:DbContext
    {
        public RegisterLoginContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Persona> Persona { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Recepcionista> Recepcionista { get; set; }
        public DbSet<Habitacion> Habitacion { get; set; } = default!;
        public DbSet<Reserva> Reserva { get; set; }
        public DbSet<HotelTransilvania.Models.Promocion> Promocion { get; set; } = default!;

    }
}
