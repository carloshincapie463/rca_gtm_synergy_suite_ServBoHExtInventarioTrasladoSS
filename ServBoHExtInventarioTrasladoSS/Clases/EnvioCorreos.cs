using System;
using System.Collections.Generic;
using System.Text;
using EnvioCorreo;


namespace ServBoHExtInventarioTrasladoSS.Clases
{
    /// <summary>
    /// Clase para realizar envio de correos atraves de un servicio web, se puede modificar si es necesario y respetando los metodos del WS de correos. 
    /// </summary>
    public class EnvioCorreos
    {
        public EnvioCorreos()
        {

        }

        public void EnviarCorreo(string asunto, string aviso)
        {
            ServicioEnvioCorreoReporteSoapClient enviar = new ServicioEnvioCorreoReporteSoapClient(ServicioEnvioCorreoReporteSoapClient.EndpointConfiguration.ServicioEnvioCorreoReporteSoap);
            AppSettings configuraciones = new AppSettings();
            enviar.sEnviarCorreoAsync(asunto, configuraciones.configuracion("correo"), "", configuraciones.configuracion("correo"), "", "", aviso);
        }

        public void EnviarCorreo(string asunto, string remitente, string aliasRemitente, string destinatario, string copia, string copiaOculta, string cuerpoMensaje)
        {
            ServicioEnvioCorreoReporteSoapClient enviar = new ServicioEnvioCorreoReporteSoapClient(ServicioEnvioCorreoReporteSoapClient.EndpointConfiguration.ServicioEnvioCorreoReporteSoap);            
            enviar.sEnviarCorreoAsync(asunto,remitente,aliasRemitente,destinatario,copia,copiaOculta,cuerpoMensaje);
        }
    }
}
