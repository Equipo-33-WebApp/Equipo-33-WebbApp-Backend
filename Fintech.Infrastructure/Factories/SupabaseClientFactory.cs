using Microsoft.Extensions.Configuration;
using Supabase;

namespace Fintech.Infrastructure.Factories
{
    public class SupabaseClientFactory
    {
        private readonly IConfiguration _config;

        public SupabaseClientFactory(IConfiguration config)
        {
            _config = config;
        }

        public async Task<Client> CreateClientAsync(string? accessToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
                throw new ArgumentException("El token de acceso es nulo o vacío.", nameof(accessToken));

            var url = _config["SUPABASE_URL"];
            var key = _config["SUPABASE_KEY"];

            var options = new SupabaseOptions
            {
                AutoRefreshToken = false,
                AutoConnectRealtime = false,
                Headers = new Dictionary<string, string>
                {
                    { "Authorization", $"Bearer {accessToken}" }
                }
            };

            var client = new Client(url, key, options);

            await client.InitializeAsync();

            return client;
        }
    }
}