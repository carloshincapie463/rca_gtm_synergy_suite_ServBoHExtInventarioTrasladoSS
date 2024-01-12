using Newtonsoft.Json;
using ServBoHExtInventarioTrasladoSS.Clases;
using ServBoHExtInventarioTrasladoSS.Conexion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Text;
using LibreriaArchivosNoEstructurados.Clases;
using LibreriaArchivosNoEstructurados.Repositorio;
using System.Diagnostics.Metrics;

namespace ServBoHExtInventarioTrasladoSS.Repositorio
{
    /// <summary>
    /// Clase que se utiliza para serializar la lógica de las extraciones, esta se modifica con las necesidades del negocio (uso a discreción del programador).
    /// </summary>
    class ExtraccionRepo
    {
        ExtraccionBD extraccionBD;
        string nombreBitacora;
        ArchivoRepo archivoRepo;
        AppSettings appSettings;

        public ExtraccionRepo()
        {
            
        }

        /// <summary>
        /// Clase que se utiliza para realizar la extracción del mensaje.
        /// Comenzando por serializar el mensaje, se ejecuta el Query de la DB BOH_RYR, de esta manera
        /// podemos llenar la lista detalle y con los datos recibidos podemos llenar el encabezado para
        /// el mensaje que se ira a RabbitMQ
        /// </summary>
        /// 
        public void EjecutaExtraccion(string mensaje)
        {

            Peticion mensajeCola = JsonConvert.DeserializeObject<Peticion>(mensaje);
            try {
                switch (mensajeCola.tipoSincronizacion)
                {
                    case "02":
                        ExtraccionTraslado(mensaje);
                        break;
                    case "03":
                        ExtraccionInventario(mensaje);
                        break;
                }
            }
            catch(Exception e) { 
                EscribeBitacora(e.Message);
                EscribeBitacora(e.StackTrace);
            }

        }
        public void ExtraccionTraslado(string mensaje)
        {
            appSettings = new AppSettings();
            HabilitaBitacora();
            RabbitMqEnvioColaRepo rabbitMqEnvioColaRepo = new RabbitMqEnvioColaRepo();
            Transferencia transferencia = new Transferencia();
            ;
            Peticion mensajeCola = JsonConvert.DeserializeObject<Peticion>(mensaje);
            DateTime currentDateTime = DateTime.Now;
            string formattedDateTime = currentDateTime.ToString("dddd, dd MMMM yyyy HH:mm:ss");
            EscribeBitacora("Inicio de Extracción de inventario por usuario: " + mensajeCola.usuario);
            EscribeBitacora(" a las : " + formattedDateTime );
            extraccionBD = new ExtraccionBD(mensajeCola.ambiente.ToString());
            EscribeBitacora("");
            transferencia.idLog = extraccionBD.InsertaLogPeticionInicial(mensajeCola);

            DataSet datosInventario = extraccionBD.ObtenerTraslado(mensajeCola);
            DataTable tablaInventario = datosInventario.Tables[0];
            List<ZMMMOVCAB> listaEncabezado = new List<ZMMMOVCAB>();

            var traslados = datosInventario.Tables[0].AsEnumerable().GroupBy(row => new
            {
                FRBNR = row["FRBNR"]
            }).Select(g => g.CopyToDataTable()).ToList();
            foreach ( DataTable traslado in traslados)
            {
                ZMMMOVCAB encabezado = new ZMMMOVCAB
                {
                    WERKS = mensajeCola.idCentroInicio,
                    ACT_CODE = mensajeCola.tipoSincronizacion,
                    BLDAT = mensajeCola.fechaInicio,
                    BUDAT = mensajeCola.fechaInicio,
                    FRBNR = traslado.Rows[0]["FRBNR"].ToString(),
                    UMWRK = traslado.Rows[0]["UMWRK"].ToString(),
                };
                List<ZMMMOVPOS> listadoDetalle = new List<ZMMMOVPOS>();
                foreach (DataRow fila in traslado.Rows)
                {
                    ZMMMOVPOS item = new ZMMMOVPOS
                    {
                        BLDAT = "0",
                        MATNR = fila["MATNR"].ToString(),
                        ERFMG = Convert.ToDecimal(fila["ERFMG"]),
                        ERFME = fila["ERFME"].ToString(),
                        UMWRK = traslado.Rows[0]["UMWRK"].ToString()
                    };
                    listadoDetalle.Add(item);
                }
                encabezado.DETALLE = listadoDetalle;
                listaEncabezado.Add(encabezado);
            }
            if (datosInventario.Tables[0].Rows.Count != 0)
            {                
                transferencia.sincronizacion = "SincronizacionTrasladoSS";
                transferencia.ambiente = mensajeCola.ambiente;
                transferencia.idMenu = appSettings.configuracion("idMenu");
                transferencia.codigoCentro = mensajeCola.idCentroInicio;
                transferencia.zMMMOVCAB = listaEncabezado;
                transferencia.idPais = mensajeCola.idPais;
                transferencia.idEmpresa = mensajeCola.idEmpresa;
                extraccionBD.ActualizarLogPeticionDatos(Convert.ToInt16(transferencia.idLog), "Se realizó la extracción de datos", 1);
                EscribeBitacora("Datos de extracción "  );
                EscribeBitacora("Centro inicio: " + mensajeCola.idCentroInicio);
                EscribeBitacora("Centro final: " + mensajeCola.idCentroFinal);
                EscribeBitacora("Fecha Contabilidad: " + mensajeCola.fechaContabilidad);
                EscribeBitacora("Se realizó la extracción traslado completa");
                rabbitMqEnvioColaRepo.SendMessageRabbitMq(transferencia.DarJSon());
            }
            else
            {
                EscribeBitacora("No hay datos para la extracción");
                extraccionBD.ActualizarLogPeticionDatos(Convert.ToInt16(transferencia.idLog), "No hay datos para extraer", 0);
            }
        }

        public void ExtraccionInventario(string mensaje)
        {
            appSettings = new AppSettings();
            HabilitaBitacora();
            RabbitMqEnvioColaRepo rabbitMqEnvioColaRepo = new RabbitMqEnvioColaRepo();
            Transferencia transferencia = new Transferencia();
            
            Peticion mensajeCola = JsonConvert.DeserializeObject<Peticion>(mensaje);
            extraccionBD = new ExtraccionBD(mensajeCola.ambiente.ToString());
            DateTime currentDateTime = DateTime.Now;
            string formattedDateTime = currentDateTime.ToString("dddd, dd MMMM yyyy HH:mm:ss");
            EscribeBitacora("Inicio de Extracción de inventario por usuario: " + mensajeCola.usuario);
            EscribeBitacora(" a las : " + formattedDateTime);
            transferencia.idLog = extraccionBD.InsertaLogPeticionInicial(mensajeCola);

            DataSet datosInventario = extraccionBD.ObtenerInventario(mensajeCola);
            DataTable tablaInventario = datosInventario.Tables[0];

            if (datosInventario.Tables[0].Rows.Count != 0)
            {
                List<ZMMMOVPOS> listadoDetalle = new List<ZMMMOVPOS>();
                foreach (DataRow fila in tablaInventario.Rows)
                {
                    ZMMMOVPOS item = new ZMMMOVPOS
                    {
                        BLDAT = mensajeCola.fechaContabilidad,
                        MATNR = fila["material_sap"].ToString(),
                        MENGE_F = Convert.ToDecimal(fila["cantidad"]),
                        ZLDAT_F = mensajeCola.fechaContabilidad,
                        MEINS_S = fila["unidad_medida"].ToString(),
                    };
                    listadoDetalle.Add(item);
                }
                List<ZMMMOVCAB> listaEncabezado = new List<ZMMMOVCAB>();
                ZMMMOVCAB encabezado = new ZMMMOVCAB
                {
                    WERKS = mensajeCola.idCentroInicio,
                    ACT_CODE = mensajeCola.tipoSincronizacion,
                    BLDAT = mensajeCola.fechaContabilidad,
                    BUDAT = mensajeCola.fechaContabilidad,
                    DETALLE = listadoDetalle
                };

                listaEncabezado.Add(encabezado);
                transferencia.sincronizacion = "SincronizacionInventarioSS"; 
                transferencia.ambiente = mensajeCola.ambiente;
                transferencia.idMenu = appSettings.configuracion("idMenu");
                transferencia.codigoCentro = mensajeCola.idCentroInicio;
                transferencia.zMMMOVCAB = listaEncabezado;
                transferencia.idPais = mensajeCola.idPais;
                transferencia.idEmpresa = mensajeCola.idEmpresa;

                extraccionBD.ActualizarLogPeticionDatos(Convert.ToInt16(transferencia.idLog),"Se realizó la extracción de datos",1);
                EscribeBitacora("Datos de extracción ");
                EscribeBitacora("Centro inicio: " + mensajeCola.idCentroInicio);
                EscribeBitacora("Centro final: " + mensajeCola.idCentroFinal);
                EscribeBitacora("Fecha Contabilidad: " + mensajeCola.fechaContabilidad);
                EscribeBitacora("Se realizó la extracción inventario completa");
                
                rabbitMqEnvioColaRepo.SendMessageRabbitMq(transferencia.DarJSon());
            }
            else
            {
                EscribeBitacora("No hay datos para la extracción");
                extraccionBD.ActualizarLogPeticionDatos(Convert.ToInt16(transferencia.idLog), "No hay datos para extraer", 0);
            }
        }

      
        /// <summary>
        /// Función para poder guardar una bitacora local y de haber un error hacerle seguimiento
        /// </summary>
        void HabilitaBitacora()
        {
            nombreBitacora = new AppSettings().configuracion("directorioBitacoraErrores") + DateTime.Now.ToString("yyyyMMdd") + ".csv";
            ArchivoCls archivoCls = new ArchivoCls();
            archivoCls.archivo = nombreBitacora;
            archivoCls.tipoDirectorio = TipoDirectorio.local;
            archivoRepo = new ArchivoRepo();
            Respuesta respuesta = archivoRepo.ValidaArchivo(archivoCls);
            if (respuesta.estado == false)
            {
                archivoRepo.EscribirArchivo(archivoCls);
            }
        }
        public void EscribeBitacora(string texto)
        {
            ArchivoCls archivoCls = new ArchivoCls();
            archivoCls.archivo = nombreBitacora;
            archivoCls.tipoDirectorio = TipoDirectorio.local;
            archivoRepo = new ArchivoRepo();
            archivoCls.contenido = texto + ",";
            archivoRepo.AgregarLineasArchivo(archivoCls);
        }

    }
}
