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
    public class TicketController : ControllerBase
    {
        private IDBService _serviceDB;

        public TicketController(IDBService serviceDB)
        {
            _serviceDB = serviceDB;
        }

        [HttpPost]
        [Route("AddTicket")]
        public async Task<IActionResult> AddTicket([FromBody] ListaDetalle modelo)
        {
            var result = await _serviceDB.AddTicket(modelo);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetReporte")]
        public async Task<IActionResult> GetReporte()
        {
            var result = await _serviceDB.GetReporte();
            return Ok(result);
        }
    }
}
