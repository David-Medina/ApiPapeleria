using ApiPapeleria.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPapeleria.Services
{
    public class DBService : IDBService
    {
        private SqlConnection _connection;
        public DBService()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var connString = config.GetConnectionString("Default");

            _connection = new SqlConnection(connString);
        }
        //Usuarios
        public async Task<ResponseBase<int>> AddUsuario(Usuario modelo)
        {
            //Variables insert y Update porque son las mismas
            var st_parametro_idrol = new SqlParameter("@idrol", System.Data.SqlDbType.Int);
            st_parametro_idrol.Value = modelo.idrol;

            var st_parametro_usuario = new SqlParameter("@usuario", System.Data.SqlDbType.NVarChar,500);
            st_parametro_usuario.Value = modelo.NUsuario;

            var st_parametro_pass = new SqlParameter("@contrasenia", System.Data.SqlDbType.NVarChar,500);
            st_parametro_pass.Value = modelo.Contrasenia;

            var st_parametro_nombre = new SqlParameter("@nombre", System.Data.SqlDbType.NVarChar,500);
            st_parametro_nombre.Value = modelo.Nombre;

            var st_parametro_apemat = new SqlParameter("@apellidoMaterno", System.Data.SqlDbType.NVarChar,500);
            st_parametro_apemat.Value = modelo.ApellidoMaterno;

            var st_parametro_apepat = new SqlParameter("@apellidoPaterno", System.Data.SqlDbType.NVarChar, 500);
            st_parametro_apepat.Value = modelo.ApellidoPaterno;

            var st_parametro_idusout = new SqlParameter("@IdUsuarioOut", System.Data.SqlDbType.Int);
            st_parametro_idusout.Direction = System.Data.ParameterDirection.Output;

            var st_parametro_estado = new SqlParameter("@estado", System.Data.SqlDbType.NVarChar,500);
            st_parametro_estado.Direction = System.Data.ParameterDirection.Output;

            var st_parametro_error = new SqlParameter("@error", System.Data.SqlDbType.Bit);
            st_parametro_error.Direction = System.Data.ParameterDirection.Output;

            try
            {
                _connection.Open();

                if (_connection.State == System.Data.ConnectionState.Open)
                {
                    string sql = $"add_usuario";
                    SqlCommand commado = new SqlCommand(sql, _connection);
                    commado.CommandType = System.Data.CommandType.StoredProcedure;
                    commado.Parameters.Add(st_parametro_idrol);
                    commado.Parameters.Add(st_parametro_usuario);
                    commado.Parameters.Add(st_parametro_pass);
                    commado.Parameters.Add(st_parametro_nombre);
                    commado.Parameters.Add(st_parametro_apemat);
                    commado.Parameters.Add(st_parametro_apepat);
                    commado.Parameters.Add(st_parametro_idusout);
                    commado.Parameters.Add(st_parametro_estado);
                    commado.Parameters.Add(st_parametro_error);
                    await commado.ExecuteNonQueryAsync();
                }

             
            }
            catch (Exception e)
            {
                return new ResponseBase<int> { TieneResultado = false, Mensaje = "Error Interno de Api", Modelo = 0 };
            }
            finally
            {
                _connection.Close();
            }
            return new ResponseBase<int> { TieneResultado = (bool) st_parametro_error.Value, Mensaje = st_parametro_estado.Value.ToString(), Modelo = int.Parse(st_parametro_idusout.Value.ToString()) };

        }

        public async Task<ResponseBase<bool>> DelUsuario(int idusuario)
        {

            var st_parametro_idusuario = new SqlParameter("@idusuario", System.Data.SqlDbType.Int);
            st_parametro_idusuario.Value = idusuario;

            var st_parametro_estado = new SqlParameter("@estado", System.Data.SqlDbType.NVarChar,500);
            st_parametro_estado.Direction = System.Data.ParameterDirection.Output;

            var st_parametro_error = new SqlParameter("@error", System.Data.SqlDbType.Bit);
            st_parametro_error.Direction = System.Data.ParameterDirection.Output;

            try
            {
                _connection.Open();

                if (_connection.State == System.Data.ConnectionState.Open)
                {
                    string sql = $"delete_usuario";
                    SqlCommand commado = new SqlCommand(sql, _connection);
                    commado.CommandType = System.Data.CommandType.StoredProcedure;
                    commado.Parameters.Add(st_parametro_idusuario);
                    commado.Parameters.Add(st_parametro_estado);
                    commado.Parameters.Add(st_parametro_error);

                    await commado.ExecuteNonQueryAsync();

                   // return new ResponseBase<bool> { TieneResultado = (bool)st_parametro_error.Value, Mensaje = st_parametro_estado.Value.ToString(), Modelo = (bool)st_parametro_error.Value };
                }
                
            }
            catch (Exception e)
            {
                return new ResponseBase<bool> { TieneResultado = false, Mensaje = "Error en la base de datos ", Modelo = false };
            }
            finally
            {
                _connection.Close();
            }

            return new ResponseBase<bool> { TieneResultado = (bool)st_parametro_error.Value, Mensaje = st_parametro_estado.Value.ToString(), Modelo = (bool)st_parametro_error.Value };
        }

        public async Task<ResponseBase<IEnumerable<Usuario>>> GetUsuario()
        {
            try
            {
                var listaUsuarios = new List<Usuario>();

                _connection.Open();

                if (_connection.State == System.Data.ConnectionState.Open)
                {
                    string sql = $"select * from Usuario";
                    SqlCommand commado = new SqlCommand(sql, _connection);
                    commado.CommandType = System.Data.CommandType.Text;
                    var reader = await commado.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var usuario = new Usuario();
                        usuario.IdUsuario = int.Parse(reader["IdUsuario"].ToString());
                        usuario.NUsuario = reader["Usuario"].ToString();
                        usuario.Contrasenia = reader["Contrasenia"].ToString();
                        usuario.Nombre = reader["Nombre"].ToString();
                        usuario.ApellidoPaterno = reader["ApellidoPaterno"].ToString();
                        usuario.ApellidoMaterno = reader["ApellidoMaterno"].ToString();
                        listaUsuarios.Add(usuario);
                    }

                }

                return new ResponseBase<IEnumerable<Usuario>> { TieneResultado = true, Mensaje = "Lista obtenida correctamente", Modelo = listaUsuarios };

            }
            catch (Exception e)
            {
                return new ResponseBase<IEnumerable<Usuario>> { TieneResultado = false, Mensaje = "Error Interno de Api", Modelo = null };
            }
            finally
            {
                _connection.Close();
            }
        }

        public async Task<ResponseBase<Usuario>> GetUsuarioId(int idusuario)
        {
            var usuario = new Usuario();

            var st_parametro_id = new SqlParameter("@idusuario", System.Data.SqlDbType.Int);
            st_parametro_id.Value = idusuario;

            var st_parametro_error = new SqlParameter("@error", System.Data.SqlDbType.Bit);
            st_parametro_error.Direction = System.Data.ParameterDirection.Output;

            var st_parametro_mensaje = new SqlParameter("@mensaje", System.Data.SqlDbType.NVarChar,500);
            st_parametro_mensaje.Direction = System.Data.ParameterDirection.Output;

            try
            {
               
                _connection.Open();

                if (_connection.State == System.Data.ConnectionState.Open)
                {
                  
                    string sql = $"get_usuario";
                    SqlCommand commado = new SqlCommand(sql, _connection);
                    commado.CommandType = System.Data.CommandType.StoredProcedure;
                    commado.Parameters.Add(st_parametro_id);
                    commado.Parameters.Add(st_parametro_error);
                    commado.Parameters.Add(st_parametro_mensaje);
                    var reader = await commado.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        usuario = new Usuario();
                        usuario.IdUsuario = int.Parse(reader["IdUsuario"].ToString());
                        usuario.NUsuario = reader["Usuario"].ToString();
                        usuario.Contrasenia = reader["Contrasenia"].ToString();
                        usuario.Nombre = reader["Nombre"].ToString();
                        usuario.ApellidoPaterno = reader["ApellidoPaterno"].ToString();
                        usuario.ApellidoMaterno = reader["ApellidoMaterno"].ToString();
                    }

                }
        
            }
            catch (Exception e)
            {
                return new ResponseBase<Usuario> { TieneResultado = (bool) st_parametro_error.Value, Mensaje = "Error Interno de Api", Modelo = null };
            }
            finally
            {
                _connection.Close();
            }

            return new ResponseBase<Usuario> { TieneResultado = (bool)st_parametro_error.Value, Mensaje = st_parametro_mensaje.Value.ToString(), Modelo = usuario };
        }

        public async Task<ResponseBase<int>> UpdateUsuario(Usuario modelo)
        {
            //Variables insert y Update porque son las mismas
            var st_parametro_idrol = new SqlParameter("@idrol", System.Data.SqlDbType.Int);
            st_parametro_idrol.Value = modelo.idrol;

            var st_parametro_usuario = new SqlParameter("@usuario", System.Data.SqlDbType.NVarChar, 500);
            st_parametro_usuario.Value = modelo.NUsuario;

            var st_parametro_pass = new SqlParameter("@contrasenia", System.Data.SqlDbType.NVarChar, 500);
            st_parametro_pass.Value = modelo.Contrasenia;

            var st_parametro_nombre = new SqlParameter("@nombre", System.Data.SqlDbType.NVarChar, 500);
            st_parametro_nombre.Value = modelo.Nombre;

            var st_parametro_apemat = new SqlParameter("@apellidoMaterno", System.Data.SqlDbType.NVarChar, 500);
            st_parametro_apemat.Value = modelo.ApellidoMaterno;

            var st_parametro_apepat = new SqlParameter("@apellidoPaterno", System.Data.SqlDbType.NVarChar, 500);
            st_parametro_apepat.Value = modelo.ApellidoPaterno;

            var st_parametro_idusout = new SqlParameter("@IdUsuarioOut", System.Data.SqlDbType.Int);
            st_parametro_idusout.Direction = System.Data.ParameterDirection.Output;

            var st_parametro_estado = new SqlParameter("@estado", System.Data.SqlDbType.NVarChar, 500);
            st_parametro_estado.Direction = System.Data.ParameterDirection.Output;

            var st_parametro_error = new SqlParameter("@error", System.Data.SqlDbType.Bit);
            st_parametro_error.Direction = System.Data.ParameterDirection.Output;

            try
            {
                _connection.Open();

                if (_connection.State == System.Data.ConnectionState.Open)
                {
                    string sql = $"update_usuario";
                    SqlCommand commado = new SqlCommand(sql, _connection);
                    commado.CommandType = System.Data.CommandType.StoredProcedure;
                    commado.Parameters.Add(st_parametro_idrol);
                    commado.Parameters.Add(st_parametro_usuario);
                    commado.Parameters.Add(st_parametro_pass);
                    commado.Parameters.Add(st_parametro_nombre);
                    commado.Parameters.Add(st_parametro_apemat);
                    commado.Parameters.Add(st_parametro_apepat);
                    commado.Parameters.Add(st_parametro_idusout);
                    commado.Parameters.Add(st_parametro_estado);
                    commado.Parameters.Add(st_parametro_error);
                    await commado.ExecuteNonQueryAsync();
                }


            }
            catch (Exception e)
            {
                return new ResponseBase<int> { TieneResultado = false, Mensaje = "Error Interno de Api", Modelo = 0 };
            }
            finally
            {
                _connection.Close();
            }
            return new ResponseBase<int> { TieneResultado = (bool)st_parametro_error.Value, Mensaje = "Usuario Actualizado Correctamente", Modelo = 1 };

        }

        //Copias
        public async Task<ResponseBase<int>> AddCopia(Copia modelo)
        {
            //Variables insert y Update porque son las mismas
            var st_parametro_idpapel = new SqlParameter("@idpapel", System.Data.SqlDbType.Int);
            st_parametro_idpapel.Value = modelo.IdPapel;

            var st_parametro_cantidadmin = new SqlParameter("@cantidadmin", System.Data.SqlDbType.Int);
            st_parametro_cantidadmin.Value = modelo.CantidadMinima;

            var st_parametro_cantidadmax = new SqlParameter("@cantidadmax", System.Data.SqlDbType.Int);
            st_parametro_cantidadmax.Value = modelo.CantidadMaxima;

            var st_parametro_precio = new SqlParameter("@precio", System.Data.SqlDbType.Money);
            st_parametro_precio.Value = modelo.precio;

            var st_parametro_idusout = new SqlParameter("@IdUsuarioOut", System.Data.SqlDbType.Int);
            st_parametro_idusout.Direction = System.Data.ParameterDirection.Output;

            var st_parametro_mensaje = new SqlParameter("@mensaje", System.Data.SqlDbType.NVarChar, 500);
            st_parametro_mensaje.Direction = System.Data.ParameterDirection.Output;

            var st_parametro_error = new SqlParameter("@error", System.Data.SqlDbType.Bit);
            st_parametro_error.Direction = System.Data.ParameterDirection.Output;

            try
            {
                _connection.Open();

                if (_connection.State == System.Data.ConnectionState.Open)
                {
                    string sql = $"add_copias";
                    SqlCommand commado = new SqlCommand(sql, _connection);
                    commado.CommandType = System.Data.CommandType.StoredProcedure;
                    commado.Parameters.Add(st_parametro_idpapel);
                    commado.Parameters.Add(st_parametro_cantidadmin);
                    commado.Parameters.Add(st_parametro_cantidadmax);
                    commado.Parameters.Add(st_parametro_precio);
                    commado.Parameters.Add(st_parametro_idusout);
                    commado.Parameters.Add(st_parametro_mensaje);
                    commado.Parameters.Add(st_parametro_error);
                    await commado.ExecuteNonQueryAsync();
                }


            }
            catch (Exception e)
            {
                Debug.Write(e);
                return new ResponseBase<int> { TieneResultado = false, Mensaje = "Error Interno de Api", Modelo = 0 };
            }
            finally
            {
                _connection.Close();
            }
            return new ResponseBase<int> { TieneResultado = (bool)st_parametro_error.Value, Mensaje = st_parametro_mensaje.Value.ToString(), Modelo = int.Parse(st_parametro_idusout.Value.ToString()) };

        }
        public async Task<ResponseBase<bool>> DelCopia(int idcopias)
        {
            var st_parametro_idcopias= new SqlParameter("@idcopias", System.Data.SqlDbType.Int);
            st_parametro_idcopias.Value = idcopias;

            var st_parametro_mensaje = new SqlParameter("@mensaje", System.Data.SqlDbType.NVarChar, 500);
            st_parametro_mensaje.Direction = System.Data.ParameterDirection.Output;

            var st_parametro_estado = new SqlParameter("@estado", System.Data.SqlDbType.Bit);
            st_parametro_estado.Direction = System.Data.ParameterDirection.Output;

            try
            {
                _connection.Open();

                if (_connection.State == System.Data.ConnectionState.Open)
                {
                    string sql = $"delete_copias";
                    SqlCommand commado = new SqlCommand(sql, _connection);
                    commado.CommandType = System.Data.CommandType.StoredProcedure;
                    commado.Parameters.Add(st_parametro_idcopias);
                    commado.Parameters.Add(st_parametro_estado);
                    commado.Parameters.Add(st_parametro_mensaje);

                    await commado.ExecuteNonQueryAsync();
                }
            }
            catch (Exception e)
            {
                return new ResponseBase<bool> { TieneResultado = false, Mensaje = "Error en Api ", Modelo = false };
            }
            finally
            {
                _connection.Close();
            }
            return new ResponseBase<bool> { TieneResultado = (bool)st_parametro_estado.Value, Mensaje = st_parametro_mensaje.Value.ToString(), Modelo = (bool)st_parametro_estado.Value };

        }
        public async Task<ResponseBase<IEnumerable<Copia>>> GetCopia()
        {

            try
            {
                var listaCopias= new List<Copia>();

                _connection.Open();

                if (_connection.State == System.Data.ConnectionState.Open)
                {
                    string sql = $"select * from Copias";
                    SqlCommand commado = new SqlCommand(sql, _connection);
                    commado.CommandType = System.Data.CommandType.Text;
                    var reader = await commado.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var copia = new Copia();
                        copia.IdCopias = int.Parse(reader["IdCopias"].ToString());
                        copia.IdPapel = int.Parse(reader["IdPapel"].ToString());
                        copia.CantidadMinima = int.Parse(reader["cantidadMinima"].ToString());
                        copia.CantidadMaxima = int.Parse(reader["cantidadMaxima"].ToString());
                        copia.precio = float.Parse(reader["precio"].ToString());
                        listaCopias.Add(copia);
                    }

                }

                return new ResponseBase<IEnumerable<Copia>> { TieneResultado = true, Mensaje = "Lista obtenida correctamente", Modelo = listaCopias };

            }
            catch (Exception e)
            {
                return new ResponseBase<IEnumerable<Copia>> { TieneResultado = false, Mensaje = "Error Interno de Api", Modelo = null };
            }
            finally
            {
                _connection.Close();
            }
        }
        public async Task<ResponseBase<Copia>> GetCopiaId(int idcopia)
        {
            var copia = new Copia();

            var st_parametro_id = new SqlParameter("@idcopias", System.Data.SqlDbType.Int);
            st_parametro_id.Value = idcopia;

            var st_parametro_error = new SqlParameter("@error", System.Data.SqlDbType.Bit);
            st_parametro_error.Direction = System.Data.ParameterDirection.Output;

            var st_parametro_mensaje = new SqlParameter("@mensaje", System.Data.SqlDbType.NVarChar,500);
            st_parametro_mensaje.Direction = System.Data.ParameterDirection.Output;

            try
            {

                _connection.Open();

                if (_connection.State == System.Data.ConnectionState.Open)
                {

                    string sql = $"get_copias";
                    SqlCommand commado = new SqlCommand(sql, _connection);
                    commado.CommandType = System.Data.CommandType.StoredProcedure;
                    commado.Parameters.Add(st_parametro_id);
                    commado.Parameters.Add(st_parametro_error);
                    commado.Parameters.Add(st_parametro_mensaje);
                    var reader = await commado.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        copia = new Copia();
                        copia.IdCopias = int.Parse(reader["idCopias"].ToString());
                        copia.IdPapel = int.Parse(reader["idPapel"].ToString());
                        copia.CantidadMinima = int.Parse(reader["cantidadMinima"].ToString());
                        copia.CantidadMaxima = int.Parse(reader["cantidadMaxima"].ToString());
                        copia.precio = float.Parse(reader["precio"].ToString());
                    }

                }

            }
            catch (Exception e)
            {
                return new ResponseBase<Copia> { TieneResultado = false, Mensaje = "Error Interno de Api", Modelo = null };
            }
            finally
            {
                _connection.Close();
            }

            return new ResponseBase<Copia> { TieneResultado = (bool)st_parametro_error.Value, Mensaje = st_parametro_mensaje.Value.ToString(), Modelo = copia };
        }
        public async Task<ResponseBase<int>> UpdateCopia(Copia modelo)
        {
            var st_parametro_idcopias = new SqlParameter("@idcopias", System.Data.SqlDbType.Int);
            st_parametro_idcopias.Value = modelo.IdPapel;

            var st_parametro_idpapel = new SqlParameter("@idpapel", System.Data.SqlDbType.Int);
            st_parametro_idpapel.Value = modelo.IdPapel;

            var st_parametro_cantidadmin = new SqlParameter("@cantidadmin", System.Data.SqlDbType.Int);
            st_parametro_cantidadmin.Value = modelo.CantidadMinima;

            var st_parametro_cantidadmax = new SqlParameter("@cantidadmax", System.Data.SqlDbType.Int);
            st_parametro_cantidadmax.Value = modelo.CantidadMaxima;

            var st_parametro_precio = new SqlParameter("@precio", System.Data.SqlDbType.Money);
            st_parametro_precio.Value = modelo.precio;

            var st_parametro_idusout = new SqlParameter("@IdUsuarioOut", System.Data.SqlDbType.Int);
            st_parametro_idusout.Direction = System.Data.ParameterDirection.Output;

            var st_parametro_mensaje = new SqlParameter("@mensaje", System.Data.SqlDbType.NVarChar, 500);
            st_parametro_mensaje.Direction = System.Data.ParameterDirection.Output;

            var st_parametro_error = new SqlParameter("@error", System.Data.SqlDbType.Bit);
            st_parametro_error.Direction = System.Data.ParameterDirection.Output;

            try
            {
                _connection.Open();

                if (_connection.State == System.Data.ConnectionState.Open)
                {
                    string sql = $"update_copias";
                    SqlCommand commado = new SqlCommand(sql, _connection);
                    commado.CommandType = System.Data.CommandType.StoredProcedure;
                    commado.Parameters.Add(st_parametro_idcopias);
                    commado.Parameters.Add(st_parametro_idpapel);
                    commado.Parameters.Add(st_parametro_cantidadmin);
                    commado.Parameters.Add(st_parametro_cantidadmax);
                    commado.Parameters.Add(st_parametro_precio);
                    commado.Parameters.Add(st_parametro_mensaje);
                    commado.Parameters.Add(st_parametro_idusout);
                    commado.Parameters.Add(st_parametro_error);
                    await commado.ExecuteNonQueryAsync();
                }


            }
            catch (Exception e)
            {
                return new ResponseBase<int> { TieneResultado = false, Mensaje = "Error Interno de Api", Modelo = 0 };
            }
            finally
            {
                _connection.Close();
            }
            return new ResponseBase<int> { TieneResultado = (bool)st_parametro_error.Value, Mensaje = st_parametro_mensaje.Value.ToString(), Modelo = 1 };

        }
        
        //Papel
        public async Task<ResponseBase<IEnumerable<Papel>>> GetPapel()
        {
            try
            {
                var listaPapel = new List<Papel>();

                _connection.Open();

                if (_connection.State == System.Data.ConnectionState.Open)
                {
                    string sql = $"select * from Papel";
                    SqlCommand commado = new SqlCommand(sql, _connection);
                    commado.CommandType = System.Data.CommandType.Text;
                    var reader = await commado.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var papel = new Papel();
                        papel.idPapel = int.Parse(reader["IdPapel"].ToString());
                        papel.Tipo_Papel = reader["Tipo_Papel"].ToString();
                        listaPapel.Add(papel);
                    }

                }

                return new ResponseBase<IEnumerable<Papel>> { TieneResultado = true, Mensaje = "Lista obtenida correctamente", Modelo = listaPapel };

            }
            catch (Exception e)
            {
                return new ResponseBase<IEnumerable<Papel>> { TieneResultado = false, Mensaje = "Error Interno de Api", Modelo = null };
            }
            finally
            {
                _connection.Close();
            }
        }

        public async Task<ResponseBase<Papel>> GetPapelId(int idpapel)
        {
            var papel = new Papel();

            var st_parametro_id = new SqlParameter("@idpapel", System.Data.SqlDbType.Int);
            st_parametro_id.Value = idpapel;

            var st_parametro_error = new SqlParameter("@error", System.Data.SqlDbType.Bit);
            st_parametro_error.Direction = System.Data.ParameterDirection.Output;

            var st_parametro_mensaje = new SqlParameter("@mensaje", System.Data.SqlDbType.NVarChar,500);
            st_parametro_mensaje.Direction = System.Data.ParameterDirection.Output;

            try
            {

                _connection.Open();

                if (_connection.State == System.Data.ConnectionState.Open)
                {

                    string sql = $"get_papel";
                    SqlCommand commado = new SqlCommand(sql, _connection);
                    commado.CommandType = System.Data.CommandType.StoredProcedure;
                    commado.Parameters.Add(st_parametro_id);
                    commado.Parameters.Add(st_parametro_mensaje);
                    commado.Parameters.Add(st_parametro_error);
                    var reader = await commado.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        papel = new Papel();
                        papel.idPapel = int.Parse(reader["idpapel"].ToString());
                        papel.Tipo_Papel = reader["Tipo_papel"].ToString();
                    }

                }

            }
            catch (Exception e)
            {
                return new ResponseBase<Papel> { TieneResultado = false, Mensaje = "Error Interno de Api", Modelo = null };
            }
            finally
            {
                _connection.Close();
            }

            return new ResponseBase<Papel> { TieneResultado = (bool)st_parametro_error.Value, Mensaje = st_parametro_mensaje.Value.ToString(), Modelo = papel };
        }

        public async Task<ResponseBase<int>> AddPapel(Papel modelo)
        {
            //Variables insert y Update porque son las mismas
            var st_parametro_tipopapel = new SqlParameter("@tipoPapel", System.Data.SqlDbType.NVarChar,500);
            st_parametro_tipopapel.Value = modelo.Tipo_Papel;

            var st_parametro_error = new SqlParameter("@error", System.Data.SqlDbType.Bit);
            st_parametro_error.Direction = System.Data.ParameterDirection.Output;

            var st_parametro_estado = new SqlParameter("@estado", System.Data.SqlDbType.NVarChar,500);
            st_parametro_estado.Direction = System.Data.ParameterDirection.Output;

            var st_parametro_idusout = new SqlParameter("@IdUsuarioOut", System.Data.SqlDbType.Int);
            st_parametro_idusout.Direction = System.Data.ParameterDirection.Output;

            try
            {
                _connection.Open();

                if (_connection.State == System.Data.ConnectionState.Open)
                {
                    string sql = $"add_papel";
                    SqlCommand commado = new SqlCommand(sql, _connection);
                    commado.CommandType = System.Data.CommandType.StoredProcedure;
                    commado.Parameters.Add(st_parametro_tipopapel);
                    commado.Parameters.Add(st_parametro_estado);
                    commado.Parameters.Add(st_parametro_idusout);
                    commado.Parameters.Add(st_parametro_error);
                    await commado.ExecuteNonQueryAsync();
                }


            }
            catch (Exception e)
            {
                return new ResponseBase<int> { TieneResultado = false, Mensaje = "Error Interno de Api", Modelo = 0 };
            }
            finally
            {
                _connection.Close();
            }
            return new ResponseBase<int> { TieneResultado = (bool)st_parametro_error.Value, Mensaje = st_parametro_estado.Value.ToString(), Modelo = int.Parse(st_parametro_idusout.Value.ToString()) };

        }

        public async Task<ResponseBase<int>> UpdatePapel(Papel modelo)
        {
            //Variables insert y Update porque son las mismas
            var st_parametro_idpapel = new SqlParameter("@idpapel", System.Data.SqlDbType.Int);
            st_parametro_idpapel.Value = modelo.idPapel;

            var st_parametro_tipopapel = new SqlParameter("@tipoPapel", System.Data.SqlDbType.NVarChar, 500);
            st_parametro_tipopapel.Value = modelo.Tipo_Papel;

            var st_parametro_error = new SqlParameter("@error", System.Data.SqlDbType.Bit);
            st_parametro_error.Direction = System.Data.ParameterDirection.Output;

            var st_parametro_estado = new SqlParameter("@estado", System.Data.SqlDbType.NVarChar, 500);
            st_parametro_estado.Direction = System.Data.ParameterDirection.Output;

            var st_parametro_idusout = new SqlParameter("@IdUsuarioOut", System.Data.SqlDbType.Int);
            st_parametro_idusout.Direction = System.Data.ParameterDirection.Output;

            try
            {
                _connection.Open();

                if (_connection.State == System.Data.ConnectionState.Open)
                {
                    string sql = $"update_edit_papel";
                    SqlCommand commado = new SqlCommand(sql, _connection);
                    commado.CommandType = System.Data.CommandType.StoredProcedure;
                    commado.Parameters.Add(st_parametro_idpapel);
                    commado.Parameters.Add(st_parametro_tipopapel);
                    commado.Parameters.Add(st_parametro_estado);
                    commado.Parameters.Add(st_parametro_idusout);
                    commado.Parameters.Add(st_parametro_error);
                    await commado.ExecuteNonQueryAsync();
                }


            }
            catch (Exception e)
            {
                return new ResponseBase<int> { TieneResultado = false, Mensaje = "Error Interno de Api", Modelo = 0 };
            }
            finally
            {
                _connection.Close();
            }
            return new ResponseBase<int> { TieneResultado = (bool)st_parametro_error.Value, Mensaje = st_parametro_estado.Value.ToString(), Modelo =1 };

        }

        public async Task<ResponseBase<bool>> DelPapel(int id)
        {
            var st_parametro_idpapel = new SqlParameter("@idpapel", System.Data.SqlDbType.Int);
            st_parametro_idpapel.Value = id;

            var st_parametro_error = new SqlParameter("@error", System.Data.SqlDbType.Bit);
            st_parametro_error.Direction = System.Data.ParameterDirection.Output;

            var st_parametro_estado = new SqlParameter("@estado", System.Data.SqlDbType.NVarChar,500);
            st_parametro_estado.Direction = System.Data.ParameterDirection.Output;

            try
            {
                _connection.Open();

                if (_connection.State == System.Data.ConnectionState.Open)
                {
                    string sql = $"delete_papel";
                    SqlCommand commado = new SqlCommand(sql, _connection);
                    commado.CommandType = System.Data.CommandType.StoredProcedure;
                    commado.Parameters.Add(st_parametro_idpapel);
                    commado.Parameters.Add(st_parametro_error);
                    commado.Parameters.Add(st_parametro_estado);

                    await commado.ExecuteNonQueryAsync();
                }
            }
            catch (Exception e)
            {
                return new ResponseBase<bool> { TieneResultado = false, Mensaje = "Error en la base de datos ", Modelo = false };
            }
            finally
            {
                _connection.Close();
            }
            return new ResponseBase<bool> { TieneResultado = (bool)st_parametro_error.Value, Mensaje = st_parametro_estado.Value.ToString(), Modelo = (bool)st_parametro_error.Value };

        }

        //Productos
        public async Task<ResponseBase<IEnumerable<Producto>>> GetProductos()
        {
            try
            {
                var listaProductos = new List<Producto>();

                _connection.Open();

                if (_connection.State == System.Data.ConnectionState.Open)
                {
                    string sql = $"select * from Productos";
                    SqlCommand commado = new SqlCommand(sql, _connection);
                    commado.CommandType = System.Data.CommandType.Text;
                    var reader = await commado.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var producto = new Producto();
                        producto.idProducto = int.Parse(reader["idProducto"].ToString());
                        producto.ProductoN = reader["Producto"].ToString();
                        producto.cantidad = int.Parse(reader["cantidad"].ToString());
                        producto.costo_unitario = float.Parse(reader["costo_unitario"].ToString());
                        producto.costo_mayoreo = float.Parse(reader["costo_mayoreo"].ToString());
                        listaProductos.Add(producto);
                    }

                }

                return new ResponseBase<IEnumerable<Producto>> { TieneResultado = true, Mensaje = "Lista obtenida correctamente", Modelo = listaProductos };

            }
            catch (Exception e)
            {
                return new ResponseBase<IEnumerable<Producto>> { TieneResultado = false, Mensaje = "Error Interno de Api", Modelo = null };
            }
            finally
            {
                _connection.Close();
            }
        }

        public async Task<ResponseBase<Producto>> GetProductosId(int idproducto)
        {
            var producto = new Producto();

            var st_parametro_id = new SqlParameter("@idproducto", System.Data.SqlDbType.Int);
            st_parametro_id.Value = idproducto;

            var st_parametro_error = new SqlParameter("@error", System.Data.SqlDbType.Bit);
            st_parametro_error.Direction = System.Data.ParameterDirection.Output;

            var st_parametro_mensaje = new SqlParameter("@mensaje", System.Data.SqlDbType.NVarChar,500);
            st_parametro_mensaje.Direction = System.Data.ParameterDirection.Output;


            try
            {

                _connection.Open();

                if (_connection.State == System.Data.ConnectionState.Open)
                {

                    string sql = $"get_productos";
                    SqlCommand commado = new SqlCommand(sql, _connection);
                    commado.CommandType = System.Data.CommandType.StoredProcedure;
                    commado.Parameters.Add(st_parametro_id);
                    commado.Parameters.Add(st_parametro_error);
                    commado.Parameters.Add(st_parametro_mensaje);
                    var reader = await commado.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        producto = new Producto();
                        producto.idProducto = int.Parse(reader["idProducto"].ToString());
                        producto.ProductoN = reader["Producto"].ToString();
                        producto.cantidad = int.Parse(reader["cantidad"].ToString());
                        producto.costo_unitario = float.Parse(reader["costo_unitario"].ToString());
                        producto.costo_mayoreo = float.Parse(reader["costo_mayoreo"].ToString());
                    }

                }

            }
            catch (Exception e)
            {
                return new ResponseBase<Producto> { TieneResultado = false, Mensaje = "Error Interno de Api", Modelo = null };
            }
            finally
            {
                _connection.Close();
            }

            return new ResponseBase<Producto> { TieneResultado = (bool)st_parametro_error.Value, Mensaje = st_parametro_mensaje.Value.ToString(), Modelo = producto };
       
        }

        public async Task<ResponseBase<int>> AddProducto(Producto modelo)
        {
            //Variables insert y Update porque son las mismas
            var st_parametro_producto = new SqlParameter("@producto", System.Data.SqlDbType.NVarChar, 500);
            st_parametro_producto.Value = modelo.ProductoN;

            var st_parametro_cantidad = new SqlParameter("@cantidad", System.Data.SqlDbType.Int);
            st_parametro_cantidad.Value = modelo.cantidad;

            var st_parametro_costouni = new SqlParameter("@costo_unitario", System.Data.SqlDbType.Money);
            st_parametro_costouni.Value = modelo.costo_unitario;

            var st_parametro_costomay = new SqlParameter("@costo_mayoreo", System.Data.SqlDbType.Money);
            st_parametro_costomay.Value = modelo.costo_mayoreo;

            var st_parametro_error = new SqlParameter("@error", System.Data.SqlDbType.Bit);
            st_parametro_error.Direction = System.Data.ParameterDirection.Output;

            var st_parametro_idusout = new SqlParameter("@IdUsuarioOut", System.Data.SqlDbType.Int);
            st_parametro_idusout.Direction = System.Data.ParameterDirection.Output;

            var st_parametro_estado = new SqlParameter("@estado", System.Data.SqlDbType.NVarChar,500);
            st_parametro_estado.Direction = System.Data.ParameterDirection.Output;

            try
            {
                _connection.Open();

                if (_connection.State == System.Data.ConnectionState.Open)
                {
                    string sql = $"add_product";
                    SqlCommand commado = new SqlCommand(sql, _connection);
                    commado.CommandType = System.Data.CommandType.StoredProcedure;
                    commado.Parameters.Add(st_parametro_producto);
                    commado.Parameters.Add(st_parametro_cantidad);
                    commado.Parameters.Add(st_parametro_costouni);
                    commado.Parameters.Add(st_parametro_costomay);
                    commado.Parameters.Add(st_parametro_estado);
                    commado.Parameters.Add(st_parametro_idusout);
                    commado.Parameters.Add(st_parametro_error);
                    await commado.ExecuteNonQueryAsync();
                }


            }
            catch (Exception e)
            {
                return new ResponseBase<int> { TieneResultado = false, Mensaje = "Error Interno de Api", Modelo = 0 };
            }
            finally
            {
                _connection.Close();
            }
            return new ResponseBase<int> { TieneResultado = (bool)st_parametro_error.Value, Mensaje = st_parametro_estado.Value.ToString(), Modelo = 1 };

        }

        public async Task<ResponseBase<int>> UpdateProducto(Producto modelo)
        {
            //Variables insert y Update porque son las mismas

            var st_parametro_producto = new SqlParameter("@producto", System.Data.SqlDbType.NVarChar, 500);
            st_parametro_producto.Value = modelo.ProductoN;

            var st_parametro_cantidad = new SqlParameter("@cantidad", System.Data.SqlDbType.Int);
            st_parametro_cantidad.Value = modelo.cantidad;

            var st_parametro_costouni = new SqlParameter("@costo_unitario", System.Data.SqlDbType.Money);
            st_parametro_costouni.Value = modelo.costo_unitario;

            var st_parametro_costomay = new SqlParameter("@costo_mayoreo", System.Data.SqlDbType.Money);
            st_parametro_costomay.Value = modelo.costo_mayoreo;

            var st_parametro_error = new SqlParameter("@error", System.Data.SqlDbType.Bit);
            st_parametro_error.Direction = System.Data.ParameterDirection.Output;

            var st_parametro_idusout = new SqlParameter("@IdUsuarioOut", System.Data.SqlDbType.Int);
            st_parametro_idusout.Direction = System.Data.ParameterDirection.Output;

            var st_parametro_estado = new SqlParameter("@estado", System.Data.SqlDbType.NVarChar, 500);
            st_parametro_estado.Direction = System.Data.ParameterDirection.Output;

            try
            {
                _connection.Open();

                if (_connection.State == System.Data.ConnectionState.Open)
                {
                    string sql = $"update_products";
                    SqlCommand commado = new SqlCommand(sql, _connection);
                    commado.CommandType = System.Data.CommandType.StoredProcedure;
                    commado.Parameters.Add(st_parametro_producto);
                    commado.Parameters.Add(st_parametro_cantidad);
                    commado.Parameters.Add(st_parametro_costouni);
                    commado.Parameters.Add(st_parametro_costomay);
                    commado.Parameters.Add(st_parametro_estado);
                    commado.Parameters.Add(st_parametro_idusout);
                    commado.Parameters.Add(st_parametro_error);
                    await commado.ExecuteNonQueryAsync();
                }


            }
            catch (Exception e)
            {
                return new ResponseBase<int> { TieneResultado = false, Mensaje = "Error Interno de Api", Modelo = 0 };
            }
            finally
            {
                _connection.Close();
            }
            return new ResponseBase<int> { TieneResultado = (bool)st_parametro_error.Value, Mensaje = st_parametro_estado.Value.ToString(), Modelo = 1 };

        }

        public async Task<ResponseBase<bool>> DelProducto(int id)
        {
            var st_parametro_idproducto= new SqlParameter("@idproducto", System.Data.SqlDbType.Int);
            st_parametro_idproducto.Value = id;

            var st_parametro_error = new SqlParameter("@error", System.Data.SqlDbType.Bit);
            st_parametro_error.Direction = System.Data.ParameterDirection.Output;

            var st_parametro_estado = new SqlParameter("@estado", System.Data.SqlDbType.NVarChar, 500);
            st_parametro_estado.Direction = System.Data.ParameterDirection.Output;

            try
            {
                _connection.Open();

                if (_connection.State == System.Data.ConnectionState.Open)
                {
                    string sql = $"delete_product";
                    SqlCommand commado = new SqlCommand(sql, _connection);
                    commado.CommandType = System.Data.CommandType.StoredProcedure;
                    commado.Parameters.Add(st_parametro_idproducto);
                    commado.Parameters.Add(st_parametro_error);
                    commado.Parameters.Add(st_parametro_estado);

                    await commado.ExecuteNonQueryAsync();
                }
            }
            catch (Exception e)
            {
                return new ResponseBase<bool> { TieneResultado = false, Mensaje = "Error en la base de datos ", Modelo = false };
            }
            finally
            {
                _connection.Close();
            }
            return new ResponseBase<bool> { TieneResultado = (bool)st_parametro_error.Value, Mensaje = st_parametro_estado.Value.ToString(), Modelo = (bool)st_parametro_error.Value };

        }
        //Login
        public async Task<ResponseBase<Login>> IniciarSesion(Login modelo)
        {
            var usuario_log = new Login();

            var st_parametro_usuario = new SqlParameter("@usuario", System.Data.SqlDbType.NVarChar,500);
            st_parametro_usuario.Value = modelo.usuario;

            var st_parametro_contrasenia = new SqlParameter("@contrasenia", System.Data.SqlDbType.NVarChar,500);
            st_parametro_contrasenia.Value = modelo.contrasenia;

            var st_parametro_mensaje = new SqlParameter("@mensaje", System.Data.SqlDbType.NVarChar, 500);
            st_parametro_mensaje.Direction = System.Data.ParameterDirection.Output;

            var st_parametro_error = new SqlParameter("@error", System.Data.SqlDbType.Bit);
            st_parametro_error.Direction = System.Data.ParameterDirection.Output;

            var st_parametro_idout = new SqlParameter("@IdUsuarioOut", System.Data.SqlDbType.Int);
            st_parametro_idout.Direction = System.Data.ParameterDirection.Output;

            try
            {
                _connection.Open();

                if (_connection.State == System.Data.ConnectionState.Open)
                {
                    string sql = $"PS_Login";
                    SqlCommand command = new SqlCommand(sql, _connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(st_parametro_usuario);
                    command.Parameters.Add(st_parametro_contrasenia);
                    command.Parameters.Add(st_parametro_mensaje);
                    command.Parameters.Add(st_parametro_error);
                    command.Parameters.Add(st_parametro_idout);
                    var reader = await command.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        usuario_log = new Login();
                        usuario_log.idrol = int.Parse(reader["idrol"].ToString());
                        usuario_log.idUsuario = int.Parse(reader["idusuario"].ToString());
                        usuario_log.nombre = reader["nombre"].ToString();
                        usuario_log.contrasenia = reader["contrasenia"].ToString();
                        usuario_log.usuario = reader["usuario"].ToString();
                        usuario_log.ApellidoPaterno = reader["apellidopaterno"].ToString();
                        usuario_log.ApellidoMaterno = reader["apellidomaterno"].ToString();
                        usuario_log.idMenu = int.Parse(reader["idMenu"].ToString());
                        usuario_log.Menu = reader["Menu"].ToString();
                        usuario_log.Ruta = reader["Ruta"].ToString();

                    }
                }
            }
            catch(Exception e)
            {
                return new ResponseBase<Login> { TieneResultado = (bool)st_parametro_error.Value, Mensaje = "Error Interno de Api", Modelo = null };
            }
            finally
            {
                _connection.Close();
            }
            return new ResponseBase<Login> { TieneResultado = (bool)st_parametro_error.Value, Mensaje = st_parametro_mensaje.Value.ToString(), Modelo = usuario_log };
        }

        public async Task<ResponseBase<int>> AddTicket(ListaDetalle modelo)
        {
            //TicketDetalle
            var st_parametro_producto = new SqlParameter("@idProducto", System.Data.SqlDbType.Int);

            var st_parametro_idcopia = new SqlParameter("@IdCopia", System.Data.SqlDbType.Int);

            var st_parametro_idticketdetalle = new SqlParameter("@idTicket", System.Data.SqlDbType.Int);

            var st_parametro_cantidad= new SqlParameter("@Cantidad", System.Data.SqlDbType.Int);

            var st_parametro_totalticket = new SqlParameter("@totalProducto", System.Data.SqlDbType.Int);

            var st_parametro_error_ticket = new SqlParameter("@error", System.Data.SqlDbType.Bit);
            st_parametro_error_ticket.Direction = System.Data.ParameterDirection.Output;

            var st_parametro_mensaje = new SqlParameter("@mensaje", System.Data.SqlDbType.NVarChar,500);
            st_parametro_mensaje.Direction = System.Data.ParameterDirection.Output;

            float total = 0;
            for(int i = 0;i<modelo.productos.Count();i++)
            {
                total += modelo.productos[i].totalProducto;
            }
            var st_parametro_usuario = new SqlParameter("@usuario", System.Data.SqlDbType.NVarChar, 500);
            st_parametro_usuario.Value = modelo.usuario;

            var st_parametro_total = new SqlParameter("@total", System.Data.SqlDbType.Money);
            st_parametro_total.Value = total;

            var st_parametro_error = new SqlParameter("@error", System.Data.SqlDbType.Bit);
            st_parametro_error.Direction = System.Data.ParameterDirection.Output;

            var st_parametro_idusout = new SqlParameter("@IdOutUsuario", System.Data.SqlDbType.Int);
            st_parametro_idusout.Direction = System.Data.ParameterDirection.Output;

            var st_parametro_estado = new SqlParameter("@mensaje", System.Data.SqlDbType.NVarChar, 500);
            st_parametro_estado.Direction = System.Data.ParameterDirection.Output;
         
            try
            {
                _connection.Open();

                if (_connection.State == System.Data.ConnectionState.Open)
                {
                    string sql = $"ST_Ticket";
                    SqlCommand commado = new SqlCommand(sql, _connection);
                    commado.CommandType = System.Data.CommandType.StoredProcedure;
                    commado.Parameters.Add(st_parametro_usuario);
                    commado.Parameters.Add(st_parametro_total);
                    commado.Parameters.Add(st_parametro_estado);
                    commado.Parameters.Add(st_parametro_idusout);
                    commado.Parameters.Add(st_parametro_error);
                    await commado.ExecuteNonQueryAsync();

                    st_parametro_idticketdetalle.Value = int.Parse(st_parametro_idusout.Value.ToString());
                    string sql_ticket = $"Ticket_Detalle";
                    SqlCommand commado_ticket = new SqlCommand(sql_ticket, _connection);
                    commado_ticket.CommandType = System.Data.CommandType.StoredProcedure;
                    commado_ticket.Parameters.Add(st_parametro_producto);
                    commado_ticket.Parameters.Add(st_parametro_idticketdetalle);
                    commado_ticket.Parameters.Add(st_parametro_idcopia);
                    commado_ticket.Parameters.Add(st_parametro_cantidad);
                    commado_ticket.Parameters.Add(st_parametro_totalticket);
                    commado_ticket.Parameters.Add(st_parametro_mensaje);
                    commado_ticket.Parameters.Add(st_parametro_error_ticket);
                    for (int i = 0; i < modelo.productos.Count(); i++)
                    {
                        st_parametro_producto.Value = modelo.productos[i].IdProducto;
                        st_parametro_idcopia.Value = modelo.productos[i].IdCopia;
                        st_parametro_cantidad.Value = modelo.productos[i].Cantidad;
                        st_parametro_totalticket.Value = modelo.productos[i].totalProducto;
                        await commado_ticket.ExecuteNonQueryAsync();


                    }
                }


            }
            catch (Exception e)
            {
                return new ResponseBase<int> { TieneResultado = false, Mensaje = "Error Interno de Api", Modelo = 0 };
            }
            finally
            {
                _connection.Close();
            }
            return new ResponseBase<int> { TieneResultado = (bool)st_parametro_error.Value, Mensaje = st_parametro_estado.Value.ToString(), Modelo = int.Parse(st_parametro_idusout.Value.ToString()) };


        }

        public Task<ResponseBase<TicketDetalle>> GetTicketDetalle(List<TicketDetalle> modelo)
        {
            throw new NotImplementedException();
        }
        //Quitar Productos
        public async Task<ResponseBase<int>> QuitarProductos(int idTicket)
        {
            //[getproductoticket]
            var st_parametro_idticket = new SqlParameter("@idticket", System.Data.SqlDbType.Int);
            st_parametro_idticket.Value = idTicket;

            var st_parametro_error = new SqlParameter("@error", System.Data.SqlDbType.Bit);
            st_parametro_error.Direction = System.Data.ParameterDirection.Output;

            var st_parametro_mensaje = new SqlParameter("@mensaje", System.Data.SqlDbType.NVarChar, 500);
            st_parametro_mensaje.Direction = System.Data.ParameterDirection.Output;

            var st_parametro_idout = new SqlParameter("@idout", System.Data.SqlDbType.NVarChar, -1);
            st_parametro_idout.Direction = System.Data.ParameterDirection.Output;

            //[quitar_cantidad]

            var st_parametro_idproducto = new SqlParameter("@idproducto", System.Data.SqlDbType.Int);

            var st_parametro_errorquitar = new SqlParameter("@error", System.Data.SqlDbType.Bit);
            st_parametro_errorquitar.Direction = System.Data.ParameterDirection.Output;

            var st_parametro_mensajequitar = new SqlParameter("@mensaje", System.Data.SqlDbType.NVarChar, 500);
            st_parametro_mensajequitar.Direction = System.Data.ParameterDirection.Output;

            try
            {

                _connection.Open();
                
                if (_connection.State == System.Data.ConnectionState.Open)
                {

                    string sql = $"getproductoticket";
                    SqlCommand commado = new SqlCommand(sql, _connection);
                    commado.CommandType = System.Data.CommandType.StoredProcedure;
                    commado.Parameters.Add(st_parametro_idticket);
                    commado.Parameters.Add(st_parametro_mensaje);
                    commado.Parameters.Add(st_parametro_error);
                    commado.Parameters.Add(st_parametro_idout);
                    await commado.ExecuteNonQueryAsync();
                    //var productos = Newtonsoft.Json.JsonConvert.DeserializeObject<ListaProducto>(st_parametro_idout.Value.ToString());



                    var id_productos = st_parametro_idout.SqlValue.ToString();
                    List<int> productos_lista = new List<int>();
                    int temp = 0;
                    for (int i = 0; i < id_productos.Length; i++)
                    {
                        if (Char.IsDigit(id_productos[i]))
                        {
                            temp = int.Parse(id_productos[i].ToString());
                            productos_lista.Add(temp);
                        }
                    }
                    string sql_quit = $"quitar_cantidad";
                    SqlCommand commado_quit = new SqlCommand(sql_quit, _connection);
                    commado_quit.CommandType = System.Data.CommandType.StoredProcedure;
                    commado_quit.Parameters.Add(st_parametro_idproducto);
                    commado_quit.Parameters.Add(st_parametro_mensajequitar);
                    commado_quit.Parameters.Add(st_parametro_errorquitar);
                    for (int i = 0; i <productos_lista.Count(); i++)
                    {
                        st_parametro_idproducto.Value = productos_lista[i];
                        await commado_quit.ExecuteNonQueryAsync();
                    }
                }

            }
            catch (Exception e)
            {
                return new ResponseBase<int> { TieneResultado = false, Mensaje = "Error Interno de Api", Modelo = 0 };
            }
            finally
            {
                _connection.Close();
            }

            return new ResponseBase<int> { TieneResultado = (bool)st_parametro_error.Value, Mensaje = st_parametro_mensaje.Value.ToString(), Modelo = 1 };

        }

        public async Task<ResponseBase<IEnumerable<Ticket>>> GetReporte()
        {

            var ticket = new Ticket();
            var lista_ticket = new List<Ticket>();
            var st_parametro_error = new SqlParameter("@error", System.Data.SqlDbType.Bit);
            st_parametro_error.Direction = System.Data.ParameterDirection.Output;

            var st_parametro_mensaje = new SqlParameter("@mensaje", System.Data.SqlDbType.NVarChar, 500);
            st_parametro_mensaje.Direction = System.Data.ParameterDirection.Output;
            try
            {
                
                _connection.Open();

                if (_connection.State == System.Data.ConnectionState.Open)
                {

                    string sql = $"reporte";
                    SqlCommand commado = new SqlCommand(sql, _connection);
                    commado.CommandType = System.Data.CommandType.StoredProcedure;
                    commado.Parameters.Add(st_parametro_error);
                    commado.Parameters.Add(st_parametro_mensaje);
                    var reader = await commado.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        ticket = new Ticket();
                        ticket.IdTicket = int.Parse(reader["IdTicket"].ToString());
                        ticket.FechaCompra = reader["FechaCompra"].ToString();
                        ticket.Total = float.Parse(reader["Total"].ToString());
                        ticket.idUsuario = int.Parse(reader["Idusuario"].ToString());
                        lista_ticket.Add(ticket);
                    }

                }

            }
            catch (Exception e)
            {
                return new ResponseBase<IEnumerable<Ticket>> { TieneResultado = false, Mensaje = "Error Interno de Api", Modelo = null };
            }
            finally
            {
                _connection.Close();
            }

            return new ResponseBase<IEnumerable<Ticket>> { TieneResultado = (bool)st_parametro_error.Value, Mensaje = st_parametro_mensaje.Value.ToString(), Modelo = lista_ticket };

        }


    }
}
