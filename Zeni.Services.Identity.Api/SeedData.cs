using System.Security.Claims;
using IdentityModel;
using Zeni.Services.Identity.Api.Data;
using IdentityServerHost.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Duende.IdentityServer;

namespace Zeni.Services.Identity.Api;

public class SeedData
{
    public const string Admin = "Admin";
    public const string User = "User";
    public const string Editor = "Editor";


    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Email(),
            new IdentityResources.Profile()
        };
    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new ApiScope("zeni","Zeni Server"),
            new ApiScope(name:"read",displayName:"Read Data"),
            new ApiScope(name:"write",displayName:"Write Data"),
            new ApiScope(name:"delete",displayName:"Delete Data")
        };

    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            new Client
            {
                ClientId="client",
                ClientSecrets = {new Secret("secret".Sha256())},
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes ={"read","write","profile"}
            },
            new Client
            {
                ClientId="zeni.services.category",
                ClientSecrets = {new Secret("secret".Sha256())},
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                //RedirectUris = { "https://localhost:5000/signin-oidc" },
                //PostLogoutRedirectUris = { "https://localhost:5000/signout-callback-oidc" },
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Email,
                    IdentityServerConstants.StandardScopes.Profile,
                    "zeni"
                }

            }
        };


    public static void EnsureSeedData(WebApplication app)
    {
        using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ZeniIdentityDbContext>();
            var percontext = scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>();
            var concontext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
            context.Database.Migrate();
            percontext.Database.Migrate();
            concontext.Database.Migrate();
            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ZeniUser>>();
            var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();


            if (!concontext.Clients.Any())
            {
                foreach (var client in SeedData.Clients)
                {
                    concontext.Clients.Add(client.ToEntity());

                }
                concontext.SaveChanges();
            }
            if (!concontext.IdentityResources.Any())
            {
                foreach (var resource in SeedData.IdentityResources)
                {
                    concontext.IdentityResources.Add(resource.ToEntity());
                }
                concontext.SaveChanges();
            }

            if (!concontext.ApiScopes.Any())
            {
                foreach (var resource in SeedData.ApiScopes)
                {
                    concontext.ApiScopes.Add(resource.ToEntity());
                }
                concontext.SaveChanges();
            }
      

            if (roleMgr.FindByNameAsync(SeedData.Admin).Result == null)
            {
                roleMgr.CreateAsync(new IdentityRole(SeedData.Admin)).GetAwaiter().GetResult();
                roleMgr.CreateAsync(new IdentityRole(SeedData.Editor)).GetAwaiter().GetResult();
                roleMgr.CreateAsync(new IdentityRole(SeedData.User)).GetAwaiter().GetResult();
            }

            var erdem = userMgr.FindByNameAsync("erdemkayagentr").Result;
            if (erdem == null)
            {
                erdem = new ZeniUser
                {
                    UserName = "erdemkayagentr",
                    Email = "mail@erdemkaya.gen.tr",
                    EmailConfirmed = true,
                    FirstName = "Erdem",
                    LastName = "Kaya"
                };
                var result = userMgr.CreateAsync(erdem, "Pass123$").GetAwaiter().GetResult();
                userMgr.AddToRoleAsync(erdem, SeedData.Admin).GetAwaiter().GetResult();
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userMgr.AddClaimsAsync(erdem, new Claim[]{
                            new Claim(JwtClaimTypes.Name, erdem.FirstName + " "+erdem.LastName),
                            new Claim(JwtClaimTypes.GivenName, erdem.FirstName),
                            new Claim(JwtClaimTypes.FamilyName, erdem.LastName),
                            new Claim(JwtClaimTypes.Role, SeedData.Admin),
                            new Claim(JwtClaimTypes.WebSite, "http://erdemkaya.gen.tr"),
                        }).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
                Log.Debug("erdem created");
            }
            else
            {
                Log.Debug("erdem already exists");
            }
        }
    }
}
