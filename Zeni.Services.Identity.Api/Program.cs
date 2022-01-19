using Zeni.Services.Identity.Api;
using Serilog;
using Zeni.Services.Identity.Api.Data;
using Microsoft.EntityFrameworkCore;
using IdentityServerHost.Models;
using Microsoft.AspNetCore.Identity;
using System.Reflection;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((ctx, lc) => lc
        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
        .Enrich.FromLogContext()
        .ReadFrom.Configuration(ctx.Configuration));


    // this seeding is only for the template to bootstrap the DB and users.
    // in production you will likely want a different approach.

    builder.Services.AddRazorPages();
    builder.Services.AddDbContext<ZeniIdentityDbContext>((opt) =>
    {
        opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    });
    builder.Services.AddIdentity<ZeniUser, IdentityRole>()
        .AddEntityFrameworkStores<ZeniIdentityDbContext>().AddDefaultTokenProviders();

   

    var identityBuilder = builder.Services.AddIdentityServer(opt =>
    {
        opt.Events.RaiseErrorEvents = true;
        opt.Events.RaiseInformationEvents = true;
        opt.Events.RaiseFailureEvents = true;
        opt.Events.RaiseSuccessEvents = true;
        opt.EmitStaticAudienceClaim = true;

    }).AddAspNetIdentity<ZeniUser>();

    var migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;
    identityBuilder
        .AddDeveloperSigningCredential()
        .AddOperationalStore(opt =>
        {
            opt.ConfigureDbContext = build => build.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
                ,sql=>sql.MigrationsAssembly(migrationsAssembly));
            opt.EnableTokenCleanup = true;
            opt.TokenCleanupInterval = 3600;
        }).AddConfigurationStore(options =>
        {
            options.ConfigureDbContext = b => b.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
                , sql => sql.MigrationsAssembly(migrationsAssembly));
        });



    var app = builder.Build();

    //using(var scope = app.Services.CreateScope())
    //{
    //    var service = scope.ServiceProvider;
    //    var identityContext = service.GetRequiredService<ZeniIdentityDbContext>();
    //    await identityContext.Database.MigrateAsync();
    //}

    Log.Information("Seeding database...");
    SeedData.EnsureSeedData(app);
    Log.Information("Done seeding database. Exiting.");

    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseCookiePolicy(new CookiePolicyOptions
    {
        MinimumSameSitePolicy = SameSiteMode.Lax
    });

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseIdentityServer();
    app.UseAuthorization();
    app.MapRazorPages();
    app.Run();
}
catch (Exception ex) when (ex.GetType().Name is not "StopTheHostException") // https://github.com/dotnet/runtime/issues/60600
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}