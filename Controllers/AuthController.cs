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

        #region ROUTES

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login([FromBody] LoginDto loginFormUsuario)
        {
            //TODO: buscar usuario por correo

            // verificar passwords

            bool verifiedPassword = BCrypt.Net.BCrypt.Verify(contrasenaSinEncruptar, usuarioEncontrado.Contrasena);


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
        [Route("perfil")]
        public async Task<ActionResult> GetProfile()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            int idUsuario = int.Parse(identity.Claims.FirstOrDefault(c => c.Type == "Id").Value);

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
                    perfil,
                    token = _jwtService.GenerateToken(usuarioEncontrado)
                }
            });

        }



        [HttpPost, Route("register")]
        public async Task<ActionResult> Register(Persona persona)
        {
            if (await _context.Usuario.FirstOrDefaultAsync(x => x.Correo == persona!.Cliente!.Usuario!.Correo) != null)
            {
                return BadRequest(new
                {
                    msg = "El correo ya se encuentra registrado",
                    status = 400,
                    code = "DUPLICATE_EMAIL",
                });
            }

            if (await _context.Persona.FirstOrDefaultAsync(x => x.NroDocumento == persona.NroDocumento) != null)
            {
                return BadRequest(new
                {
                    msg = "El nro de documento ya se encuentra registrado",
                    status = 400,
                    code = "DUPLICATE_DOCUMENT",
                });
            }

            persona.Perfil.Usuario.Contrasena = BCrypt.Net.BCrypt.HashPassword(persona.Perfil.Usuario.Contrasena);


            _context.Persona.Add(persona);
            await _context.SaveChangesAsync();




            Usuario? usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == persona.Perfil.Usuario.Id);



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