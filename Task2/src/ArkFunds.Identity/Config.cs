using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;

namespace ArkFunds.Identity;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("ArkFundsAPI")
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new()
            {
                ClientId = "api_swagger",
                ClientName = "Swagger UI for Sample API",
                ClientSecrets = {new Secret("d422a063-ca8f-4104-a3c4-60fc798519c5".Sha256())},

                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris = {"https://localhost:7108/swagger/oauth2-redirect.html"},
                AllowedCorsOrigins = {"https://localhost:7108"},
                AllowedScopes = new List<string>
                {
                    "ArkFundsAPI"
                }
            },
            new()
            {
                ClientId = "blazor_web_app",
                ClientName = "Blazor Web App",
                ClientSecrets = {new Secret("d422a063-ca8f-4104-a3c4-60fc798519c5".Sha256())},
                AllowedGrantTypes =  new[] { GrantType.AuthorizationCode, GrantType.ResourceOwnerPassword },
                
                AllowOfflineAccess = true,
                RedirectUris = { "https://localhost:7164", "https://localhost:7164/signin-oidc", "https://localhost:7164/authentication/login-callback" },
                PostLogoutRedirectUris = { "https://localhost:7164", "https://localhost:7164/signout-oidc", "https://localhost:7164/signout-callback-oidc" },
                AllowedCorsOrigins= { "https://localhost:7164" },
                AlwaysSendClientClaims = true,
                AlwaysIncludeUserClaimsInIdToken = true,
                
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "ArkFundsAPI"
                },
                RequireClientSecret = false
            }
        };
}