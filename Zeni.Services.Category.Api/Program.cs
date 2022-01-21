

using Serilog;
using Serilog.AspNetCore;
using Serilog.Sinks.Elasticsearch;
using Elastic.Apm.AspNetCore;


var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Application", "Zeni.Services.Category")
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Warning)
    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(
        new Uri("http://localhost:9200/"))
    {
        AutoRegisterTemplate = true,
        TemplateName ="serilog-events-teamplate",
        IndexFormat = "Zeni.Services.Category-log-{0:yyyy.MM.dd}"
    })
    .MinimumLevel.Verbose()
    .CreateLogger();

try
{

}
catch (Exception ex)
{

    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut Down Complete");
    Log.CloseAndFlush();
}
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(
        new Uri("http://localhost:9200/"))
{
    AutoRegisterTemplate = true,
    TemplateName = "serilog-events-teamplate",
    IndexFormat = "Zeni.Services.Category-log-{0:yyyy.MM.dd}"
}).ReadFrom.Configuration(ctx.Configuration));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSerilogRequestLogging();

app.UseElasticApm(app.Configuration);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
