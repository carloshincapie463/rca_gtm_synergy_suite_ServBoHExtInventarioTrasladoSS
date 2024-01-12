using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServBoHExtInventarioTrasladoSS.Clases
{
    /// <summary>
    /// Clase que contendra los datos de conexión a la cola de Rabbit, no se modifica.
    /// </summary>
    public class DatosCola
    {
        public string nombreCliente { get; set; }
        public string nombreCola { get; set; }
        public string usuario { get; set; }
        public string contrasena { get; set; }
        public bool autoAck { get; set; }
        public int prefetchSize { get; set; }
        public int prefetchCount { get; set; }
        public bool global { get; set; }
        public string virtualHost { get; set; }

        public DatosCola() { }
    }
}
