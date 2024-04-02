using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelTransilvania.Context;
using HotelTransilvania.Models;

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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Habitacion>>> GetHabitacion()
        {
          if (_context.Habitacion == null)
          {
              return NotFound();
          }
            return await _context.Habitacion.ToListAsync();
        }

        // GET: api/Habitacions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Habitacion>> GetHabitacion(int id)
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

            return habitacion;
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
            public string TipoHabitacion { get; set;}
            public DateTime FechaInicio { get; set; }
            public DateTime FechaFin { get; set; }


        }

        [HttpGet]
        [Route("buscar")]
        public async Task<ActionResult<IEnumerable<Habitacion>>> BuscarHabitacion([FromQuery] FiltroHabitacion filtros)
        {
            // Obtener las reservas que intersectan con el rango de fechas proporcionado
            var reservasEnFecha = await _context.Reserva
                .Where(r =>
                    (r.FechaInicio <= filtros.FechaFin && r.FechaFin >= filtros.FechaInicio) ||
                    (r.FechaInicio >= filtros.FechaInicio && r.FechaFin <= filtros.FechaFin))
                .Select(r => r.IdHabitacion)
                .ToListAsync();

            // Obtener las habitaciones que cumplen con los filtros de capacidad y tipo
            var habitacionesDisponibles = await _context.Habitacion
                .Where(h => h.Capacidad >= filtros.CantidadPersonas)
                .Where(h => h.Tipo == filtros.TipoHabitacion)
                .Where(h => !reservasEnFecha.Contains(h.Id))
                .ToListAsync();

            if (habitacionesDisponibles == null || habitacionesDisponibles.Count == 0)
            {
                return NotFound();
            }

            return habitacionesDisponibles;
        }

    }
}
