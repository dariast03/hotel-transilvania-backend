using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelTransilvania.Context;
using HotelTransilvania.Models;
using System.ComponentModel.DataAnnotations;

namespace HotelTransilvania.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HabitacionController : ControllerBase
    {
        private readonly RegisterLoginContext _context;

        public HabitacionController(RegisterLoginContext context)
        {
            _context = context;
        }

        // GET: api/Habitacions
        // GET: api/Habitacions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetHabitacion()
        {
            if (_context.Habitacion == null)
            {
                return NotFound();
            }

            var userIdClaim = HttpContext.User.FindFirst("Id")?.Value;
            int? userId = userIdClaim != null ? int.Parse(userIdClaim) : null;

            var cliente = userId.HasValue ? await _context.Cliente.FirstOrDefaultAsync(c => c.IdUsuario == userId) : null;

            var promocionActiva = await _context.Promocion
                                .OrderByDescending(p => p.Id)
                                .FirstOrDefaultAsync();

            var habitaciones = await _context.Habitacion.ToListAsync();

            var habitacionesConDescuento = new List<object>();

            if (promocionActiva != null && cliente != null && cliente.Frecuencia == "Frecuente")
            {
                foreach (var habitacion in habitaciones)
                {
                    float precioConDescuento = habitacion.Precio;

                    if (cliente.Frecuencia == "Frecuente")
                    {
                        precioConDescuento -= precioConDescuento * promocionActiva.Descuento;
                    }

                    habitacionesConDescuento.Add(new
                    {
                        habitacion.Id,
                        habitacion.Codigo,
                        habitacion.Nombre,
                        habitacion.Capacidad,
                        habitacion.Tipo,
                        habitacion.Precio,
                        habitacion.Imagen,
                        habitacion.NroBaños,
                        habitacion.NroCamas,
                        habitacion.NroCuartos,
                        habitacion.Ubicacion,
                        habitacion.Wifi,
                        habitacion.Estado,
                        PrecioConDescuento = precioConDescuento
                    });
                }

                return habitacionesConDescuento;
            }
            else
            {
                return habitaciones;
            }



            return new List<Habitacion>();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetHabitacion(int id)
        {
            var userIdClaim = HttpContext.User.FindFirst("Id")?.Value;
            int? userId = userIdClaim != null ? int.Parse(userIdClaim) : null;

            var cliente = userId.HasValue ? await _context.Cliente.FirstOrDefaultAsync(c => c.IdUsuario == userId) : null;

            var promocionActiva = await _context.Promocion
                                                .OrderByDescending(p => p.Id)
                                                .FirstOrDefaultAsync();

            var habitacion = await _context.Habitacion.FindAsync(id);

            if (habitacion == null)
            {
                return NotFound();
            }

            var reservas = await _context.Reserva
                                .Where(r => r.IdHabitacion == id && r.Estado == "Confirmada")
                                .OrderBy(r => r.FechaInicio)
                                .ToListAsync();

            DateTime fechaInicioDisponible = DateTime.Now.Date;
            DateTime fechaFinDisponible = DateTime.MaxValue.Date;

            foreach (var reserva in reservas)
            {
                if (reserva.FechaInicio > fechaInicioDisponible)
                {
                    fechaInicioDisponible = reserva.FechaFin.AddDays(1).Date;
                }

                if (reserva.FechaFin < fechaFinDisponible)
                {
                    fechaFinDisponible = reserva.FechaInicio.AddDays(-1).Date;
                }
            }

            fechaFinDisponible = fechaInicioDisponible.AddDays(60).Date;

            if (promocionActiva != null && cliente != null && cliente.Frecuencia == "Frecuente")
            {
                float precioConDescuento = habitacion.Precio;

                if (cliente.Frecuencia == "Frecuente")
                {
                    precioConDescuento -= precioConDescuento * promocionActiva.Descuento;
                }

                var habitacionConDescuento = new
                {
                    habitacion.Id,
                    habitacion.Codigo,
                    habitacion.Nombre,
                    habitacion.Capacidad,
                    habitacion.Tipo,
                    habitacion.Precio,
                    habitacion.Imagen,
                    habitacion.NroBaños,
                    habitacion.NroCamas,
                    habitacion.NroCuartos,
                    habitacion.Ubicacion,
                    habitacion.Wifi,
                    habitacion.Estado,
                    FechaDeInicioDisponible = fechaInicioDisponible,
                    FechaFinDisponible = fechaFinDisponible,
                    PrecioConDescuento = precioConDescuento
                };

                return habitacionConDescuento;
            }
            else
            {
                var habitacionSinDescuento = new
                {
                    habitacion.Id,
                    habitacion.Codigo,
                    habitacion.Nombre,
                    habitacion.Capacidad,
                    habitacion.Tipo,
                    habitacion.Precio,
                    habitacion.Imagen,
                    habitacion.NroBaños,
                    habitacion.NroCamas,
                    habitacion.NroCuartos,
                    habitacion.Ubicacion,
                    habitacion.Wifi,
                    habitacion.Estado,
                    PrecioConDescuento = habitacion.Precio,
                    FechaDeInicioDisponible = fechaInicioDisponible,
                    FechaFinDisponible = fechaFinDisponible
                };

                return habitacionSinDescuento;
            }
        }


        // PUT: api/Habitacion/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHabitacion(int id, Habitacion habitacion)
        {
            if (id != habitacion.Id)
            {
                return BadRequest();
            }

            _context.Entry(habitacion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HabitacionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Habitacions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Habitacion>> PostHabitacion(Habitacion habitacion)
        {
            if (_context.Habitacion == null)
            {
                return Problem("Entity set 'RegisterLoginContext.Habitacion'  is null.");
            }
            _context.Habitacion.Add(habitacion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHabitacion", new { id = habitacion.Id }, habitacion);
        }

        // DELETE: api/Habitacions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHabitacion(int id)
        {
            if (_context.Habitacion == null)
            {
                return NotFound();
            }
            var habitacion = await _context.Habitacion.FindAsync(id);
            if (habitacion == null)
            {
                return NotFound();
            }

            _context.Habitacion.Remove(habitacion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HabitacionExists(int id)
        {
            return (_context.Habitacion?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        public class FiltroHabitacion
        {
            public int CantidadPersonas { get; set; }
            public string TipoHabitacion { get; set; }
            [DataType(DataType.Date)]
            public DateTime FechaInicio { get; set; }
            [DataType(DataType.Date)]
            public DateTime FechaFin { get; set; }


        }

        [HttpGet]
        [Route("buscar")]
        public async Task<ActionResult<IEnumerable<Habitacion>>> BuscarHabitacion([FromQuery] FiltroHabitacion filtros)
        {

            var reservasEnFecha = await _context.Reserva
                .Where(r =>
                    (r.FechaInicio.Date <= filtros.FechaFin.Date && r.FechaFin.Date >= filtros.FechaInicio.Date) ||
                    (r.FechaInicio.Date >= filtros.FechaInicio.Date && r.FechaFin.Date <= filtros.FechaFin.Date))
                .Select(r => r.IdHabitacion)
                .ToListAsync();


            var habitacionesDisponibles = await _context.Habitacion
                .Where(h => h.Capacidad >= filtros.CantidadPersonas)
                .Where(h => h.Tipo == filtros.TipoHabitacion)
                .Where(h => !reservasEnFecha.Contains(h.Id))
                .ToListAsync();

            if (habitacionesDisponibles == null || habitacionesDisponibles.Count == 0)
            {
                return new List<Habitacion>();
            }

            return habitacionesDisponibles;
        }


        [HttpGet("fechasOcupadas/{idHabitacion}")]
        public async Task<ActionResult<IEnumerable<DateTime>>> GetFechasOcupadas(int idHabitacion)
        {
            var reservas = await _context.Reserva
                .Where(r => r.IdHabitacion == idHabitacion && r.Estado == "Confirmada")
                .ToListAsync();

            var fechasOcupadas = new List<DateTime>();

            foreach (var reserva in reservas)
            {
                var fechaInicio = reserva.FechaInicio.Date;
                var fechaFin = reserva.FechaFin.Date;

                for (DateTime fecha = fechaInicio; fecha <= fechaFin; fecha = fecha.AddDays(1))
                {
                    fechasOcupadas.Add(fecha);
                }
            }

            return Ok(fechasOcupadas.Distinct());
        }
    }
}
