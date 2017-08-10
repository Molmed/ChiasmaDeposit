using System;
using System.Collections.Specialized;

namespace Molmed.ChiasmaDep.Data.Exception
{
    public class BarCodeException : DataException
    {
        public BarCodeException(String message)
            : base(message)
        {
        }

        public BarCodeException(String message, StringCollection barCodes)
            : base(message, barCodes)
        {
        }
    }
}
