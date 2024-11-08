namespace Converter.Service.Converters.Interfaces
{
    public interface IConverterFactory
    {
        IConverter CreateConverter(string fileExtension);
    }
}