using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPapeleria.Models
{
    public class Producto
    {
        public int idProducto { set; get; }
        public string ProductoN { set; get; }
        public int cantidad { set; get; }
        public float costo_unitario { set; get; }
        public float costo_mayoreo { set; get; }
    }
}
