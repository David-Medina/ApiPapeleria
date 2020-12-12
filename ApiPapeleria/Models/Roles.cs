using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPapeleria.Models
{
    public class Roles
    {
        public int idrol { set; get; }
        public string rol { set; get; }

        public int idRolMenu { set; get; }
        public int idRol { set; get; }
        public int idMenu { set; get; }
        public int permiso { set; get; }
    }
}
