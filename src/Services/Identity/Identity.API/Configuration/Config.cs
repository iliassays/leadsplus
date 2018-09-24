using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace LeadsPlus.Services.Identity.API.Configuration
{
    public class Config
    {
        // ApiResources define the apis in your system
        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>
            {
                //new ApiResource("cloudmailinwebhook", "Cloudmailin Webhook"),
                new ApiResource("contact", "Contact Service"),
                new ApiResource("agent", "Agent Service"),
                new ApiResource("inqueryhistory", "Inquery History Service"),
                //new ApiResource("autorespondar", "Autorespondar Service"),
                //new ApiResource("smtpservice", "Smtp Service"),
            };
        }

        // Identity resources are data like user ID, name, or email address of a user
        // see: http://docs.identityserver.io/en/release/configuration/resources.html
        public static IEnumerable<IdentityResource> GetResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        // client want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients(Dictionary<string,string> clientsUrl)
        {
            return new List<Client>
            {
                // JavaScript Client
                new Client
                {
                    ClientId = "adminportal",
                    ClientName = "leadsplus SPA OpenId Client",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris =           { $"{clientsUrl["AdminPortal"]}/" },
                    RequireConsent = false,
                    PostLogoutRedirectUris = { $"{clientsUrl["AdminPortal"]}/" },
                    AllowedCorsOrigins =     { $"{clientsUrl["AdminPortal"]}" },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "agent",
                        "contact",
                    }
                },
                new Client
                {
                    ClientId = "leadsplusagentswaggerui",
                    ClientName = "Leads Plus Agent Swagger UI",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris = { $"{clientsUrl["AgentApi"]}/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { $"{clientsUrl["AgentApi"]}/swagger/" },

                    AllowedScopes =
                    {
                        "agent"
                    }
                },
                new Client
                {
                    ClientId = "contactswaggerui",
                    ClientName = "Leads Plus Contact Swagger UI",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris = { $"{clientsUrl["ContactApi"]}/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { $"{clientsUrl["ContactApi"]}/swagger/" },

                    AllowedScopes =
                    {
                        "contact"
                    }
                }
            };
        }
    }
}