using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;

namespace ServBoHExtInventarioTrasladoSS
{
    class Program
    {
        /// <summary>
        /// Clase de inicio del programa se copia esta configuración no se cambia a menos que sea necesario
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                 .ConfigureHostConfiguration(configHost =>
                 {
                     //Configuración del host
                 })
                 .ConfigureAppConfiguration((hostContext, configApp) =>
                 {
                     //Configuración de la aplicacion
                 })
                .ConfigureServices((hostContext, services) =>
                {
                    //Configuración de los servicios
                    services.AddHostedService<LifetimeHostedService>();
                });

            if (!Debugger.IsAttached) //En caso de no haber debugger, lanzamos como servicio
            {
                await host.RunServiceAsync();
            }
            else
            {
                await host.RunConsoleAsync();
            }
        }
    }
}