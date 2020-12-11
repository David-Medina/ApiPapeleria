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
    public class CopiasController : ControllerBase
    {
        private IDBService _servicioDB;

        public CopiasController(IDBService servicioDB)
        {
            _servicioDB = servicioDB;
        }

        [HttpGet]
        [Route("GetCopias")]
        public async Task<IActionResult> GetCopia()
        {
            var result = await _servicioDB.GetCopia();
            return Ok(result);
        }

        [HttpGet]
        [Route("GetCopiaID")]
        public async Task<IActionResult> GetCopiaId(int idcopia)
        {
            var result = await _servicioDB.GetCopiaId(idcopia);
            return Ok(result);
        }
        [HttpDelete]
        [Route("DeleteCopia")]
        public async Task<IActionResult> DelCopia(int idcopia)
        {
            var result = await _servicioDB.DelCopia(idcopia);
            return Ok(result);
        }
        [HttpPost]
        [Route("AddCopia")]
        public async Task<IActionResult> AddCopia([FromBody] Copia modelo)
        {
            var result = await _servicioDB.AddCopia(modelo);
            return Ok(result);
        }
        [HttpPost]
        [Route("UpdateCopia")]
        public async Task<IActionResult> UpdateCopia([FromBody] Copia modelo)
        {
            var result = await _servicioDB.UpdateCopia(modelo);
            return Ok(result);
        }
    }
}
