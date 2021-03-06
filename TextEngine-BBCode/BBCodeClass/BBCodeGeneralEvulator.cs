using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEngine.Evulator;
using TextEngine.Text;

namespace TextEngine_BBCode
{
    public class BBCodeGeneralEvulator : BaseEvulator
    {
        private BBCodeInfo currentInfo = null;
        private BBCodeEvulator bbCodeEvulator = null;
        private BaseEvulator customEvulatorHandler = null;
        private bool printed = false;


        private Dictionary<string, object> GetDictionary(TextElement tag)

        {
            return new Dictionary<string, object>()
            {
                ["TagAttrib"] = tag.TagAttrib,
                ["TagName"] = tag.ElemName.ToLower()
            };
        }
        
        public override TextEvulateResult Render(TextElement tag, object vars)
        {
            this.bbCodeEvulator = this.Evulator.CustomDataSingle as BBCodeEvulator;
            TextEvulateResult result = new TextEvulateResult()
            {
                Result = TextEvulateResultEnum.EVULATE_DEPTHSCAN
            };
            var dict = this.GetDictionary(tag);
            currentInfo = this.bbCodeEvulator.GetTag(tag.ElemName);
            if(currentInfo == null)
            {
                return result;
            }
            if (currentInfo.CustomEvulator != null)
            {
                customEvulatorHandler = Activator.CreateInstance(currentInfo.CustomEvulator) as BaseEvulator;
                customEvulatorHandler.SetEvulator(this.Evulator);
                result = customEvulatorHandler.Render(tag, vars);

            }
            else if ((currentInfo.Flags & BBCodeInfoFlags.InnerTextOnly) != 0 && currentInfo.Enabled)
            {
                dict["Text"] = tag.InnerText();
                var validateResult = currentInfo.Validate(dict, tag);
                if(validateResult != null && validateResult.Cancel)
                {
                    result.TextContent = null;
                }
                else
                {
                    result.TextContent = currentInfo.TagFormat.Apply(dict);
                }

                result.Result = TextEvulateResultEnum.EVULATE_TEXT;
                printed = true;
            }
            return result;
        }
        public override void RenderFinish(TextElement tag, object vars, TextEvulateResult latestResult)
        {
            //Render kısmında tamamlandıysa devam etmez.
            if(printed)
            {
                base.RenderFinish(tag, vars, latestResult);
                return;
            }
            if (customEvulatorHandler != null)
            {
                customEvulatorHandler.RenderFinish(tag, vars, latestResult);
            }
            else if (currentInfo != null && (currentInfo.Flags & BBCodeInfoFlags.InnerTextOnly) == 0 && currentInfo.Enabled)
            {
                var dict = this.GetDictionary(tag);
                dict["Text"] = latestResult.TextContent;
                var validateResult = currentInfo.Validate(dict, tag);
                if(validateResult != null && validateResult.Cancel)
                {
                    latestResult.TextContent = null;
                }
                else
                {
                    latestResult.TextContent = this.currentInfo.TagFormat.Apply(dict);
                }

            }
            else
            {
                //default 
                latestResult.TextContent = "[" + tag.ElemName + "]" + latestResult.TextContent + "[/" + tag.ElemName + "]";
            }
            base.RenderFinish(tag, vars, latestResult);
        }

    }
}
