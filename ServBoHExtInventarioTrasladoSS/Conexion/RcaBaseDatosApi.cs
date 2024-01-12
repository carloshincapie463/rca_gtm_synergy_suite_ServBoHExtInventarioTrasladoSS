using Newtonsoft.Json;
using ServBoHExtInventarioTrasladoSS.Clases;
using ServBoHExtInventarioTrasladoSS.Conexion;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace ServBoHExtInventarioTrasladoSS.Conexion
{
    /// <summary>
    /// Clase que obtiene los datos de conexión a la base de datos, no se modifica.
    /// </summary>
    class RcaBaseDatosApi
    {
        ConexionApi conexionApi;

        public RcaBaseDatosApi()
        {
            conexionApi = new ConexionApi();
        }

        public string ObtenerCadenadeConexion(string ambiente)
        {
            AppSettings appSettings = new AppSettings();
            string ruta = appSettings.configuracion("apiConexionDB");
            string verboHttp = "GET";
            string[] nombreParametros = new string[] { "nombreBD", "codigoAmbiente" };
            string[] valorParametros = new string[] { appSettings.configuracion("conexionDB"), ambiente };
            string rutaCompleta = conexionApi.crearRutaAccion(ruta, nombreParametros, valorParametros);
            Dictionary<string, string> cuerpo = null;
            return conexionApi.crearRespuestApi(rutaCompleta, cuerpo, verboHttp, true, true);
        }

        public string ObtenerCadenadeConexion(string conexionDB,string ambiente)
        {
            AppSettings appSettings = new AppSettings();
            string ruta = appSettings.configuracion("apiConexionDB");
            string verboHttp = "GET";
            string[] nombreParametros = new string[] { "nombreBD", "codigoAmbiente" };
            string[] valorParametros = new string[] { conexionDB, ambiente };
            string rutaCompleta = conexionApi.crearRutaAccion(ruta, nombreParametros, valorParametros);
            Dictionary<string, string> cuerpo = null;
            return conexionApi.crearRespuestApi(rutaCompleta, cuerpo, verboHttp, true, true);
        }

        

    }
}
