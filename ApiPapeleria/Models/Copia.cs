using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPapeleria.Models
{
    public class Copia
    {
        public int IdCopias { set; get; }
        public int IdPapel{ set; get; }
        public int CantidadMinima { set; get; }
        public int CantidadMaxima { set; get; }
        public float precio { set; get; }
    }
}
