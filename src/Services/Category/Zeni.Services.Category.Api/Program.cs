using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Host.AddZeniLogging(builder.Configuration);
builder.Services.AddControllers(config =>
{
    config.Filters.Add(new ZeniServiceLoggingActionFilter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Host.UseZeniRegisterServices();

builder.Services.AddDbContext<CategoryDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));

}).AddUnitOfWork<CategoryDbContext>();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
{
    options.Authority = "http://localhost:5000";
    options.TokenValidationParameters.ValidateAudience = false;
    options.RequireHttpsMetadata = false;
});
builder.Services.AddAuthorization(options =>
    options.AddPolicy("ApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "zeni");
    })
);
//.AddAuthentication(options =>
//{
//    options.DefaultScheme = "cookies";
//    options.DefaultChallengeScheme = "oidc";
//})
//.AddCookie("cookies")
//.AddOpenIdConnect("oidc", options =>
//{
//    options.Authority = "http://localhost:5000";
//    options.RequireHttpsMetadata = false;
//    options.ClientSecret = "secret";
//    options.ClientId = "zeni.services.category";
//    options.MapInboundClaims = false;
//    options.SaveTokens = true;
//    options.Scope.Add("zeni");
//});

//builder.Services.AddAuthorization(opt =>
//{
//    opt.AddPolicy("Admin", policy =>
//    {

//        policy.RequireClaim("scope", "zeni");
//    });
//});


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Zeni.Service.Category", Version = "v1" });
    c.AddSecurityDefinition("Oauth2", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.OAuth2,
        In = ParameterLocation.Header,
        Flows = new OpenApiOAuthFlows
        {
            ClientCredentials = new OpenApiOAuthFlow
            {
                TokenUrl = new Uri("http://localhost:5000/connect/token"),
                Scopes = new Dictionary<string, string>
                {
                    {"zeni", "Zeni Server"}
                }
            }
        },
        BearerFormat = "Bearer <token>"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Oauth2",
                },
                Scheme = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>{"zeni"}
        }
    });
});
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.UseSwaggerUI(c =>
    //{
    //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Category Api v1");
    //    c.OAuthClientId("zeni.services.category");
    //    c.OAuthClientSecret("secret");
    //});
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseAllElasticApm(builder.Configuration);
app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials()); // allow credentials

app.Run();
