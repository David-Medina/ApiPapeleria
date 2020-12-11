using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPapeleria.Models
{
    public class ListaProducto
    {
        public List<QuitarProducto> quitarProductos { set; get; }
    }
    public class QuitarProducto
    {
        public string idquitarproducto { set; get; }

        public int idproducto { set; get; }


    }
}
