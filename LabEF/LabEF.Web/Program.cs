using LabEF.DataAccess;
using LabEF.Web.Components;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// TODO 2: registrá SqlContext para que use SQL Server con el connection string
// "DefaultConnection" de appsettings.json

builder.Services.AddScoped<VehiculoRepositorio>();

var app = builder.Build();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
