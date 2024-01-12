using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ServBoHExtInventarioTrasladoSS.Clases;
using ServBoHExtInventarioTrasladoSS.Repositorio;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace ServBoHExtInventarioTrasladoSS

{
    /// <summary>
    /// Clase base para los servicios, esta no se toca a menos que se necesite algo especifico.
    /// </summary>
    public class LifetimeHostedService : IHostedService, IDisposable
    {
        ILogger<LifetimeHostedService> logger;
        private Timer _timer;
        private int time;

        public LifetimeHostedService(ILogger<LifetimeHostedService> logger, IHostEnvironment hostingEnvironment)
        {
            this.logger = logger;
            ColaRabbit();
        }

        /// <summary>
        /// Método que no ejecuta acciones, se establecio para que el servico se encuentre activo
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                time = 60;
                _timer = new Timer((e) => iniciarProceso(), null, TimeSpan.Zero, TimeSpan.FromMinutes(time));
            }
            catch (Exception ex)
            {
                logger.LogError($"{ex.Message},{ex.StackTrace}");
            }
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
        /// <summary>
        /// Método que no ejecuta acciones, se establecio para que el servico se encuentre activo
        /// </summary>
        public void iniciarProceso()
        {
        }
        /// <summary>
        /// Acá se establece el cliente que escucha la cola de Rabbit
        /// </summary>
        public void ColaRabbit()
        {
            try
            {
                DatosCola datosCola = new ConfiguracionColaRepo().ObtenerConfiguracionCola(new AppSettings().configuracion("nombreServicio"));
                var factory = new ConnectionFactory()
                {
                    HostName = datosCola.nombreCliente,
                    UserName = datosCola.usuario,
                    Password = datosCola.contrasena,
                    VirtualHost = datosCola.virtualHost
                };
                var _connection = factory.CreateConnection();
                var _channel = _connection.CreateModel();
                _channel.BasicQos(prefetchSize: (uint)datosCola.prefetchSize, prefetchCount: (ushort)datosCola.prefetchCount, global: datosCola.global);
                EventingBasicConsumer consumer = new EventingBasicConsumer(_channel);
                _channel.BasicConsume(queue: datosCola.nombreCola, autoAck: datosCola.autoAck, consumer: consumer);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    ExtraccionRepo extraccionRepo = new ExtraccionRepo();
                    try
                    {
                        extraccionRepo.EjecutaExtraccion(message);
                        _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                    }
                    catch (Exception e)
                    {
                        _channel.BasicNack(deliveryTag: ea.DeliveryTag, multiple: false, requeue: false);
                    }
                    finally
                    {
                        extraccionRepo = null;
                    }
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
