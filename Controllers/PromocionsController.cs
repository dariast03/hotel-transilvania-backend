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
    public class PromocionsController : ControllerBase
    {
        private readonly RegisterLoginContext _context;
        public PromocionsController(RegisterLoginContext context)
        {
            _context = context;
        }
        // GET: api/Promocions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Promocion>>> GetPromocion()
        {
          if (_context.Promocion == null)
          {
              return NotFound();
          }
            return await _context.Promocion.ToListAsync();
        }
        // GET: api/Promocions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Promocion>> GetPromocion(int id)
        {
          if (_context.Promocion == null)
          {
              return NotFound();
          }
            var promocion = await _context.Promocion.FindAsync(id);
            if (promocion == null)
            {
                return NotFound();
            }
            return promocion;
        }
        // PUT: api/Promocions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPromocion(int id, Promocion promocion)
        {
            if (id != promocion.Id)
            {
                return BadRequest();
            }
            _context.Entry(promocion).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PromocionExists(id))
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
        // POST: api/Promocions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Promocion>> PostPromocion(Promocion promocion)
        {
          if (_context.Promocion == null)
          {
              return Problem("Entity set 'RegisterLoginContext.Promocion'  is null.");
          }
            _context.Promocion.Add(promocion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPromocion", new { id = promocion.Id }, promocion);
        }
        // DELETE: api/Promocions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePromocion(int id)
        {
            if (_context.Promocion == null)
            {
                return NotFound();
            }
            var promocion = await _context.Promocion.FindAsync(id);
            if (promocion == null)
            {
                return NotFound();
            }
            _context.Promocion.Remove(promocion);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool PromocionExists(int id)
        {
            return (_context.Promocion?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        // POST: api/Promocions/ApplyDiscounts
        [HttpPost("ApplyDiscounts")]
        public async Task<ActionResult> ApplyDiscounts()
        {
            var promociones = await _context.Promocion.ToListAsync();
            foreach (var promocion in promociones)
            {
                // Check if the discount applies
                if (promocion.AplicaDescuento())
                {
                    // Get the client
                    var cliente = await _context.Cliente.FindAsync(promocion.IdCliente);

                    // Apply the discount to the client's room price
                    var habitacion = await _context.Habitacion.FindAsync(cliente.Id);
                    habitacion.Precio -= habitacion.Precio * promocion.Descuento;

                    // Update the room in the context
                    _context.Entry(habitacion).State = EntityState.Modified;
                }
            }

            // Save changes in the context
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
