using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextEngine_BBCode
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        BBCodeEvulator evulator = new BBCodeEvulator();
        private void Form1_Load(object sender, EventArgs e)
        {

            //BBKod tanımlanmamış taglar için çıktı biçimi
            evulator.SetTag("*", new BBCodeInfo("[{%TagName}]{%Text}[/{%TagName%}]"));

            evulator.SetMultipleTag(new string[] { "b", "i", "u", "s" }, new BBCodeInfo("<{%TagName}>{%Text}</{%TagName}>"));


            //Validator kullanarak aşağıdaki gibi içeriğe müdahele edebilirriz.
            evulator.SetTag("url", new BBCodeInfo("<a href=\"{%TagAttrib}\">{%Text}</url>").SetValidator(
                (validator, tag) =>
                {
                    string attr = validator.TagData["TagAttrib"]?.ToString();
                    if(attr == "@cw")
                    {
                        validator.TagData["TagAttrib"] = "www.cyber-warrior.org";
                    }
                }
                )
            );
            evulator.SetTag("img", new BBCodeInfo("<img>{%Text}</img>", BBCodeInfoFlags.InnerTextOnly));
            evulator.SetTag("size", new BBCodeInfo("<font size=\"{%TagAttrib}\">{%Text}</font>"));
            evulator.SetTag("color", new BBCodeInfo("<font color=\"{%TagAttrib}\">{%Text}</font>"));
            evulator.SetTag("font", new BBCodeInfo("<font face=\"{%TagAttrib}\">{%Text}</font>"));
            evulator.SetTag("center", new BBCodeInfo().SetCustomEvulator(typeof(CustomBBEvulator)));
            //Color, Font ile kapatılabilir.
            evulator.SetAlias("color", "font");
            //Size, Font ile kapatılabilir.
            evulator.SetAlias("size", "font");
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            tbHtmlCode.Text = evulator.EvulateBBCodes(tbBBCode.Text);
        }
    }
}
