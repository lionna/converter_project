using Converter.Service.Converters;
using Converter.Service.Converters.Interfaces;
using Converter.Service.Repositories;
using Converter.Service.Repositories.Interfaces;
using Converter.Service.Services;
using Converter.Service.Services.Interfaces;
using Converter.Service.Settings;
using WebApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.OperationFilter<AddFileUploadParams>();
});

builder.Services.Configure<ApplicationSettings>(builder.Configuration.GetSection("Application"));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:5173")
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddScoped<IFileRepository, FileRepository>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IConverterFactory, ConverterFactory>();
builder.Services.AddScoped<IConverter, HtmlToPdfConverter>();
builder.Services.AddMemoryCache();

var app = builder.Build();

app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();