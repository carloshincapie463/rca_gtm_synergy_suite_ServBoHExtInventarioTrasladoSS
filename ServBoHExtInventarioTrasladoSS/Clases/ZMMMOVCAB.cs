using System;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

namespace ServBoHExtInventarioTrasladoSS.Clases
{
   public class ZMMMOVCAB
   {

       ///<summary>
       ///Es lleve primaria 
       ///</summary>
       public string IDSYS { get; set; }
       ///<summary>
       ///Es lleve primaria 
       ///</summary>
       public string FRBNR { get; set; }
       ///<summary>
       ///Es lleve primaria 
       ///</summary>
       public string WERKS { get; set; }
        ///<summary>
        ///Puede ser nula 
        ///</summary>
        public string ACT_CODE { get; set; }
        ///<summary>
        ///Puede ser nula 
        ///</summary>
        public string UMWRK { get; set; }
        ///<summary>
        ///Puede ser nula 
        ///</summary>
        public string XBLNR { get; set; }
        ///<summary>
        ///Es lleve primaria 
        ///</summary>
        public string BLDAT { get; set; }
        ///<summary>
        ///Puede ser nula 
        ///</summary>
        public string BUDAT { get; set; }
        ///<summary>
        ///Puede ser nula 
        ///</summary>
        public string BWART { get; set; }
       ///<summary>
       ///Puede ser nula 
       ///</summary>
       public string XABLN { get; set; }
       ///<summary>
       ///Puede ser nula 
       ///</summary>
       public string WEVER { get; set; }
       ///<summary>
       ///Puede ser nula 
       ///</summary>
       public string STATUS { get; set; }

        public List<ZMMMOVPOS> DETALLE { get; set; }

       
    }
}
