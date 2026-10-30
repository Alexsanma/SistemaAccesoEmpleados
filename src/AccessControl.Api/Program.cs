using System.Reflection;
using System.Text.Json.Serialization;
using AccessControl.Api.Responses;
using AccessControl.Application;
using AccessControl.Infrastructure;
using AccessControl.Infrastructure.Persistence;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services
    .AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true)
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
    .ConfigureApiBehaviorOptions(options =>
        options.InvalidModelStateResponseFactory = ModelStateResponseFactory.Create);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "SGCAE - Sistema de Gestión y Control de Acceso de Empleados",
        Version = "v1"
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));
});

var app = builder.Build();

app.UseExceptionHandler(errorApp => errorApp.Run(async context =>
{
    var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
    app.Logger.LogError(exception, "Unhandled exception for {Path}", context.Request.Path);

    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
    await context.Response.WriteAsJsonAsync(ApiResponse<object>.Fail(
        [new ApiError("Unexpected", "Ocurrió un error inesperado al procesar la solicitud.", "Unexpected")]));
}));

if (app.Configuration.GetValue("Database:ApplyMigrationsOnStartup", true))
    await app.Services.InitializeDatabaseAsync();

app.UseSwagger();
app.UseSwaggerUI(options => options.DocumentTitle = "SGCAE API");

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

public partial class Program;
