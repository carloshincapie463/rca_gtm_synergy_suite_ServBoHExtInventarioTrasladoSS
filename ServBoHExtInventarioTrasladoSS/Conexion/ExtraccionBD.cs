using Microsoft.VisualBasic;
using RCA_DB;
using ServBoHExtInventarioTrasladoSS.Clases;
using ServBoHExtInventarioTrasladoSS.Repositorio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;

namespace ServBoHExtInventarioTrasladoSS.Conexion
{
    /// <summary>
    /// Clase que se utiliza para serializar una conexión a base de datos, esta se modifica con las necesidades del negocio (uso a discreción del programador).
    /// </summary>
    class ExtraccionBD
    {
        clsDB clsdb;
        clsDB clsdb2;
        int tiempoExpiracion;
        AppSettings appSettings;

        public ExtraccionBD(string ambiente)
        {
            appSettings = new AppSettings();
            clsdb = new RcaBaseDatosRepo().CrearConeccion(ambiente);
            clsdb2 = new RcaBaseDatosRepo().CrearConeccion(appSettings.configuracion("conexionDB2"), ambiente);

            tiempoExpiracion = 900;
        }
        /// <summary>
        /// Función para obtener los elementos que tiene en inventario el centro deseado.
        /// </summary>
        /// <param name="peticion"></param>
        /// <returns></returns>
        public DataSet ObtenerInventario(Peticion peticion)
        {
            string query = @"select pro.stock_code, pro.id, uni.code, cop.unit_id into #material
                            from PRODUCT pro
                            inner join COMPANY_PRODUCT cop on pro.id=cop.product_id
                            inner join UNIT_OF_MEASURE uni on cop.unit_id=uni.id
                            group by pro.stock_code, pro.id, uni.code,cop.unit_id

                            SELECT  com.company_code as codigo_centro, mat.stock_code material_sap, stl.count_quantity cantidad, mat.code unidad_medida
                            from COMPANY com 
                            inner join stock_take  stt on com.id= stt.company_id
                            inner join (select product_id,stock_take_id,stock_unit_id, sum(count_quantity) count_quantity from stock_take_line group by product_id,stock_take_id,stock_unit_id) stl on stt.id = stl.stock_take_id
                            inner join #material mat on mat.id=stl.product_id and mat.unit_id=stl.stock_unit_id
                            Where 
                            stt.stock_count_date = @BLDAT and com.company_code=@WERKS and stt.type_id=3"; 

            SqlParameter[] parametros = new SqlParameter[2];
            parametros[0] = new SqlParameter() { ParameterName = "@WERKS", Value = peticion.idCentroInicio };
            parametros[1] = new SqlParameter() { ParameterName = "@BLDAT", Value = DateTime.ParseExact(peticion.fechaInicio, "yyyyMMdd", CultureInfo.InvariantCulture) };
            return clsdb.mConsultaSQL(query, parametros);
        }


        /// <summary>
        /// Función para obtener los elementos que tiene en inventario el centro deseado.
        /// </summary>
        /// <param name="peticion"></param>
        /// <returns></returns>
        public DataSet ObtenerTraslado(Peticion peticion)
        {
            string query = @"select pro.stock_code, pro.id, uni.code, cop.unit_id into #material
                            from PRODUCT pro
                            inner join COMPANY_PRODUCT cop on pro.id=cop.product_id
                            inner join UNIT_OF_MEASURE uni on cop.unit_id=uni.id
                            group by pro.stock_code, pro.id, uni.code,cop.unit_id

                            SELECT co2.company_code WERKS
	                              ,co1.company_code UMWRK
                                  ,trh.our_ref FRBNR	  
	                              ,mat.stock_code MATNR      
                                  ,(quantity_out + quantity_in) ERFMG
                                  , mat.code ERFME
                              FROM TRANSACTION_HEADER trh
                              inner join COMPANY co1 on trh.outlet_id=co1.id
                              inner join COMPANY co2 on trh.supplier_id=co2.id
                              inner join TRANSACTION_DETAIL trd on trd.transaction_header_id=trh.id
                              inner join #material mat on mat.id=trd.product_id and mat.unit_id=trd.transaction_unit_id
                              where transaction_date=@BLDAT
                              and trh.transaction_type= 'TRANSFER_IN'
                              and alt_ref is not null
                              and co2.company_code=@WERKS";

            SqlParameter[] parametros = new SqlParameter[2];
            parametros[0] = new SqlParameter() { ParameterName = "@WERKS", Value = peticion.idCentroInicio };
            parametros[1] = new SqlParameter() { ParameterName = "@BLDAT", Value = DateTime.ParseExact(peticion.fechaInicio, "yyyyMMdd", CultureInfo.InvariantCulture) };
            return clsdb.mConsultaSQL(query, parametros);
        }





        /// <summary>
        /// Funcion para dejar registro en la bitacora de la DB
        /// </summary>
        /// <param name="consulta"></param>
        /// <returns></returns>
        public string InsertaLogPeticionInicial(Peticion consulta)
        {            
            string query = @"DECLARE @pidentity INT
            INSERT INTO dbo.ps_bitacora_dw
                       (nombre
                       ,id_usuario
                       ,peticion
                       ,respuesta
                       ,fecha
                       ,estado)
                 VALUES
                       (@nombre
                       ,@id_usuario
                       ,@peticion
                       ,@respuesta
                       ,GETDATE()
                       ,0)
                set @pidentity= SCOPE_IDENTITY();

 

	            select @pidentity";

            SqlParameter[] parametros = new SqlParameter[4];
            parametros[0] = new SqlParameter() { ParameterName = "@nombre", Value = "Sincronizacion inventario SynergySuite" };
            parametros[1] = new SqlParameter() { ParameterName = "@id_usuario", Value = consulta.usuario };
            parametros[2] = new SqlParameter() { ParameterName = "@peticion", Value = consulta.DarJSon() };
            parametros[3] = new SqlParameter() { ParameterName = "@respuesta", Value = "Generada" };
            string log = clsdb2.mEjecutaEscalar(query, parametros);

            query = @"INSERT INTO ps_bitacora_dw_complemento
                       (id_bitacora
                       ,codigo_centro
                       ,fecha_peticion
                       ,tipo_sincronizacion
                       ,id_menu)
                 VALUES
                       (@id_bitacora
                       ,@codigo_centro
                       ,@fecha_peticion
                       ,@tipo_sincronizacion
                       ,@id_menu)";

            parametros = new SqlParameter[5];
            parametros[0] = new SqlParameter() { ParameterName = "@id_bitacora", Value = log };
            parametros[1] = new SqlParameter() { ParameterName = "@codigo_centro", Value = consulta.idCentroInicio };
            parametros[2] = new SqlParameter() { ParameterName = "@fecha_peticion", Value = DateTime.ParseExact(consulta.fechaContabilidad,"yyyyMMdd",CultureInfo.InvariantCulture)};
            string respuesta = "";
            if(consulta.tipoSincronizacion == "03")
            {
                respuesta = "Inventario";
            }
            else
            {
                respuesta = "Translado";
            }
            parametros[3] = new SqlParameter() { ParameterName = "@tipo_sincronizacion", Value = respuesta};
            parametros[4] = new SqlParameter() { ParameterName = "@id_menu", Value = appSettings.configuracion("idMenu") };
            clsdb2.mEjecutaSQL(query, parametros);

            return log;
        }
        //Funcion para actualizar el registro en la bitacora de la DB
        public string ActualizarLogPeticionDatos(int log_, string respuesta, int estado)
        {
            
            string query = @"UPDATE ps_bitacora_dw
            SET estado = @estado, respuesta = @respuesta where id_bitacora = @id_bitacora";

            SqlParameter[] parametros = new SqlParameter[3];
            parametros[0] = new SqlParameter() { ParameterName = "@id_bitacora", Value = log_ };
            parametros[1] = new SqlParameter() { ParameterName = "@respuesta", Value = respuesta };
            parametros[2] = new SqlParameter() { ParameterName = "@estado", Value = estado };
            clsdb2.mEjecutaSQL(query, parametros);
            return log_.ToString();
        }



    }
}
