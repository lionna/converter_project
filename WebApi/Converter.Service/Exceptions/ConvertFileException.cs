using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Converter.Service.Exceptions
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    public sealed class ConvertFileException :
        Exception
    {
        public ConvertFileException()
        {
        }

        private ConvertFileException(
            SerializationInfo info,
            StreamingContext context) :
            base(info, context)
        {
        }

        public ConvertFileException(
            string message)
            : base(message)
        {
        }

        public ConvertFileException(
            string message,
            Exception innerException) :
            base(message, innerException)
        {
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context) =>
            base.GetObjectData(info, context);
    }
}