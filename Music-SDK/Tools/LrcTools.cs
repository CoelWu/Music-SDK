using System;
using System.Collections.Generic;
using System.Text;

namespace Music.SDK.Tools
{
    public class LrcTools
    {
        public string FormatTime(double minutes)
        {
            TimeSpan span = TimeSpan.FromSeconds(minutes);
            return span.ToString(@"mm\:ss\.ff");
        }
    }
}
