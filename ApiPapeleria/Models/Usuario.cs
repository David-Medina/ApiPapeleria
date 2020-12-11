using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPapeleria.Models
{
    public class Usuario
    {
        public int IdUsuario { set; get; }
        public string NUsuario { set; get; }
        public string Contrasenia { set; get; }
        public string Nombre { set; get; }
        public string ApellidoPaterno { set; get; }
        public string ApellidoMaterno { set; get; }
        public int idrol { set; get; }
    }
}
