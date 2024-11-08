using Converter.Service.Converters.Interfaces;
using Converter.Service.Exceptions;
using Converter.Service.Model;
using Converter.Service.Repositories.Interfaces;
using Converter.Service.Settings;
using Microsoft.Extensions.Options;
using PuppeteerSharp;

namespace Converter.Service.Converters
{
    public class HtmlToPdfConverter : IConverter
    {
        private readonly IFileRepository _fileRepository;
        private readonly string _directoryPath;
        private readonly string _dateFormat;

        public HtmlToPdfConverter(
            IFileRepository fileRepository,
            IOptions<ApplicationSettings> settings)
        {
            _fileRepository = fileRepository;
            var settings1 = settings.Value;
            _directoryPath = settings1?.FileSettings?.DirectoryPath ?? "files";
            _dateFormat = settings1?.FileSettings?.DateFormat ?? "yyyy-MM-dd-HH-mm-ss";
        }

        private string PdfFileName(string fileName)
        {
            var date = DateTime.Now.ToString(_dateFormat);
            var originalFileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            return $"{originalFileNameWithoutExtension}_{date}.pdf";
        }

        public async Task<FileModel> ConvertAsync(FileRequestModel inputFile)
        {
            if (inputFile == null || inputFile.Length == 0 || !Path.GetExtension(inputFile.FileName).Equals(".html", StringComparison.CurrentCultureIgnoreCase))
            {
                throw new IncorrectInputFileException("Invalid file. Please select a valid HTML file.");
            }

            var id = Guid.NewGuid();
            var pdfFileName = PdfFileName(inputFile.FileName);

            try
            {
                var pdfFilePath = Path.Combine(_directoryPath, pdfFileName);
                Directory.CreateDirectory(_directoryPath);

                using (var htmlStream = new MemoryStream(inputFile.Content))
                {
                    await ConvertToPdfAsync(htmlStream, pdfFilePath);
                }

                await _fileRepository.AddAsync(id, pdfFileName);
            }
            catch (Exception ex)
            {
                throw new Exception("Error processing file.", ex);
            }

            return new FileModel { Id = id, Name = pdfFileName };
        }

        private static async Task ConvertToPdfAsync(Stream htmlStream, string pdfFilePath)
        {
            await new BrowserFetcher().DownloadAsync();

            var launchOptions = new LaunchOptions
            {
                Headless = true,
                Args = [
                    "--disable-gpu",
                    "--disable-dev-shm-usage",
                    "--disable-setuid-sandbox",
                    "--no-sandbox"]
            };

            using (var browser = await Puppeteer.LaunchAsync(launchOptions))
            using (var page = await browser.NewPageAsync())
            {
                var htmlContent = new StreamReader(htmlStream).ReadToEnd();

                await page.SetContentAsync(htmlContent);

                await page.PdfAsync(
                    pdfFilePath, new PdfOptions
                    {
                        Format = PuppeteerSharp.Media.PaperFormat.Letter,
                        PageRanges = ""
                    });
            }
        }
    }
}