using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HotelTransilvania.Services;
using System.Security.Claims;
using HotelTransilvania.Context;
using Microsoft.EntityFrameworkCore;
using HotelTransilvania.Models;


namespace HotelTransilvania.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class AuthController : ControllerBase
    {
        private readonly IJwtService _jwtService;

        private readonly RegisterLoginContext _context;

        public AuthController(
            RegisterLoginContext context,
            IJwtService jwtService
            )
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login([FromBody] LoginDto loginFormUsuario)
        {
            //TODO: buscar usuario por correo

            //            // verificar passwords

            Usuario? usuario = await _context.Usuario.FirstOrDefaultAsync(x => x.Correo == loginFormUsuario.Correo);

            if (usuario is null)
            {
                return BadRequest(new
                {
                    status = 400,
                    code = "INVALID_CREDENTIALS",
                    msg = "Usuario o contraseña incorrectas"
                });
            }

            bool verifiedPassword = BCrypt.Net.BCrypt.Verify(loginFormUsuario.Contraseña, usuario!.Contraseña);

            if (!verifiedPassword)
            {
                return BadRequest(new
                {
                    status = 400,
                    code = "INVALID_CREDENTIALS",
                    msg = "Usuario o contraseña incorrectas"
                });
            }

            object perfil;
            if (usuario.Rol == "CLIENTE")
            {
                perfil = await _context.Cliente
                    .Include(c => c.Persona)
                    .FirstOrDefaultAsync(x => x.IdUsuario == usuario.Id);
            }
            else if (usuario.Rol == "RECEPCIONISTA")
            {
                perfil = await _context.Recepcionista
                    .Include(c => c.Persona)
                    .FirstOrDefaultAsync(x => x.IdUsuario == usuario.Id);
            }
            else
            {
                return BadRequest(new
                {
                    status = 400,
                    code = "INVALID_PROFILE",
                    msg = "Perfil de usuario no válido"
                });
            }


            return Ok(new
            {
                status = StatusCodes.Status200OK,
                data = new
                {
                    usuario = new
                    {
                        usuario.Id,
                        usuario.Correo,
                        usuario.Contraseña,
                        usuario.Rol,
                        usuario.Estado,
                        perfil
                    },
                    token = _jwtService.GenerateToken(usuario)
                }
            });
        }

        [HttpGet, Authorize]
        [Route("refresh")]
        public async Task<ActionResult> GetProfile()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            int idUsuario = int.Parse(identity.Claims.FirstOrDefault(c => c.Type == "Id").Value);

            Usuario? usuario = await _context.Usuario.FirstOrDefaultAsync(x => x.Id == idUsuario);


            if (usuario == null)
            {
                return Unauthorized(new
                {
                    status = StatusCodes.Status401Unauthorized,
                    code = "INVALID_TOKEN",
                    msg = "Token inválido o expirado"
                });
            }

            object perfil;
            if (usuario.Rol == "CLIENTE")
            {
                perfil = await _context.Cliente
                    .Include(c => c.Persona)
                    .FirstOrDefaultAsync(x => x.IdUsuario == usuario.Id);
            }
            else if (usuario.Rol == "RECEPCIONISTA")
            {
                perfil = await _context.Recepcionista
                    .Include(c => c.Persona)
                    .FirstOrDefaultAsync(x => x.IdUsuario == usuario.Id);
            }
            else
            {
                return BadRequest(new
                {
                    status = 400,
                    code = "INVALID_PROFILE",
                    msg = "Perfil de usuario no válido"
                });
            }

            return Ok(new
            {
                status = StatusCodes.Status200OK,
                data = new
                {
                    usuario = new
                    {
                        usuario.Id,
                        usuario.Correo,
                        usuario.Contraseña,
                        usuario.Rol,
                        usuario.Estado,
                         perfil
                    },
                    token = _jwtService.GenerateToken(usuario)
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

            cliente.Usuario.Rol = "CLIENTE";
            cliente.Usuario.Contraseña = BCrypt.Net.BCrypt.HashPassword(cliente.Usuario.Contraseña);


            _context.Cliente.Add(cliente);
            await _context.SaveChangesAsync();

            Usuario? usuario = await _context.Usuario.FirstOrDefaultAsync(x => x.Id == cliente.Usuario.Id);

            object perfil;
            if (usuario.Rol == "CLIENTE")
            {
                perfil = await _context.Cliente
                    .Include(c => c.Persona)
                    .FirstOrDefaultAsync(x => x.IdUsuario == usuario.Id);
            }
            else if (usuario.Rol == "RECEPCIONISTA")
            {
                perfil = await _context.Recepcionista
                    .Include(c => c.Persona)
                    .FirstOrDefaultAsync(x => x.IdUsuario == usuario.Id);
            }
            else
            {
                return BadRequest(new
                {
                    status = 400,
                    code = "INVALID_PROFILE",
                    msg = "Perfil de usuario no válido"
                });
            }
           


            return Ok(new
            {
                status = StatusCodes.Status200OK,
                data = new
                {
                    usuario = new
                    {
                        usuario.Id,
                        usuario.Correo,
                        usuario.Contraseña,
                        usuario.Rol,
                        usuario.Estado,
                        perfil
                    },
                    token = _jwtService.GenerateToken(usuario)
                }
            });
        }
    }
}