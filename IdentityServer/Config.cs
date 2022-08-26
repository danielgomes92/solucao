// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            { 
                new IdentityResources.OpenId()
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[] //listas de API e listas de Clients que podem acessar
            {
                new ApiScope("api1", "Minha API")
            };

        public static IEnumerable<Client> Clients =>
            new Client[] 
            {
                new Client
                {
                    ClientId = "client",
                    // tipo de autenticação possíveis
                    AllowedGrantTypes = GrantTypes.ClientCredentials, //se o cliente tem credencial, tem acesso ao app
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256()) // padrão que ajuda na segurança do segredo
                    },
                    // escopos aos quais os clientes tem acesso
                    AllowedScopes = {"api1"}
                }
            };
    }
}