using HotelTransilvania.Context;
using HotelTransilvania.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelTransilvania.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadoSistemaController : ControllerBase
    {
        private readonly RegisterLoginContext _context;
        public EstadoSistemaController(RegisterLoginContext context)
        {
            _context = context;
        }

        [HttpGet("pendientes")]
        public async Task<ActionResult<IEnumerable<string>>> ReservasPendientes()
        {
            var cantidad = _context.Reserva.Where(r => r.Estado == "Pendiente").Count();

            return Ok(cantidad);
        }

        [HttpGet("confirmadas")]
        public async Task<ActionResult<IEnumerable<string>>> ReservasConfirmadas()
        {
            var cantidad = _context.Reserva.Where(r => r.Estado == "Confirmada").Count();

            return Ok(cantidad);
        }

        //[HttpGet("habitaciones-disponibles")]
        //public async Task<ActionResult<IEnumerable<string>>> Habitaciones()
        //{
        //    var cantidad = _context.Reserva.Where(r => r.Estado == "Confirmada").Count();

        //    return Ok(cantidad);
        //}

    }
}
