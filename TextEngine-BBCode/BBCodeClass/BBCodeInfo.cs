using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEngine.Evulator;
using TextEngine.ParDecoder;

namespace TextEngine_BBCode
{
    public class BBCodeInfo
    {
        public BBCodeInfo()
        {
            this.Enabled = true;
        }
        public BBCodeInfo(string tagText)
        {
            this.Enabled = true;
            this.TagText = tagText;
        }
        public BBCodeInfo(string tagText, BBCodeInfoFlags flags)
        {
            this.Enabled = true;
            this.TagText = tagText;
            this.Flags = flags;
        }
        public BBCodeInfo SetValidator(Action<BBCodeValidator, TextEngine.Text.TextElement> validator)
        {
            this.Validator = validator;
            return this;
        }
        public BBCodeInfo SetCustomEvulator(Type customBBEvulator)
        {
            this.CustomEvulator = customBBEvulator;
            return this;
        }
        /// <summary>
        /// Etkin olup olmadığınıbelirler
        /// </summary>
        public bool Enabled { get; set; }
        public Type CustomEvulator { get; set; }
        public BBCodeInfoFlags Flags { get; set; }
        public Action<BBCodeValidator, TextEngine.Text.TextElement> Validator { get; set; }



        private string tagText;
        public string TagText
        {
            get
            {
                return this.tagText;
            }
            set
            {
                this.tagText = value;
                this.tagformat = null;
            }
        }

        private ParFormat tagformat;
        public ParFormat TagFormat
        {
            get
            {
                if(this.tagformat == null)
                {
                    this.tagformat = new ParFormat(this.TagText);
                    this.tagformat.SurpressError = true;
                }
                return this.tagformat;
            }
        }
        public BBCodeValidator Validate(Dictionary<string, object> data, TextEngine.Text.TextElement tag)
        {
            if (this.Validator == null) return null;
            BBCodeValidator validator = new BBCodeValidator();
            validator.BBCode = this;
            validator.TagData = data;
            this.Validator.Invoke(validator, tag);
            return validator;
            
        }

    }
}
