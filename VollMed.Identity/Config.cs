using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;
using System.Net.NetworkInformation;

namespace VollMed.Identity
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            [
            
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
                        
            ];

        public static IEnumerable<ApiScope> ApiScopes =>
            [
                //Definimos os escopos // para teste inicial para este deve ser criado.
                new ApiScope(name: "VollMed.Web.Scope", displayName: "VollMed.Web.Scope"),

                //Depois que testamos a autenticação incluímos as apis
                new ApiScope(name: "Consultas.ServiceAPI.Scope", displayName: "Consultas.ServiceAPI.Scope"),
                new ApiScope(name: "Medicos.ServiceAPI.Scope", displayName: "Medicos.ServiceAPI.Scope"),
                new ApiScope(name: "Pacientes.ServiceAPI.Scope", displayName: "Pacientes.ServiceAPI.Scope")

            ];

        public static IEnumerable<ApiResource> ApiResources =>
            [
                new ApiResource(name:"Consultas.ServiceAPI.Resource", displayName: "Consultas.ServiceAPI.Resource" )
                {
                    Scopes = { "Consultas.ServiceAPI.Scope" }
                },
                new ApiResource(name:"Medicos.ServiceAPI.Resource", displayName: "Medicos.ServiceAPI.Resource" )
                {
                    Scopes = { "Medicos.ServiceAPI.Scope" }
                },
                new ApiResource(name:"Pacientes.ServiceAPI", displayName: "Pacientes.ServiceAPI.Resource" )
                {
                    Scopes = { "Pacientes.ServiceAPI.Scope" }
                }

            ];

        public static IEnumerable<Client> Clients =>
            [

                new Client
                {
                    ClientId = "VollMed.Web.Id",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    
                    AllowedGrantTypes = GrantTypes.Code,
                                    
                    // where to redirect to after login
                    RedirectUris = { "https://localhost:7236/signin-oidc" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "https://localhost:7236/signout-callback-oidc" },

                    AllowOfflineAccess = true,
                    AllowedScopes =
                    [
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        //Na primeira etapa apenas este scope
                        "VollMed.Web.Scope", 

                        //depois que testamos a autenticação incluímos estes scopes
                        "Consultas.ServiceAPI.Scope",
                        "Medicos.ServiceAPI.Scope",
                        "Pacientes.ServiceAPI.Scope"
                    ]
                }

                #region original
            //// m2m client credentials flow client
            //new Client
            //{
            //    ClientId = "m2m.client",
            //    ClientName = "Client Credentials Client",

            //    AllowedGrantTypes = GrantTypes.ClientCredentials,
            //    ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

            //    AllowedScopes = { "scope1" }
            //},

            //// interactive client using code flow + pkce
            //new Client
            //{
            //    ClientId = "interactive",
            //    ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

            //    AllowedGrantTypes = GrantTypes.Code,

            //    RedirectUris = { "https://localhost:44300/signin-oidc" },
            //    FrontChannelLogoutUri = "https://localhost:44300/signout-oidc",
            //    PostLogoutRedirectUris = { "https://localhost:44300/signout-callback-oidc" },

            //    AllowOfflineAccess = true,
            //    AllowedScopes = { "openid", "profile", "scope2" }
            //},
            #endregion
            ];
    }
}
