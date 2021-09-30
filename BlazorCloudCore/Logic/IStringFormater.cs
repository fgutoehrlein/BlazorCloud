using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorCloudCore.Logic
{
    public interface IStringFormater: IDisposable
    {
        public string ConvertFormattedStringToHtml(string formattedString);
    }
}
