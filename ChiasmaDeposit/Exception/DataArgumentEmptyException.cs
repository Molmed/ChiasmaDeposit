using System;

namespace Molmed.ChiasmaDep.Data.Exception
{
    public class DataArgumentEmptyException : DataArgumentException
    {
        String MyParameterName;

        public DataArgumentEmptyException(String parameterName)
            : base(parameterName)
        {
            MyParameterName = parameterName;
        }

        public String GetParameterName()
        {
            return MyParameterName;
        }

        public override string Message
        {
            get
            {
                return GetMessageBase() + "must be assigned a non-empty value.";
            }
        }
    }
}
