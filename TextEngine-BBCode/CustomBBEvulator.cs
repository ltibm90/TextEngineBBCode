using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEngine.Evulator;
using TextEngine.Text;

namespace TextEngine_BBCode
{
    public class CustomBBEvulator : BaseEvulator
    {
        public override TextEvulateResult Render(TextElement tag, object vars)
        {
            return new TextEvulateResult()
            {
                Result = TextEvulateResultEnum.EVULATE_DEPTHSCAN,
                TextContent = "<div style='text-align: center'>"
            };
        }
        public override void RenderFinish(TextElement tag, object vars, TextEvulateResult latestResult)
        {

            latestResult.TextContent += "</div>";
            base.RenderFinish(tag, vars, latestResult);
        }
    }
}
