using Temapp.Services;
using DeviceModel = Temapp.Models.Device;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Temapp;

public partial class MainPage : ContentPage
{
	private readonly SupabaseService _supa;
	private List<DeviceModel> _devices = new();

	public MainPage()
	{
		InitializeComponent();
		_supa = App.Services?.GetRequiredService<SupabaseService>() ?? throw new InvalidOperationException("SupabaseService no está registrado en App.Services");
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		StatusLabel.Text = "Cargando dispositivos...";
		try
		{
			// Ensure client initialized
			await _supa.InicializarAsync();

			_devices = await _supa.ObtenerDispositivosAsync();
			DevicesPicker.ItemsSource = _devices.Select(d => d.Name).ToList();

			if (_devices.Count == 0)
				StatusLabel.Text = "No hay dispositivos en Supabase.";
			else
				StatusLabel.Text = $"{_devices.Count} dispositivos cargados.";
		}
		catch (Exception ex)
		{
			StatusLabel.Text = "Error cargando dispositivos: " + ex.Message;
		}
	}

	private async void OnSendClicked(object? sender, EventArgs e)
	{
		StatusLabel.Text = "Enviando...";

		try
		{
			if (!decimal.TryParse(TempEntry.Text, out var temp))
			{
				StatusLabel.Text = "Temperatura inválida.";
				return;
			}

			if (!decimal.TryParse(HumEntry.Text, out var hum))
			{
				StatusLabel.Text = "Humedad inválida.";
				return;
			}

			Guid deviceId;
			if (DevicesPicker.SelectedIndex >= 0 && DevicesPicker.SelectedIndex < _devices.Count)
			{
				deviceId = _devices[DevicesPicker.SelectedIndex].Id;
				await _supa.InsertarDatoAsync(deviceId, temp, hum);
			}
			else
			{
				// If no device selected, insert into first device via helper
				await _supa.InsertarDatoEnPrimerDispositivoAsync(temp, hum);
			}

			StatusLabel.Text = "Dato enviado correctamente";
			TempEntry.Text = string.Empty;
			HumEntry.Text = string.Empty;
		}
		catch (Exception ex)
		{
			StatusLabel.Text = "Error al enviar: " + ex.Message;
		}
	}
}
