using RabbitMQ.Client;
using ServBoHExtInventarioTrasladoSS.Clases;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace ServBoHExtInventarioTrasladoSS.Repositorio
{
    /// <summary>
    /// Clase para enviar los mensajes a la cola, no se modifica.
    /// </summary>
    class RabbitMqEnvioColaRepo
    {
        public RabbitMqEnvioColaRepo()
        {

        }

        public void SendMessageRabbitMq(string mensajeCola)
        {
            DatosCola datosCola = new ConfiguracionColaRepo().ObtenerConfiguracionCola(new AppSettings().configuracion("servicioDestino"));
            var factory = new ConnectionFactory()
            {
                HostName = datosCola.nombreCliente
                , UserName = datosCola.usuario
                , Password = datosCola.contrasena
                , VirtualHost = datosCola.virtualHost
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var body = Encoding.UTF8.GetBytes(mensajeCola);

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.BasicPublish(exchange: "",
                                     routingKey: datosCola.nombreCola,
                                     basicProperties: properties,
                                     body: body);
            }
        }

        public void SendMessageRabbitMq(string mensajeCola,string servicioDestino)
        {
            DatosCola datosCola = new ConfiguracionColaRepo().ObtenerConfiguracionCola(servicioDestino);

            var factory = new ConnectionFactory()
            {
                HostName = datosCola.nombreCliente
                ,
                UserName = datosCola.usuario
                ,
                Password = datosCola.contrasena
                ,
                VirtualHost = datosCola.virtualHost
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var body = Encoding.UTF8.GetBytes(mensajeCola);

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.BasicPublish(exchange: "",
                                     routingKey: datosCola.nombreCola,
                                     basicProperties: properties,
                                     body: body);
            }
        }
    }
}
