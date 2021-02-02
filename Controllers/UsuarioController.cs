using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TarefasBackEnd.Models;
using TarefasBackEnd.Models.ViewModels;
using TarefasBackEnd.Repositories;

namespace TarefasBackEnd.Controllers {

    [ApiController]
    [Route("usuarios")]
    public class UsuarioController : ControllerBase 
    {
     
        [HttpPost]
        [Route("")]
        public IActionResult Create ([FromServices] IUsuarioRepository repository, [FromBody] Usuario model) {
            if(!ModelState.IsValid){
                return BadRequest();
            }
            repository.Create(model);
            return Ok();
        }
        [HttpPost]
        [Route("login")]
        public IActionResult Login ([FromServices] IUsuarioRepository repository, [FromBody] UsuarioLogin model) 
        {
            if(!ModelState.IsValid){
                return BadRequest();
            }

             Usuario usuario =  repository.Read(model.Email, model.Senha);
            
             if(usuario == null) {
                 return Unauthorized();
             }
             usuario.Senha = "";
             return Ok(new {usuario = usuario,
                            token = GenerateToken(usuario)
                            });
        }

        private string GenerateToken(Usuario usuario) 
        {

            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes("UmaTokenMuitoGrandeParaNaoDescrobir");

            var descriptor = new SecurityTokenDescriptor {
                Subject  = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, usuario.Id.ToString()),
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256  
                )
            };

            var token = tokenHandler.CreateToken(descriptor);
            return tokenHandler.WriteToken(token);
        }
        
    }
}