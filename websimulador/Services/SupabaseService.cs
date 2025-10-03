using websimulador.Models;

namespace websimulador.Services
{
    public class SupabaseService
    {
        private readonly Supabase.Client _client;

        public SupabaseService()
        {
            _client = new Supabase.Client(
                "https://wqnlhysukykeepwulyid.supabase.co",
                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6IndxbmxoeXN1a3lrZWVwd3VseWlkIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NTkzNzgwNzAsImV4cCI6MjA3NDk1NDA3MH0.dc0Zs-1txJQTXm2Bk0VEkxKTfu1TyxZK3ByY8GR7nYk"
            );
        }

        public Supabase.Client GetClient()
        {
            return _client;
        }

        // ✅ MÉTODO AGREGADO DENTRO DE LA CLASE
        public async Task<List<SensorData>> GetSensorDataAsync()
        {
            var response = await _client
                .From<SensorData>()
                .Get();
            
            return response.Models;
        }
    }
}