using Converter.Service.Converters;
using System.ComponentModel;

namespace Converter.Service.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));

            return attribute?.Description ?? value.ToString();
        }

        public static ConverterType ToConverterType(this string fileExtension)
        {
            foreach (ConverterType converterType in Enum.GetValues(typeof(ConverterType)))
            {
                var field = converterType.GetType().GetField(converterType.ToString());
                var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));

                if (attribute?.Description == fileExtension)
                {
                    return converterType;
                }
            }

            throw new ArgumentException($"Unknown file extension: {fileExtension}");
        }
    }
}