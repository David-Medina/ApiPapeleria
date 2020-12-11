using ApiPapeleria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPapeleria.Services
{
    public interface IDBService
    {
        //Usuarios 
        Task<ResponseBase<IEnumerable<Usuario>>> GetUsuario();
        Task<ResponseBase<Usuario>> GetUsuarioId(int idusuario);
        Task<ResponseBase<int>> AddUsuario(Usuario modelo);
        Task<ResponseBase<int>> UpdateUsuario(Usuario modelo);
        Task<ResponseBase<bool>> DelUsuario(int id);
        //Copias
        Task<ResponseBase<IEnumerable<Copia>>> GetCopia();
        Task<ResponseBase<Copia>> GetCopiaId(int idcopia);
        Task<ResponseBase<int>> AddCopia(Copia modelo);
        Task<ResponseBase<int>> UpdateCopia(Copia modelo);
        Task<ResponseBase<bool>> DelCopia(int id);

        //Papel
        Task<ResponseBase<IEnumerable<Papel>>> GetPapel();
        Task<ResponseBase<Papel>> GetPapelId(int idpapel);
        Task<ResponseBase<int>> AddPapel(Papel modelo);
        Task<ResponseBase<int>> UpdatePapel(Papel modelo);
        Task<ResponseBase<bool>> DelPapel(int id);
        //Productos
        Task<ResponseBase<IEnumerable<Producto>>> GetProductos();
        Task<ResponseBase<Producto>> GetProductosId(int idproducto);
        Task<ResponseBase<int>> AddProducto(Producto modelo);
        Task<ResponseBase<int>> UpdateProducto(Producto modelo);
        Task<ResponseBase<bool>> DelProducto(int id);

        //Login
        Task<ResponseBase<Login>> IniciarSesion(Login modelo);

        //Ticket y Ticket Detalle
        Task<ResponseBase<int>> AddTicket(ListaDetalle modelo);
        Task<ResponseBase<TicketDetalle>> GetTicketDetalle(List<TicketDetalle> modelo);

        //Productos
        Task<ResponseBase<int>> QuitarProductos(int idticket);

        Task<ResponseBase<bool>> GetReporte();
    }
}
