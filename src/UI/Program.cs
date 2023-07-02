using Fluxor;
using Grpc.Gateway.Service;
using MudBlazor.Services;
using Shared;

var builder = WebApplication.CreateBuilder(args);
var isDevelopment = builder.Environment.IsDevelopment();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();

// Add Fluxor for state management
builder.Services.AddFluxor(options =>
{
    options.ScanAssemblies(typeof(Program).Assembly);
    
    if (isDevelopment)
    {
        options.UseReduxDevTools();
    }
});

// Add services for Bus
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

builder.Services.AddScoped<IBus,RemoteableBus>();
builder.Services.AddScoped<GatewayPublisher>();

builder.Services.AddGrpcClient<Gateway.GatewayClient>(options =>
{
    var url = builder.Configuration.GetValue<string>("GatewayUrl");
    options.Address = new Uri(url);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!isDevelopment)
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
