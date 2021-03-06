using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEngine.Text;

namespace TextEngine_BBCode
{
    public class BBCodeEvulator
    {
        private Dictionary<string, BBCodeInfo> BBCodes { get; set; }
        private TextEvulator evulator;
        /// <summary>
        /// Birden fazla taga bir öğreyi bbkoda bağlar
        /// </summary>
        /// <param name="bbcodes">BBKod dizisi</param>
        /// <param name="info">BBKod bilgisi</param>
        public void SetMultipleTag(string[] bbcodes, BBCodeInfo info)
        {
            for (int i = 0; i < bbcodes.Length; i++)
            {
                this.SetTag(bbcodes[i], info);
            }
        }
        /// <summary>
        /// Bir tagı bbkoda bağlar
        /// </summary>
        /// <param name="bbcode">BBKod</param>
        /// <param name="info">BBKod bilgisi</param>
        public void SetTag(string bbcode, BBCodeInfo info)
        {
            this.BBCodes[bbcode] = info;
        }
        public BBCodeEvulator()
        {
            //Dictionary aramasında büyük küçük duyarsız olacak.
            this.BBCodes = new Dictionary<string, BBCodeInfo>(StringComparer.OrdinalIgnoreCase);
            evulator = new TextEvulator();
            //Mevcut evulatör tipleri ve tag ayarlamaları silindi.
            evulator.EvulatorTypes.Clear();
            evulator.EvulatorTypes.Param = null;
            evulator.TagInfos.Clear();
            //Mevcut sınıfımız evulator ile birlikte gönderildi.
            evulator.CustomDataSingle = this;

            //Tüm tag açılışda doğrudan kapatma eylemi devredışı bırakıldı e.g [TEST /] gibi.
            evulator.TagInfos["*"].Flags = TextElementFlags.TEF_DisableLastSlash;
            evulator.EvulatorTypes.GeneralType = typeof(BBCodeGeneralEvulator);
            evulator.LeftTag = '[';
            evulator.RightTag = ']';
            evulator.SurpressError = true;
        }
        /// <summary>
        /// BBKodu belirtilen formata göre değerlendirir.
        /// </summary>
        /// <param name="bbcodetext">Çevirilecek BBKod</param>
        /// <returns></returns>
        public string EvulateBBCodes(string bbcodetext)
        {
            evulator.Text = bbcodetext;
            evulator.Elements.SubElements.Clear();
            evulator.Parse();
            var result = evulator.Elements.EvulateValue();
            return result?.TextContent;
        }
        /// <summary>
        /// Herhangi bir takın belirtilen Tag ile kapatılmasını sağlar.
        /// </summary>
        /// <param name="name">Kaynak</param>
        /// <param name="target">Hedef</param>
        public void SetAlias(string name, string target)
        {
            evulator.Aliasses.Add(name, target);
        }
        /// <summary>
        /// Belirtilen BBKod bilgisini dönderir.
        /// </summary>
        /// <param name="bbcode">BBKod</param>
        /// <returns>Yoksa varsayılan veya null döner</returns>
        public BBCodeInfo GetTag(string bbcode)
        {
            BBCodeInfo info = null;
            if (this.BBCodes.TryGetValue(bbcode, out info)) return info;
            //Eğer yoksa varsayılan tagı ara
            this.BBCodes.TryGetValue("*", out info);
            return info;

        }
    }
}
