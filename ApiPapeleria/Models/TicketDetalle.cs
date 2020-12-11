using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPapeleria.Models
{
    public class ListaDetalle
    {
        public string usuario { set; get; }
        public List<TicketDetalle> productos { set; get; }
    }
    public class TicketDetalle
    {

        public int IdTicketDetalle { set; get; }
        public int IdTicket { set; get;  }
        public int IdProducto { set; get; }
        public int IdCopia { set; get; }
        public int Cantidad { set; get; }
        public int totalProducto { set; get; }
    }
}
