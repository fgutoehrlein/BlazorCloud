using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorCloudCore.Logic.String
{
    public class StringFormater: IDisposable
    {
        private bool isDisposed;
        // Dispose() calls Dispose(true)
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed) return;

            isDisposed = true;
        }
        public string ConvertFormattedStringToHtml(string formattedString)
        {
            StringBuilder sb = new StringBuilder(formattedString);

            //Replace Newline
            sb.Replace("\n", "<br/>");
            return sb.ToString();
        }
    }
}
