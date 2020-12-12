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
    public class RolesController : ControllerBase
    {
        private IDBService _servicioDB;
        public RolesController(IDBService dBService)
        {
            _servicioDB = dBService;
        }

        [HttpGet]
        [Route("GetRolId")]
        public async Task<IActionResult> GetRolID(int idrol)
        {
            var result = await _servicioDB.GetUsuarioId(idrol);
            return Ok(result);
        }
    }
}
