using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Librería para establecer conexión con DB
using System.Data.SqlClient;

using dominioD;

namespace negocioD
{
    public class DiscosNegocio
    {
        public List<Discos> listar() 
        {
            List<Discos> lista = new List<Discos>();

            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();
            SqlDataReader lector;

            try
            {
                conexion.ConnectionString = "server=. \\SQLEXPRESS; database=DISCOS_DB; integrated security=true";

                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = "select D.Titulo, D.FechaLanzamiento, D.CantidadCanciones, D.UrlImagenTapa, E.Descripcion as Estilo, T.Descripcion as TipoEdicion, D.IdEstilo, D.IdTipoEdicion, D.Id from DISCOS D, ESTILOS E, TIPOSEDICION T where E.Id = D.IdEstilo And T.Id = D.IdTipoEdicion\r\n";
                comando.Connection = conexion;

                conexion.Open();
                lector = comando.ExecuteReader();

                while(lector.Read())
                {
                    Discos aux = new Discos();

                    aux.Id = (int)lector["Id"];
                    aux.Titulo = (string)lector["Titulo"];
                    aux.FechaLanzamiento = (DateTime)lector["FechaLanzamiento"];
                    aux.CantidadCanciones = (int)lector["CantidadCanciones"];


                    if (!(lector["UrlImagenTapa"] is DBNull))
                    {
                        aux.UrlImagenTapa = (string)lector["UrlImagenTapa"];
                    }

                    aux.IdEstilo = new Estilos();
                    aux.IdEstilo.Id = (int)lector["IdEstilo"];
                    aux.IdEstilo.Descripcion = (string)lector["Estilo"];

                    aux.IdTipoEdicion = new TiposEdicion();
                    aux.IdTipoEdicion.Id = (int)lector["IdTipoEdicion"];
                    aux.IdTipoEdicion.Descripcion = (string)lector["TipoEdicion"];

                    lista.Add(aux);

                }

                conexion.Close();

                return lista;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void agregar(Discos nuevo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("insert into DISCOS (Titulo, FechaLanzamiento, CantidadCanciones, UrlImagenTapa, IdEstilo, IdTipoEdicion) values (@Titulo, @FechaLanzamiento, @CantidadCanciones, @UrlImagenTapa, @IdEstilo, @IdTipoEdicion)");

                datos.setearParametro("@Titulo", nuevo.Titulo);
                datos.setearParametro("@FechaLanzamiento", nuevo.FechaLanzamiento);
                datos.setearParametro("@CantidadCanciones", nuevo.CantidadCanciones);
                datos.setearParametro("@UrlImagenTapa", nuevo.UrlImagenTapa);
                datos.setearParametro("@IdEstilo", nuevo.IdEstilo.Id);
                datos.setearParametro("@IdTipoEdicion", nuevo.IdTipoEdicion.Id);

                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void modificar (Discos disco)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("update DISCOS set Titulo = @titulo, FechaLanzamiento = @fechaLanzamiento, CantidadCanciones = @cantidadCanciones, UrlImagenTapa = @imagen, IdEstilo = @idEstilo, IdTipoEdicion = @idTipoEdicion where Id = @id");

                datos.setearParametro("@titulo", disco.Titulo);
                datos.setearParametro("@fechaLanzamiento", disco.FechaLanzamiento);
                datos.setearParametro("@cantidadCanciones", disco.CantidadCanciones);
                datos.setearParametro("@imagen", disco.UrlImagenTapa);
                datos.setearParametro("@idEstilo", disco.IdEstilo.Id);
                datos.setearParametro("@idTipoEdicion", disco.IdTipoEdicion.Id);
                datos.setearParametro("@id", disco.Id);

                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void eliminar (int id)
        {
            try
            {
                AccesoDatos datos = new AccesoDatos();

                datos.setearConsulta("delete from DISCOS where id = @id");

                datos.setearParametro("@id", id);

                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<Discos> filtrar(string campo, string criterio, string filtro)
        {
            List<Discos> lista = new List<Discos>();

            AccesoDatos datos = new AccesoDatos();

            try
            {

                string consulta = "select D.Titulo, D.FechaLanzamiento, D.CantidadCanciones, D.UrlImagenTapa, E.Descripcion as Estilo, T.Descripcion as TipoEdicion, D.IdEstilo, D.IdTipoEdicion, D.Id from DISCOS D, ESTILOS E, TIPOSEDICION T where E.Id = D.IdEstilo And T.Id = D.IdTipoEdicion And ";

                if(campo == "CantidadCanciones")
                {
                    switch (criterio)
                    {
                        case "Mayor a":
                            consulta += "D.CantidadCanciones > " + filtro;
                            break;

                        case "Menor a":
                            consulta += "D.CantidadCanciones < " + filtro;
                            break;

                        default:
                            consulta += "D.CantidadCanciones = " + filtro;
                            break;
                    }
                }

                else if(campo == "Titulo")
                {
                    switch(criterio)
                    {
                        case "Comienza con":
                            consulta += "D.Titulo like '" + filtro + "%' ";
                            break;

                        case "Termina con":
                            consulta += "D.Titulo like '%" + filtro + "'";
                            break;

                        default:
                            consulta += "D.Titulo like '%" + filtro + "%'";
                            break;
                    }
                }

                else
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "E.Descripcion like '" + filtro + "%' ";
                            break;

                        case "Termina con":
                            consulta += "E.Descripcion like '%" + filtro + "'";
                            break;

                        default:
                            consulta += "E.Descripcion like '%" + filtro + "%'";
                            break;
                    }
                }

                datos.setearConsulta(consulta);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Discos aux = new Discos();

                    aux.Id = (int)datos.Lector["Id"];
                    aux.Titulo = (string)datos.Lector["Titulo"];
                    aux.FechaLanzamiento = (DateTime)datos.Lector["FechaLanzamiento"];
                    aux.CantidadCanciones = (int)datos.Lector["CantidadCanciones"];

                    if (!(datos.Lector["UrlImagenTapa"] is DBNull))
                    {
                        aux.UrlImagenTapa = (string)datos.Lector["UrlImagenTapa"];
                    }

                    aux.IdEstilo = new Estilos();
                    aux.IdEstilo.Id = (int)datos.Lector["IdEstilo"];
                    aux.IdEstilo.Descripcion = (string)datos.Lector["Estilo"];

                    aux.IdTipoEdicion = new TiposEdicion();
                    aux.IdTipoEdicion.Id = (int)datos.Lector["IdTipoEdicion"];
                    aux.IdTipoEdicion.Descripcion = (string)datos.Lector["TipoEdicion"];

                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
