

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
