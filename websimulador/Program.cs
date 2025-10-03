using websimulador.Services;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Puedes inyectar el servicio si lo necesitas
builder.Services.AddSingleton<TemperaturaService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
