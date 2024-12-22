using Nest;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Elasticsearch baðlantý yapýlandýrmasý (Elastic.Clients.Elasticsearch DI null getiriyor web üzerinden baðlanýyor burada null getiriyor araþtýrýlacak) Nest kütüphanesi ile DI yapýlabiliyor.
var node1 = new Uri("http://localhost:9200");
var node2 = new Uri("https://localhost:9200");

var settings = new ConnectionSettings(node2)
    .CertificateFingerprint("a93d3acb2605fe574bf0fbaf8953b74875269371ad3bf716f4394b9959f23753")
    .BasicAuthentication("elastic", "123456")
    .DefaultIndex("news");

var client = new ElasticClient(settings);

// Elasticsearch baðlantýsý kontrol
var response = await client.PingAsync();
if (response.IsValid)
{
    Console.WriteLine("Elasticsearch'e baþarýyla baðlanýldý.");
}
else
{
    Console.WriteLine("Elasticsearch baðlantýsý baþarýsýz.");
    Console.WriteLine($"Hata: {response.DebugInformation}");
}

//DI
builder.Services.AddSingleton<IElasticClient>(client);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

//Defatult route belirle
app.MapGet("/", async context =>
{
    context.Response.Redirect("/Scraping/Get");
});

app.MapControllers();

app.Run();
