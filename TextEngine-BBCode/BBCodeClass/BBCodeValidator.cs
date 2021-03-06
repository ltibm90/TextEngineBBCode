using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEngine_BBCode
{
    public class BBCodeValidator
    {
        /// <summary>
        /// Mevcut bbkod içeriği
        /// </summary>
        public BBCodeInfo BBCode { get; set;}
        /// <summary>
        /// Mevcut tag data bilgisi
        /// </summary>
        public Dictionary<string, object> TagData { get; set; }
        /// <summary>
        /// True ayarlanırsa çıktısını basmaz.
        /// </summary>
        public bool Cancel { get; set; }
    }
}
