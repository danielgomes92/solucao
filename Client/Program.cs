using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;

namespace Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // descobrir os endpoints a partir dos metadados do IdentityServer
            var client = new HttpClient();
            var disco = 
                await client.GetDiscoveryDocumentAsync("https://localhost:5001");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            //request token
            var tokenResponse =
                await client.RequestClientCredentialsTokenAsync
                (
                    new ClientCredentialsTokenRequest
                    {
                        Address = disco.TokenEndpoint,
                        ClientId = "client",
                        ClientSecret = "secret",
                        Scope = "api1"
                    });
            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error); //cabeçalho chamado authorization
                return;
            }
            Console.WriteLine(tokenResponse.Json);

            //novo cliente http para chamar a API
            var apiClient = new HttpClient();

            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var response = await apiClient.GetAsync("https://localhost:5000/identity");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }

            Console.ReadKey();
        }
    }
}
