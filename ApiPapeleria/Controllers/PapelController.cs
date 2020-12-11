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
    public class PapelController : ControllerBase
    {
        private IDBService _servicioDB;

        public PapelController(IDBService servicioDB)
        {
            _servicioDB = servicioDB;
        }

        [HttpGet]
        [Route("GetPapel")]
        public async Task<IActionResult> GetPapel()
        {
            var result = await _servicioDB.GetPapel();
            return Ok(result);
        }

        [HttpGet]
        [Route("GetPapelID")]
        public async Task<IActionResult> GetPapelId(int idpapel)
        {
            var result = await _servicioDB.GetPapelId(idpapel);
            return Ok(result);
        }
        [HttpDelete]
        [Route("DeletePapel")]
        public async Task<IActionResult> DelPapel(int idpapel)
        {
            var result = await _servicioDB.DelPapel(idpapel);
            return Ok(result);
        }
        [HttpPost]
        [Route("AddPapel")]
        public async Task<IActionResult> AddPapel([FromBody] Papel modelo)
        {
            var result = await _servicioDB.AddPapel(modelo);
            return Ok(result);
        }
        [HttpPost]
        [Route("UpdatePapel")]
        public async Task<IActionResult> UpdatePapel([FromBody] Papel modelo)
        {
            var result = await _servicioDB.UpdatePapel(modelo);
            return Ok(result);
        }
    }
}
