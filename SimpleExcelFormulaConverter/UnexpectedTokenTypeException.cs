using System;
using System.Runtime.Serialization;

namespace SimpleExcelFormulaConverter
{
    [Serializable]
    public class UnexpectedTokenTypeException : Exception
    {
        public UnexpectedTokenTypeException()
        {
        }

        public UnexpectedTokenTypeException(string message) : base(message)
        {
        }

        public UnexpectedTokenTypeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnexpectedTokenTypeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}