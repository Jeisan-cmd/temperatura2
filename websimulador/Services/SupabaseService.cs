using Supabase;
using System.Threading.Tasks;

namespace websimulador.Services;

public class SupabaseService
{
    private static Supabase.Client? _client;

    public static async Task<Supabase.Client> GetClient()
    {
        if (_client == null)
        {
            var url = "https://wqnlhysukykeepwulyid.supabase.co";  // 🔑 Reemplazar
            var key = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6IndxbmxoeXN1a3lrZWVwd3VseWlkIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NTkzNzgwNzAsImV4cCI6MjA3NDk1NDA3MH0.dc0Zs-1txJQTXm2Bk0VEkxKTfu1TyxZK3ByY8GR7nYk";                       // 🔑 Reemplazar

            var options = new SupabaseOptions
            {
                AutoRefreshToken = true,
                AutoConnectRealtime = true
            };

            _client = new Supabase.Client(url, key, options);
            await _client.InitializeAsync();
        }

        return _client;
    }
}
