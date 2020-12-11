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
    public class LoginController : ControllerBase
    {
        private IDBService _servicioDB;
        public LoginController(IDBService dBService)
        {
            _servicioDB = dBService;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> GetUsuarios(Login modelo)
        {
            var result = await _servicioDB.IniciarSesion(modelo);
            return Ok(result);
        }
    }
}
