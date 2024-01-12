using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServBoHExtInventarioTrasladoSS.Clases
{
    /// <summary>
    /// Clase que se utiliza para serializar los mensaje de transferencia que se envian a las colas de insersiones, esta se modifica con las necesidades del negocio (uso a discreción del programador).
    /// </summary>
    public class Transferencia
    {
        public string idLog { get; set; } 
        public int ambiente { get; set; } 
        public List<ZMMMOVCAB> zMMMOVCAB { get; set; }
        public string sincronizacion { get; set; } 
        public string codigoCentro { get; set; } 
        public string idMenu { get; set; } 
        public int idPais { get; set; }
        public int idEmpresa { get; set; }
        public string DarJSon()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
