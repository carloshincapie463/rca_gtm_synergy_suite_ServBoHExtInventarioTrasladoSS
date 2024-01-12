using System;

namespace ServBoHExtInventarioTrasladoSS.Clases
{
   public class ZMMMOVPOS
    {
       public string ZEILE { get; set; }
        ///<summary>
        ///Es lleve primaria 
        ///</summary>
        public string BLDAT { get; set; }
       ///<summary>
       ///Puede ser nula 
       ///</summary>
       public string MATNR { get; set; }
        ///<summary>
        ///Puede ser nula 
        ///</summary>
        public Nullable<decimal> ERFMG { get; set; }
       ///<summary>
       ///Puede ser nula 
       ///</summary>
       public string ERFME { get; set; }
       ///<summary>
       ///Puede ser nula 
       ///</summary>
       public string LGORT { get; set; }
       ///<summary>
       ///Puede ser nula 
       ///</summary>
       public string CHARG { get; set; }
        ///<summary>
        ///Puede ser nula 
        ///</summary>
        public string UMWRK { get; set; }
        ///<summary>
        ///Puede ser nula 
        ///</summary>
        public string UMLGOBE { get; set; }
        ///<summary>
        ///Puede ser nula 
        ///</summary>
        public string UMCHA { get; set; }
        ///<summary>
        ///Puede ser nula 
        ///</summary>
        public string GRUND { get; set; }
        ///<summary>
        ///Puede ser nula 
        ///</summary>
        public string GSBER { get; set; }
        ///<summary>
        ///Puede ser nula 
        ///</summary>
        public Nullable<decimal> MENGE_I { get; set; }
        ///<summary>
        ///Puede ser nula 
        ///</summary>
        public string ZLDAT_I { get; set; } 
        ///<summary>
        ///Puede ser nula 
        ///</summary>
        public Nullable<decimal> MENGE_F { get; set; }
       ///<summary>
       ///Puede ser nula 
       ///</summary>
       public string ZLDAT_F { get; set; }
        ///<summary>
        ///Puede ser nula 
        ///</summary>
        public string MEINS_S { get; set; }
   }
}
