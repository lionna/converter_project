using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Converter.Service.Exceptions
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    public sealed class IncorrectInputFileException :
        Exception
    {
        public IncorrectInputFileException()
        {
        }

        private IncorrectInputFileException(
            SerializationInfo info,
            StreamingContext context) :
            base(info, context)
        {
        }

        public IncorrectInputFileException(
            string message)
            : base(message)
        {
        }

        public IncorrectInputFileException(
            string message,
            Exception innerException) :
            base(message, innerException)
        {
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context) =>
            base.GetObjectData(info, context);
    }
}