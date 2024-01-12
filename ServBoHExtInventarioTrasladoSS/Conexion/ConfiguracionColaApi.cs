using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using ServBoHExtInventarioTrasladoSS.Clases;

namespace ServBoHExtInventarioTrasladoSS.Conexion
{
    /// <summary>
    /// Clase que obtiene los datos de conexión a la cola, no se modifica.
    /// </summary>
    class ConfiguracionColaApi
    {
        ConexionApi conexionApi;

        public ConfiguracionColaApi()
        {
            conexionApi = new ConexionApi();
        }

        public string ObtenerConfiguracionColaAPI()
        {
            string ruta = new AppSettings().configuracion("urlApiColas");
            string nombreServicio = new AppSettings().configuracion("servicioDestino");
            string verboHttp = "GET";
            string[] nombreParametros = new string[] { "nombreServicio" };
            string[] valorParametros = new string[] { nombreServicio };
            string rutaCompleta = conexionApi.crearRutaAccion(ruta, nombreParametros, valorParametros);
            Dictionary<string, string> cuerpo = null;

            return conexionApi.crearRespuestApi(rutaCompleta, cuerpo, verboHttp, true, true);
        }

        public string ObtenerConfiguracionColaAPI(string nombreServicio)
        {
            string ruta = new AppSettings().configuracion("urlApiColas");
            string verboHttp = "GET";
            string[] nombreParametros = new string[] { "nombreServicio" };
            string[] valorParametros = new string[] { nombreServicio };
            string rutaCompleta = conexionApi.crearRutaAccion(ruta, nombreParametros, valorParametros);
            Dictionary<string, string> cuerpo = null;

            return conexionApi.crearRespuestApi(rutaCompleta, cuerpo, verboHttp, true, true);
        }
    }
}
