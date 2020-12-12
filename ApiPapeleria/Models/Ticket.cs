using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPapeleria.Models
{
    public class Ticket
    {
        public int IdTicket { set; get; }
        public string FechaCompra { set; get; }
        public float Total { set; get; }
        public int idUsuario { set; get; }
        public string Usuario { set; get; }
    }
}
