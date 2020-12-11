using ApiPapeleria.Models;
using ApiPapeleria.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPapeleria.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private IDBService _servicioDB;

        public UsuarioController(IDBService servicioDB)
        {
            _servicioDB = servicioDB;
        }

        [HttpGet]
        [Route("GetUsuarios")]
        public async Task<IActionResult> GetUsuarios()
        {
            var result = await _servicioDB.GetUsuario();
            return Ok(result);
        }
        [HttpGet]
        [Route("Login")]
        public async Task<IActionResult> Login(int idusuario)
        {
            var result = await _servicioDB.GetUsuarioId(idusuario);
            return Ok(result);
        }
        [HttpGet]
        [Route("GetUsuarioID")]
        public async Task<IActionResult> GetUsuarioId(int idusuario)
        {
            var result = await _servicioDB.GetUsuarioId(idusuario);
            return Ok(result);
        }
        [HttpDelete]
        [Route("DeleteUsuario")]
        public async Task<IActionResult> DelUsuario(int idusuario)
        {
            var result = await _servicioDB.DelUsuario(idusuario);
            return Ok(result);
        }
        [HttpPost]
        [Route("AddUsuario")]
        public async Task<IActionResult> AddUsuario([FromBody] Usuario modelo)
        {
            var result = await _servicioDB.AddUsuario(modelo);
            return Ok(result);
        }
        [HttpPost]
        [Route("UpdateUsuario")]
        public async Task<IActionResult> UpdateUsuario([FromBody] Usuario modelo)
        {
            var result = await _servicioDB.UpdateUsuario(modelo);
            return Ok(result);
        }
    }
}
