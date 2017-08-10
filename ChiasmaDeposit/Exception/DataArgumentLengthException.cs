using System;

namespace Molmed.ChiasmaDep.Data.Exception
{
    public class DataArgumentLengthException : DataArgumentException
    {
        private Int32 MyMaxLength;

        public DataArgumentLengthException(String parameterName,
                                                                                      Int32 maxLength)
            : base(parameterName)
        {
            MyMaxLength = maxLength;
        }

        public override string Message
        {
            get
            {
                return GetMessageBase() + "is to long." + Environment.NewLine +
                             "Max length is " + MyMaxLength.ToString() + ".";
            }
        }
    }
}
