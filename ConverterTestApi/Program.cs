
using AppBusinessGeneric.Application.Helpers;
using System.Text.Encodings.Web;
using System.Text.Json;
internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.MapPost("/api/ConvertListFieldToObject", async (Request request) => 
        {
            dynamic dynamicModel =
                ConverterObjectHelper.ConvertFieldToObject(request.Fields, request.SystemObject);
            
            return JsonSerializer.Serialize(
                dynamicModel,
                new JsonSerializerOptions()
                {
                    WriteIndented = true,
                    Converters =
                    {
                        new DynamicModelConverter()
                    },
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                });
        })
        .WithName("ConvertListFieldToObject")
        .WithOpenApi();

        app.Run();
    }
}

record Request
{
    public string SystemObject { get; set; } = "";
    public List<Armanino.Integration.Utilities.Models.Field> Fields { get; set; }
}
