using Nest;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Elasticsearch bağlantı yapılandırması (Elastic.Clients.Elasticsearch DI null getiriyor web üzerinden bağlanıyor burada null getiriyor araştırılacak) Nest kütüphanesi ile DI yapılabiliyor.
var node1 = new Uri("http://localhost:9200");
var node2 = new Uri("https://localhost:9200");

var settings = new ConnectionSettings(node2)
    .CertificateFingerprint("a93d3acb2605fe574bf0fbaf8953b74875269371ad3bf716f4394b9959f23753")
    .BasicAuthentication("elastic", "123456")
    .DefaultIndex("news");

var client = new ElasticClient(settings);

// Elasticsearch bağlantısı kontrol
var response = await client.PingAsync();
if (response.IsValid)
{
    Console.WriteLine("Elasticsearch'e başarıyla bağlanıldı.");
}
else
{
    Console.WriteLine("Elasticsearch bağlantısı başarısız.");
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
