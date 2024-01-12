using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace ServBoHExtInventarioTrasladoSS.Clases
{
    internal class AppSettings
    {
        IConfiguration configuraciones;


        public AppSettings()
        {

        }

        public string configuracion(string llave)
        {
            return ConfigurationManager.AppSettings[llave].ToString();
        }
    }
}
