using System;
using System.Xml.Serialization;
using System.Xml;
using System.Text;
namespace ServBoHExtInventarioTrasladoSS.Clases
{
    [XmlRoot(ElementName = "Encabezado")]
    public class ZMMPEDCAB
   {
        public string Mandt { get; set; }
        public string Idsys { get; set; }
       public string Werks { get; set; }
       public string Penum { get; set; }
       public string Bedat { get; set; }
       ///<summary>
       ///Puede ser nula 
       ///</summary>
       public string Slfdt { get; set; }
       ///<summary>
       ///Puede ser nula 
       ///</summary>
       public string Stype { get; set; }
       ///<summary>
       ///Puede ser nula 
       ///</summary>
       public string Lifnr { get; set; }
       ///<summary>
       ///Puede ser nula 
       ///</summary>
       public string Reswk { get; set; }
       ///<summary>
       ///Puede ser nula 
       ///</summary>
       public string Route { get; set; }
       ///<summary>
       ///Puede ser nula 
       ///</summary>
       public string AFNAM { get; set; }
       ///<summary>
       ///Puede ser nula 
       ///</summary>
       public string Status { get; set; }
       ///<summary>
       ///Puede ser nula 
       ///</summary>
       public string Xblnr { get; set; }
       ///<summary>
       ///Puede ser nula 
       ///</summary>
       public string Zuonr { get; set; }
        public string Datum { get; set; }

        public string Uzeit { get; set; }
        public List<Detalle> listaDetalles { get; set; }

        public string Xml()
        {
            string xml;
            XmlSerializer serializer = new XmlSerializer(GetType());
            using (MemoryStream memStream = new MemoryStream())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(memStream, Encoding.UTF8))
                {
                    serializer.Serialize(xmlWriter, this);
                    xml = Encoding.UTF8.GetString(memStream.GetBuffer());
                    xml = xml.Replace("xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "");
                    xml = xml.Replace(" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");
                    xml = xml.Replace("<listaDetalles>", "");
                    xml = xml.Replace("</listaDetalles>", "");
                    xml = xml.Substring(xml.IndexOf(Convert.ToChar(60)));
                    xml = xml.Substring(0, (xml.LastIndexOf(Convert.ToChar(62)) + 1));
                    xml = xml.Remove(0,38);                    
                }
            }

            return xml;
        }
    }
}
