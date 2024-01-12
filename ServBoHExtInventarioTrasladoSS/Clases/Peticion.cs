using System;
using Newtonsoft.Json;


namespace ServBoHExtInventarioTrasladoSS.Clases
{
    /// <summary>
    /// Clase que se utiliza para serializar los mensaje de petición que se envian a las colas de peticiones, esta se modifica con las necesidades del negocio (uso a discreción del programador).
    /// </summary>
    public class Peticion
    {
        public int idPais { get; set; }
        public int idEmpresa { get; set; } 
        public int idEmpresaCliente { get; set; } 
        public int idEmpresaProveedor { get; set; } 
        public string idCentroInicio { get; set; } 
        public string idCentroFinal { get; set; }
        public string fechaInicio { get; set; }
        public string fechaFinal { get; set; }
        public string tipoSincronizacion { get; set; }
        public string tipoConfirmacion { get; set; }
        public string fechaContabilidad { get; set; }
        public int tipo { get; set; }
        public int ambiente { get; set; } 
        public int usuario { get; set; }
        public string DarJSon()
        {
            return JsonConvert.SerializeObject(this);
        }

    }

  

   
}
