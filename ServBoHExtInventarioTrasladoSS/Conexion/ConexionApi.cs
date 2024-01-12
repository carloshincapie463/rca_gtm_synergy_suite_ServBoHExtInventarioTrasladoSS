using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ServBoHExtInventarioTrasladoSS.Conexion
{
    /// <summary>
    /// Clase base para consumo de API's, solo se modificará si el nogocio así lo requiere.
    /// </summary>
    public class ConexionApi
    {
        public ConexionApi()
        {
        }

        public string crearRespuestApi(string rutaApi, Dictionary<string, string> cuerpo, string verboApi, bool escalar, bool json)
        {
            string respuestaApi = "";
            Task<string> respuetaJson = solicitarRespuestApi(rutaApi, cuerpo, verboApi, json);

            if (respuetaJson != null)
            {
                respuestaApi = escalar ? respuetaJson.Result : respuestaApi = respuetaJson.Result;//JObject.Parse(respuetaJson.Result)["Table"].ToString();
            }

            return respuestaApi;
        }

        public Task<string> solicitarRespuestApi(string rutaApi, Dictionary<string, string> cuerpo, string verboApi, bool json)
        {
            try
            {
                HttpClient clienteApi = new HttpClient();
                clienteApi.Timeout = TimeSpan.FromSeconds(900000);
                var respuestaAPI = mensajeRespuestHttp(clienteApi, rutaApi, cuerpo, verboApi, json);
                respuestaAPI.Wait();

                var resultadoHttp = respuestaAPI.Result;
                if (resultadoHttp.IsSuccessStatusCode)
                {
                    var data = resultadoHttp.Content.ReadAsStringAsync();
                    data.Wait();
                    if (data.Result != "null")
                    {
                        return data;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return null;
        }

        public Task<HttpResponseMessage> mensajeRespuestHttp(HttpClient clienteApi, string rutaApi, Dictionary<string, string> cuerpo, string verboApi, bool json)
        {
            switch (verboApi)
            {
                case "POST":
                    HttpContent contenidoCuerpo;
                    if (json)
                    {
                        string jsonListaHuella = cuerpo["json"];
                        contenidoCuerpo = new StringContent(jsonListaHuella, Encoding.UTF8, "application/json");
                    }
                    else
                    {
                        contenidoCuerpo = new FormUrlEncodedContent(cuerpo);
                    }
                    return clienteApi.PostAsync(rutaApi, contenidoCuerpo);
                case "GET":
                    return clienteApi.GetAsync(rutaApi);
                case "PUT":
                    HttpContent contenidoCuerpoPUT;
                    if (json)
                    {
                        string jsonListaHuella = cuerpo["json"];
                        contenidoCuerpoPUT = new StringContent(jsonListaHuella, Encoding.UTF8, "application/json");
                    }
                    else
                    {
                        contenidoCuerpoPUT = new FormUrlEncodedContent(cuerpo);
                    }
                    return clienteApi.PutAsync(rutaApi, contenidoCuerpoPUT);
            }

            return null;
        }

        public Dictionary<string, string> convertirCuerpoHttp(Object objetoCuerpo)
        {
            PropertyInfo[] atributosCuerpo = objetoCuerpo.GetType().GetProperties();
            Dictionary<string, string> cuerpoHttp = new Dictionary<string, string>();

            foreach (PropertyInfo atributo in atributosCuerpo)
            {
                string valorAtributo = atributo.GetValue(objetoCuerpo, null) == null ? "" : atributo.GetValue(objetoCuerpo, null).ToString();
                cuerpoHttp.Add(atributo.Name, valorAtributo);
            }

            return cuerpoHttp;
        }

        public string crearRutaAccion(string rutaAccion, string[] nombresParametros, string[] valoresParametros)
        {
            string caracterApoyo = "?";

            if ((nombresParametros.Length == valoresParametros.Length) && nombresParametros.Length > 0)
            {
                for (int i = 0; i < nombresParametros.Length; i++)
                {
                    rutaAccion += caracterApoyo + nombresParametros[i] + "=" + valoresParametros[i];
                    caracterApoyo = "&";
                }
            }

            return rutaAccion;
        }
    }
}
