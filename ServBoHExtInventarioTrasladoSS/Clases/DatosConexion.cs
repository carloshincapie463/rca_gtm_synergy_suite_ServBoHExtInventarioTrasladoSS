using System;
using System.Collections.Generic;
using System.Text;

namespace ServBoHExtInventarioTrasladoSS.Clases
{
    /// <summary>
    /// Clase que contendrá los datos de conexión a la base de datos, no se puede modificar.
    /// </summary>
    class DatosConexion
    {
        public string nombre { get; set; }
        public string usuario { get; set; }
        public string clave { get; set; }
        public string direccion { get; set; }
        public string puerto { get; set; }

        public DatosConexion()
        {

        }
    }
}
