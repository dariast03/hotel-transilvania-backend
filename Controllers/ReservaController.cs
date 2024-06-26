﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelTransilvania.Context;
using HotelTransilvania.Models;
using System.Security.Claims;
using HotelTransilvania.Services;
using Microsoft.AspNetCore.Authorization;
namespace HotelTransilvania.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservaController : ControllerBase
    {
        private readonly RegisterLoginContext _context;
        private readonly EmailService _emailService;
        public ReservaController(RegisterLoginContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }
        // GET: api/Reserva
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reserva>>> GetReserva()
        {
            if (_context.Reserva == null)
            {
                return NotFound();
            }
            return await _context.Reserva
                .Include(r => r.Habitacion)
                .Include(r => r.Cliente)
                .ThenInclude(c => c.Persona)
                .ToListAsync();
        }

        [HttpGet, Route("me")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Reserva>>> MisReservas()
        {
            if (_context.Reserva == null)
            {
                return NotFound();
            }

            var identity = HttpContext.User.Identity as ClaimsIdentity;

            int idUsuario = int.Parse(identity.Claims?.FirstOrDefault(c => c.Type == "Id")?.Value);

            Cliente? cliente = await _context.Cliente.FirstOrDefaultAsync(x => x.IdUsuario == idUsuario);

            return await _context.Reserva
                .Include(r => r.Habitacion)
                .Include(r => r.Cliente)
                .ThenInclude(c => c.Persona)
                .Where(r => r.IdCliente == cliente.Id)
                .ToListAsync();
        }

        [HttpGet("pendientes")]
        public async Task<ActionResult<IEnumerable<Reserva>>> GetReservasPendientes()
        {
            if (_context.Reserva == null)
            {
                return NotFound();
            }
            return await _context.Reserva
                  .Include(r => r.Habitacion)
                .Include(r => r.Cliente)
                .ThenInclude(c => c.Persona)
                .ToListAsync();
        }
        // GET: api/Reserva/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Reserva>> GetReserva(int id)
        {
            if (_context.Reserva == null)
            {
                return NotFound();
            }
            var reserva = await _context.Reserva
                  .Include(r => r.Habitacion)
                .Include(r =>r.Cliente)
                .ThenInclude(c => c.Persona)
                .FirstOrDefaultAsync(r => r.Id ==id);
            if (reserva == null)
            {
                return NotFound();
            }
            return reserva;
        }
        // PUT: api/Reserva/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReserva(int id, Reserva reserva)
        {
            if (id != reserva.Id)
            {
                return BadRequest();
            }

             //  reserva.Estado = "Pendiente"; //Rechazado, Reservado, Cancelado/
            _context.Entry(reserva).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();

              
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservaExists(id))
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
        [HttpPut("confirmar/{id}")]
        public async Task<IActionResult> ConfirmarReserva(int id)
        {
            var reserva = await _context.Reserva.FindAsync(id);
            if (reserva == null)
            {
                return NotFound();
            }
            reserva.Estado = "Confirmada";//Confirmada
            _context.Entry(reserva).State = EntityState.Modified;
            var cliente = await _context.Cliente
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(c => c.Id == reserva.IdCliente);
            var habitacion = await _context.Habitacion.FindAsync(reserva.IdHabitacion);

            if (cliente == null)
            {
                return NotFound();
            }
            if (cliente.Frecuencia == null)
            {
                cliente.Frecuencia = "1";
            }
            else
            {
                int frecuencia;
                if(int.TryParse(cliente.Frecuencia, out frecuencia) && cliente.Frecuencia != "Frecuente")
                {
                    frecuencia++;
                    if (frecuencia >= 3)
                    {
                        cliente.Frecuencia = "Frecuente";
                    }
                    else
                    {
                        cliente.Frecuencia = frecuencia.ToString();
                    }
                }
                else
                {
                    cliente.Frecuencia = "1";
                }

             
            }
            _context.Entry(cliente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();


                _ = _emailService.SendEmailAsync(cliente?.Usuario?.Correo, "Reserva Confirmada", $"Tu reserva de {habitacion?.Nombre}  para las fechas {reserva.FechaInicio}-{reserva.FechaFin} ha sido aprobada con exito");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservaExists(id))
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
        [HttpPut("rechazar/{id}")]
        public async Task<IActionResult> RechazarReserva(int id)
        {
            var reserva = await _context.Reserva.FindAsync(id);
            reserva.Estado = "Rechazado";

            var cliente = await _context.Cliente.FindAsync(reserva.IdCliente);
            var habitacion = await _context.Habitacion.FindAsync(reserva.IdHabitacion);
            


            _context.Entry(reserva).State = EntityState.Modified;
            if (reserva == null)
            {
                return NotFound();
            }

            try
            {
                await _context.SaveChangesAsync();
                _ = _emailService.SendEmailAsync(cliente.Usuario.Correo, "Reserva Confirmada", $"Tu reserva de {habitacion.Nombre}  para las fechas {reserva.FechaInicio}-{reserva.FechaFin} ha sido rechazada PIPIPI. ");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservaExists(id))
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
        [HttpPut("cancelar/{id}")]
        public async Task<IActionResult> CancelarReserva(int id)
        {
            var reserva = await _context.Reserva.FindAsync(id);
            reserva.Estado = "Cancelado";
            _context.Entry(reserva).State = EntityState.Modified;
            if (reserva == null)
            {
                return NotFound();
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservaExists(id))
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

        [HttpPut("espera/{id}")]
        public async Task<IActionResult> EsperaReserva(int id)
        {
            var reserva = await _context.Reserva.FindAsync(id);
            reserva.Estado = "En Espera";
            _context.Entry(reserva).State = EntityState.Modified;
            if (reserva == null)
            {
                return NotFound();
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservaExists(id))
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
        // POST: api/Reserva
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Reserva>> PostReserva(Reserva reserva)
        {
            if (_context.Reserva == null)
            {
                return Problem("Entity set 'RegisterLoginContext.Reserva'  is null.");
            }


            _context.Reserva.Add(reserva);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetReserva", new { id = reserva.Id }, reserva);
        }
        // DELETE: api/Reserva/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReserva(int id)
        {
            if (_context.Reserva == null)
            {
                return NotFound();
            }
            var reserva = await _context.Reserva.FindAsync(id);
            if (reserva == null)
            {
                return NotFound();
            }

            _context.Reserva.Remove(reserva);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReservaExists(int id)
        {
            return (_context.Reserva?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
