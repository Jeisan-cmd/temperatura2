using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Temapp.Models;
using DeviceModel = Temapp.Models.Device;

namespace Temapp.Services
{
    public class SupabaseService
    {
        private readonly HttpClient _http;
        private readonly string _baseUrl;
        private readonly string _apiKey;

        public SupabaseService()
        {
            _baseUrl = "https://wqnlhysukykeepwulyid.supabase.co".TrimEnd('/');
            _apiKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6IndxbmxoeXN1a3lrZWVwd3VseWlkIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NTkzNzgwNzAsImV4cCI6MjA3NDk1NDA3MH0.dc0Zs-1txJQTXm2Bk0VEkxKTfu1TyxZK3ByY8GR7nYk";

            _http = new HttpClient
            {
                BaseAddress = new Uri(_baseUrl)
            };

            _http.DefaultRequestHeaders.Accept.Clear();
            _http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            // Supabase requires the apikey header also set for anonymous access
            _http.DefaultRequestHeaders.Add("apikey", _apiKey);
        }

        public Task InicializarAsync() => Task.CompletedTask;

        private async Task<List<T>> GetListAsync<T>(string path)
        {
            var resp = await _http.GetAsync(path);
            resp.EnsureSuccessStatusCode();
            await using var stream = await resp.Content.ReadAsStreamAsync();
            var items = await JsonSerializer.DeserializeAsync<List<T>>(stream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return items ?? new List<T>();
        }

        private async Task<T?> GetSingleAsync<T>(string path)
        {
            var list = await GetListAsync<T>(path);
            return list.FirstOrDefault();
        }

        // ========== DISPOSITIVOS ==========

        public Task<List<DeviceModel>> ObtenerDispositivosAsync()
            => GetListAsync<DeviceModel>("/rest/v1/devices?select=*" );

        public Task<DeviceModel?> ObtenerDispositivoPorIdAsync(Guid deviceId)
            => GetSingleAsync<DeviceModel>($"/rest/v1/devices?id=eq.{deviceId}&select=*");

        // ========== DATOS DE SENSORES ==========

        public Task<List<SensorData>> ObtenerDatosAsync()
            => GetListAsync<SensorData>("/rest/v1/sensor_data?select=*&order=timestamp.desc");

        public Task<List<SensorData>> ObtenerDatosPorDispositivoAsync(Guid deviceId)
            => GetListAsync<SensorData>($"/rest/v1/sensor_data?device_id=eq.{deviceId}&select=*&order=timestamp.desc");

        public async Task InsertarDatoAsync(Guid deviceId, decimal temperatura, decimal humedad)
        {
            var dato = new
            {
                device_id = deviceId,
                temperature = temperatura,
                humidity = humedad,
                timestamp = DateTime.UtcNow
            };

            var json = JsonSerializer.Serialize(dato);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var resp = await _http.PostAsync("/rest/v1/sensor_data", content);
            resp.EnsureSuccessStatusCode();
        }

        public async Task InsertarDatoEnPrimerDispositivoAsync(decimal temperatura, decimal humedad)
        {
            var dispositivos = await ObtenerDispositivosAsync();
            if (dispositivos.Count > 0)
            {
                await InsertarDatoAsync(dispositivos[0].Id, temperatura, humedad);
            }
            else
            {
                throw new Exception("No hay dispositivos disponibles");
            }
        }
    }
}