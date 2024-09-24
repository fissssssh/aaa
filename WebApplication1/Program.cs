using HoloTestSystem.Api;
using Microsoft.AspNetCore.OData;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebApplication1;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddOData(options =>
{
    options.Count().Select().Expand().OrderBy().SetMaxTop(null);
    options.EnableNoDollarQueryOptions = false;
    options.RouteOptions.EnableKeyInParenthesis = false;
    options.RouteOptions.EnableNonParenthesisForEmptyParameterFunction = true;
    options.RouteOptions.EnableQualifiedOperationCall = false;
    options.RouteOptions.EnableUnqualifiedOperationCall = true;
});
builder.Services.AddApiVersioning().AddOData(options =>
{
    options.ModelBuilder.DefaultModelConfiguration = (builder, apiVersion, routePrefix) =>
    {
        builder.EntitySet<WeatherForecast>("WeatherForecast");
    };
    options.AddRouteComponents("api/v{version:apiVersion}");
}).AddODataApiExplorer(opts =>
{
    opts.GroupNameFormat = "'v'VVV";
    opts.SubstituteApiVersionInUrl = true;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen(opts =>
{
    opts.OperationFilter<SwaggerDefaultValues>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (var description in app.DescribeApiVersions())
        {
            options.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                description.GroupName);
        }
    });
}

app.UseAuthorization();

app.MapControllers();

app.Run();
