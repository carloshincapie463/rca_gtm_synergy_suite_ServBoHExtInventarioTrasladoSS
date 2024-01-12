using Newtonsoft.Json;
using ServBoHExtInventarioTrasladoSS.Clases;
using ServBoHExtInventarioTrasladoSS.Conexion;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServBoHExtInventarioTrasladoSS.Repositorio
{
    /// <summary>
    /// Clase que consume el servio de obtención de datos para la conexión a la cola, no se modifica.
    /// </summary>
    class ConfiguracionColaRepo
    {

        public DatosCola ObtenerConfiguracionCola(string nombreServicio)
        {
            string json = new ConfiguracionColaApi().ObtenerConfiguracionColaAPI(nombreServicio);
            DatosCola datosCola = JsonConvert.DeserializeObject<DatosCola>(json);

            return datosCola;
        }

        public DatosCola ObtenerConfiguracionCola()
        {
            string json = new ConfiguracionColaApi().ObtenerConfiguracionColaAPI();
            DatosCola datosCola = JsonConvert.DeserializeObject<DatosCola>(json);

            return datosCola;
        }
    }
}
