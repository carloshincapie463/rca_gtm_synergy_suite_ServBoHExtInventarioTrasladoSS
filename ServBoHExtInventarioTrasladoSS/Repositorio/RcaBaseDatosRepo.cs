using Newtonsoft.Json;
using RCA_DB;
using ServBoHExtInventarioTrasladoSS.Clases;
using ServBoHExtInventarioTrasladoSS.Conexion;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServBoHExtInventarioTrasladoSS.Repositorio
{
    /// <summary>
    /// Clase que consume el servio de obtención de datos para la conexión a la base de datos, no se modifica.
    /// </summary>
    class RcaBaseDatosRepo
    {
        public DatosConexion ObtenerCadenadeConexion(string ambiente)
        {
            string json = new RcaBaseDatosApi().ObtenerCadenadeConexion(ambiente);
            List<DatosConexion> listDatosConexion = JsonConvert.DeserializeObject<List<DatosConexion>>(json);

            return listDatosConexion[0];
        }

        public DatosConexion ObtenerCadenadeConexion(string conexionDB, string ambiente)
        {
            string json = new RcaBaseDatosApi().ObtenerCadenadeConexion(conexionDB,ambiente);
            List<DatosConexion> listDatosConexion = JsonConvert.DeserializeObject<List<DatosConexion>>(json);
            return listDatosConexion[0];
        }

        public clsDB CrearConeccion(string ambiente)
        {
            try
            {
                DatosConexion datosConexion = ObtenerCadenadeConexion(ambiente);

                if (datosConexion != null)
                {
                    string servidor = datosConexion.direccion;
                    string bd = datosConexion.nombre;
                    string usuarioBd = datosConexion.usuario;
                    string contrasenaBd = datosConexion.clave;
                    string puerto = datosConexion.puerto;

                    clsDB clsDb;

                    if (puerto == "0" || string.IsNullOrEmpty(puerto))
                    {
                        clsDb = new clsDB(servidor, bd, usuarioBd, contrasenaBd);
                    }
                    else
                    {
                        clsDb = new clsDB(servidor, bd, usuarioBd, contrasenaBd, puerto);
                    }

                    return clsDb;
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return null;
        }

     
        public clsDB CrearConeccion(string conexionDB, string ambiente)

        {

            try

            {

                DatosConexion datosConexion = ObtenerCadenadeConexion(conexionDB, ambiente);



                if (datosConexion != null)

                {

                    string servidor = datosConexion.direccion;

                    string bd = datosConexion.nombre;

                    string usuarioBd = datosConexion.usuario;

                    string contrasenaBd = datosConexion.clave;

                    string puerto = datosConexion.puerto;



                    clsDB clsDb;



                    if (puerto == "0" || string.IsNullOrEmpty(puerto))

                    {

                        clsDb = new clsDB(servidor, bd, usuarioBd, contrasenaBd);

                    }

                    else

                    {

                        clsDb = new clsDB(servidor, bd, usuarioBd, contrasenaBd, puerto);

                    }



                    return clsDb;

                }

            }

            catch (Exception ex)

            {

                return null;

            }



            return null;

        }


    }
}
