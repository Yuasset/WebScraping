using Nest;

var builder = WebApplication.CreateBuilder(args);


var settings = new ConnectionSettings(new Uri("https://localhost:9200"))
    .CertificateFingerprint("a93d3acb2605fe574bf0fbaf8953b74875269371ad3bf716f4394b9959f23753")
    .BasicAuthentication("elastic", "123456")
    .DefaultIndex("news");
var client = new ElasticClient(settings);


// Add services to the container.
builder.Services.AddControllersWithViews();

//DI
builder.Services.AddSingleton<IElasticClient>(client);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
