using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Converter.Service.Exceptions
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    public sealed class FileNotFoundException :
       Exception
    {
        public FileNotFoundException()
        {
        }

        private FileNotFoundException(
            SerializationInfo info,
            StreamingContext context) :
            base(info, context)
        {
        }

        public FileNotFoundException(
            string message)
            : base(message)
        {
        }

        public FileNotFoundException(
            string message,
            Exception innerException) :
            base(message, innerException)
        {
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context) =>
            base.GetObjectData(info, context);
    }
}