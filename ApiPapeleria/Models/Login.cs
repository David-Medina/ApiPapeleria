using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPapeleria.Models
{
    public class Login
    {
        public int idrol { set; get; }
        public int idUsuario { set; get; }
        public string nombre { set; get; }
        public string contrasenia { set; get;}
        public string usuario { set; get; }
        public string ApellidoMaterno { set; get; }
        public string ApellidoPaterno { set; get; }
        public int idMenu { set; get; }
        public string Menu { set; get; }
        public string Ruta { set; get; }
    }
}
