using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPapeleria.Models
{
    public class ResponseBase<T>
    {
        public bool TieneResultado { set; get; }
        public string Mensaje { set; get; }
        public T Modelo { set; get; }
    }
}
