using websimulador.Models;
using Supabase;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace websimulador.Services;

public class TemperaturaService
{
    public static async Task<List<Temperatura>> GetTemperaturasAsync()
    {
        var client = await SupabaseService.GetClient();

        var response = await client.From<Temperatura>().Get();
        return response.Models;
    }
}
