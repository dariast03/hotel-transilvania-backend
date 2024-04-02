using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HotelTransilvania.Services;
using System.Data;
using System.Security.Claims;
using HotelTransilvania.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using HotelTransilvania.Models;
using Microsoft.CodeAnalysis.Scripting;
using Org.BouncyCastle.Crypto.Generators;

namespace HotelTransilvania.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class AuthController : ControllerBase
    {
        private readonly IJwtService _jwtService;

        private readonly RegisterLoginContext _context;
        private readonly EmailService _emailService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AuthController(
            RegisterLoginContext context,
            IJwtService jwtService,
            EmailService emailService,
            IWebHostEnvironment webHostEnvironment
            )
        {
            _context = context;
            _jwtService = jwtService;
            _emailService = emailService;
            _webHostEnvironment = webHostEnvironment;


        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login([FromBody] LoginDto loginFormUsuario)
        {
            //TODO: buscar usuario por correo

//            // verificar passwords

            Usuario? usuarioEncontrado = await _context.Usuario.FirstOrDefaultAsync(x => x.Correo == loginFormUsuario.Correo);

            bool verifiedPassword = BCrypt.Net.BCrypt.Verify(loginFormUsuario.Contraseña, usuarioEncontrado!.Contraseña);


            if (usuarioEncontrado is null)
            {
                return BadRequest(new
                {
                    status = 400,
                    code = "INVALID_CREDENTIALS",
                    msg = "Usuario o contraseña incorrectas"
                });
            }

            return Ok(new
            {
                status = StatusCodes.Status200OK,
                data = new
                {
                    usuario = usuarioEncontrado,
                    token = _jwtService.GenerateToken(usuarioEncontrado)
                }
            });
        }

        [HttpGet, Authorize]
        [Route("Refresh")]
        public async Task<ActionResult> GetProfile()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            int idUsuario = int.Parse(identity.Claims.FirstOrDefault(c => c.Type == "Id").Value);

            Usuario? usuarioEncontrado = await _context.Usuario.FirstOrDefaultAsync(x => x.Id == idUsuario);

            if (idUsuario == 0)
            {
                return Unauthorized(new
                {
                    status = StatusCodes.Status401Unauthorized,
                    code = "SESSION_EXPIRED",
                    msg = "La sesión ha caducado"
                });
            }

            //buscar usuario por id


            if (usuarioEncontrado == null)
            {
                return Unauthorized(new
                {
                    status = StatusCodes.Status401Unauthorized,
                    code = "INVALID_TOKEN",
                    msg = "Token inválido o expirado"
                });
            }

            return Ok(new
            {
                status = StatusCodes.Status200OK,
                data = new
                {
                    usuario = usuarioEncontrado,
                    token = _jwtService.GenerateToken(usuarioEncontrado)
                }
            });

        }



        [HttpPost, Route("register")]
        public async Task<ActionResult> Registrar(Cliente cliente)
        {
            if (await _context.Usuario.FirstOrDefaultAsync(x => x.Correo == cliente!.Usuario!.Correo) != null)
            {
                return BadRequest(new
                {
                    msg = "El correo ya se encuentra registrado",
                    status = 400,
                    code = "DUPLICATE_EMAIL",
                });
            }

            cliente.Usuario.Contraseña = BCrypt.Net.BCrypt.HashPassword(cliente.Usuario.Contraseña);


            _context.Cliente.Add(cliente);
            await _context.SaveChangesAsync();




            Usuario? usuario = await _context.Usuario.FirstOrDefaultAsync(x => x.Id == cliente.Usuario.Id);



            return Ok(new
            {
                status = StatusCodes.Status200OK,
                data = new
                {
                    usuario,
                    token = _jwtService.GenerateToken(usuario)
                }
            });
        }
    }
}