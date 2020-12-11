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
    public class ProductoController : ControllerBase
    {
        private IDBService _servicioDB;

        public ProductoController(IDBService servicioDB)
        {
            _servicioDB = servicioDB;
        }

        [HttpGet]
        [Route("GetProductos")]
        public async Task<IActionResult> GetProductos()
        {
            var result = await _servicioDB.GetProductos();
            return Ok(result);
        }

        [HttpGet]
        [Route("GetProductoID")]
        public async Task<IActionResult> GetProductoId(int idproducto)
        {
            var result = await _servicioDB.GetProductosId(idproducto);
            return Ok(result);
        }
        [HttpDelete]
        [Route("DeleteProducto")]
        public async Task<IActionResult> DelProducto(int idproducto)
        {
            var result = await _servicioDB.DelProducto(idproducto);
            return Ok(result);
        }
        [HttpPost]
        [Route("AddProducto")]
        public async Task<IActionResult> AddProducto([FromBody] Producto modelo)
        {
            var result = await _servicioDB.AddProducto(modelo);
            return Ok(result);
        }
        [HttpPost]
        [Route("UpdateProducto")]
        public async Task<IActionResult> UpdateProducto([FromBody] Producto modelo)
        {
            var result = await _servicioDB.UpdateProducto(modelo);
            return Ok(result);
        }

        [HttpGet]
        [Route("QuitarProductos")]
        public async Task<IActionResult> QuitarProductos(int idticket)
        {
            var result = await _servicioDB.QuitarProductos(idticket);
            return Ok(result);
        }

    }
}
