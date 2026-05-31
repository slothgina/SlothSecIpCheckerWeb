var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();   // ⭐ REQUIRED ⭐

builder.Services.AddHttpClient("ipchecker", client =>
{
    client.BaseAddress = new Uri("https://api.abuseipdb.com/api/v2/");
    client.DefaultRequestHeaders.Add("Key", builder.Configuration["AbuseIpDbApiKey"]);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


