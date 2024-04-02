using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Dominio;
using System.Collections;


namespace Negocio

{
    public class PokemonNegocio
    {
        public List<Pokemon> listar()    //se crea un metodo para listar una lista de objetos del tipo Pokemon
        {
            List<Pokemon> lista = new List<Pokemon>();
            AccesoDatos datos = new AccesoDatos();
            // metodo sin centralizar la conewxion en otra clase Acceso A datos  :
            // SqlConnection conexion = new SqlConnection();
            // SqlCommand comando = new SqlCommand();
            // SqlDataReader lector;

            try
            {
                // metodo manual:
                //conexion.ConnectionString = "server=.\\SQLEXPRESS; database=POKEDEX_DB; integrated security =true";
                //comando.CommandType = System.Data.CommandType.Text; // se define q tipo de comando se va a realizar
                //comando.CommandText = "Select Numero, Nombre, P.Descripcion, UrlImagen, E.Descripcion Tipo, D.Descripcion Debilidad From POKEMONS P, ELEMENTOS E, ELEMENTOS D Where E.Id = P.IdTipo And D.Id = P.IdDebilidad"; //realizar consulta SQL a DB
                //comando.Connection = conexion;
                //conexion.Open();
                //lector = comando.ExecuteReader();
                datos.setearConsulta("Select Numero, Nombre, P.Descripcion, UrlImagen, E.Descripcion Tipo, D.Descripcion Debilidad, P.IdTipo, P.IdDebilidad, P.Id From POKEMONS P, ELEMENTOS E, ELEMENTOS D Where E.Id = P.IdTipo And D.Id = P.IdDebilidad And P.Activo = 1");
                datos.ejecutarLectura();


                while (datos.Lector.Read())
                {
                    Pokemon aux = new Pokemon();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Numero = datos.Lector.GetInt32(0); //lee el valor pocicionandose en la columna de la tabla q se creo anteriormente; "Numero , Nombre ; Decripcion", "Numero" seria la pocicion 0, "Nombre" la posicion 1 y aSI SUCESIVAMENTE
                    aux.Nombre = (string)datos.Lector["Nombre"]; //lee el valor por el nombre de la columna q sew genero en la tabla (pero hay q poner el tipo de datos implicitamente antes )
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                           
                    //if(!(lector.IsDBNull(lector.GetOrdinal("UrlImagen"))))
                    //aux.UrlImagen = (string)lector["UrlImagen"];

                    if (!(datos.Lector["UrlImagen"] is DBNull)) //validar null en base de datos solo si no esta configurada como notnull
                     aux.UrlImagen = (string)datos.Lector["UrlImagen"];

                    aux.Tipo = new Elemento(); //se instancia el objeto elemento
                    aux.Tipo.Id = (int)datos.Lector["IdTipo"]; //se utiliza el id de dtipo para la precarga del los deplegables  cuando quiero modificar el pokemon
                    aux.Tipo.Descripcion = (string)datos.Lector["Tipo"];
                    aux.Debilidad = new Elemento();
                    aux.Debilidad.Id = (int)datos.Lector["IdDebilidad"];
                    aux.Debilidad.Descripcion = (string)datos.Lector["Debilidad"];
                    lista.Add(aux); //agrego lo q lei a la lista de pokemons

                }


                //while (lector.Read()) //mientras q el lector encuentre un dato en la tabla q se creo antes sigue cargando datos
                //{
                //    Pokemon aux = new Pokemon();
                //    aux.Numero = lector.GetInt32(0); //lee el valor pocicionandose en la columna de la tabla q se creo anteriormente; "Numero , Nombre ; Decripcion", "Numero" seria la pocicion 0, "Nombre" la posicion 1 y aSI SUCESIVAMENTE
                //    aux.Nombre = (string)lector["Nombre"]; //lee el valor por el nombre de la columna q sew genero en la tabla (pero hay q poner el tipo de datos implicitamente antes )
                //    aux.Descripcion = (string)lector["Descripcion"];
                //    aux.UrlImagen = (string)lector["UrlImagen"];
                //    aux.Tipo = new Elemento(); //se instancia el objeto elemento
                //    aux.Tipo.Descripcion = (string)lector["Tipo"];
                //    aux.Debilidad = new Elemento();
                //    aux.Debilidad.Descripcion = (string)lector["Debilidad"];
                //    lista.Add(aux); //agrego lo q lei a la lista de pokemons
                // }

                return lista;
            }

            catch (Exception ex)
            {

                throw ex;
            }
            finally { datos.cerrarConexion(); }

            
        }

        public void agregar(Pokemon nuevo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("Insert into POKEMONS (Numero, Nombre, Descripcion, Activo, IdTipo, IdDebilidad, UrlImagen)values(" + nuevo.Numero + ", '" + nuevo.Nombre + "', '" + nuevo.Descripcion + "', 1, @idTipo, @idDebilidad, @urlImagen)"); //distintas formas de agregar datos a la db
                datos.setearParametro("@idTipo", nuevo.Tipo.Id);
                datos.setearParametro("@idDebilidad", nuevo.Debilidad.Id);
                datos.setearParametro("urlImagen", nuevo.UrlImagen);
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

        public void modificar (Pokemon pokemon)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("update POKEMONS set Numero = @numero, Nombre = @nombre, Descripcion = @desc, UrlImagen = @img, IdTipo = @idTipo, IdDebilidad = @idDebilidad Where Id = @id");
                datos.setearParametro("@numero", pokemon.Numero);
                datos.setearParametro("@nombre", pokemon.Nombre);
                datos.setearParametro("@desc", pokemon.Descripcion);
                datos.setearParametro("@img", pokemon.UrlImagen);
                datos.setearParametro("@idTipo", pokemon.Tipo.Id);
                datos.setearParametro("@idDebilidad", pokemon.Debilidad.Id);
                datos.setearParametro("@id", pokemon.Id);

                datos.ejecutarAccion();

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public void eliminar(int id)
        {
            try
            {
                AccesoDatos datos = new AccesoDatos();
                datos.setearConsulta("delete from pokemons where id = @id");
                datos.setearParametro("@id", id);
                datos.ejecutarAccion();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void eliminarLogico(int id)
        {
            try
            {
                AccesoDatos datos = new AccesoDatos();
                datos.setearConsulta("update POKEMONS set Activo = 0 Where id = @id");
                datos.setearParametro("@id", id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Pokemon> filtrar(string campo, string criterio, string filtro)
        {
            List<Pokemon> listaFiltrada = new List<Pokemon>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = "Select Numero, Nombre, P.Descripcion, UrlImagen, E.Descripcion Tipo, D.Descripcion Debilidad, P.IdTipo, P.IdDebilidad, P.Id From POKEMONS P, ELEMENTOS E, ELEMENTOS D Where E.Id = P.IdTipo And D.Id = P.IdDebilidad And P.Activo = 1 And ";
                if (campo == "Numero")
                {
                    switch (criterio)
                    {
                        case "Mayor a":
                            consulta += "Numero > " + filtro;
                            break;
                        case "Menor a":
                            consulta += "Numero < " + filtro;
                            break;
                        default:
                            consulta += "Numero = " + filtro;
                            break;
                    }
                }
                else if (campo == "Nombre")
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "Nombre like '" + filtro + "%' ";
                            break;
                        case "Termina con":
                            consulta += "Nombre like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "Nombre like '%" + filtro + "%'";
                            break;
                    }
                }
                else
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "P.Descripcion like '" + filtro + "%' ";
                            break;
                        case "Termina con":
                            consulta += "P.Descripcion like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "P.Descripcion like '%" + filtro + "%'";
                            break;
                    }
                }

                datos.setearConsulta(consulta);
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Pokemon aux = new Pokemon();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Numero = datos.Lector.GetInt32(0);
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];

                    if (!(datos.Lector["UrlImagen"] is DBNull))
                        aux.UrlImagen = (string)datos.Lector["UrlImagen"];

                    aux.Tipo = new Elemento();
                    aux.Tipo.Id = (int)datos.Lector["IdTipo"];
                    aux.Tipo.Descripcion = (string)datos.Lector["Tipo"];
                    aux.Debilidad = new Elemento();
                    aux.Debilidad.Id = (int)datos.Lector["IdDebilidad"];
                    aux.Debilidad.Descripcion = (string)datos.Lector["Debilidad"];

                    listaFiltrada.Add(aux);
                }
                return listaFiltrada;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
