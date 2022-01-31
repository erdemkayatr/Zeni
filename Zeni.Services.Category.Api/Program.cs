using RabbitMQ.Client;
using Serilog;
using Serilog.Formatting.Json;
using Serilog.Sinks.RabbitMQ;
using Serilog.Sinks.RabbitMQ.Sinks.RabbitMQ;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Zeni.Infra.Ioc;
using Zeni.Infra.Logging;
using Zeni.Services.Category.Application;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Host.AddZeniLogging(builder.Configuration);
builder.Services.AddControllers(config=>{
    config.Filters.Add(new ZeniServiceLoggingActionFilter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Host.UseZeniRegisterServices();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
