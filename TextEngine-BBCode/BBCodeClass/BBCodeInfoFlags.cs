using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEngine_BBCode
{
    [Flags]
    public enum BBCodeInfoFlags
    {
        /// <summary>
        /// Alt tagların sadece metinlerini dâhil eder.
        /// </summary>
        InnerTextOnly = 1 << 0
    }
}
