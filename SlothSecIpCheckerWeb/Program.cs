var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// REGISTER YOUR SERVICE (this was missing)
builder.Services.AddScoped<IAbuseIpService, AbuseIpService>();

// Configure HttpClient for AbuseIPDB
builder.Services.AddHttpClient("ipchecker", client =>
{
    client.BaseAddress = new Uri("https://api.abuseipdb.com/api/v2/");

    // Pull API key from environment variable
    var apiKey = Environment.GetEnvironmentVariable("ABUSEIPDB_API_KEY");

    if (!string.IsNullOrEmpty(apiKey))
    {
        client.DefaultRequestHeaders.Add("Key", apiKey);
    }

    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.DefaultRequestHeaders.Add("User-Agent", "SlothSec-IP-Checker");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
