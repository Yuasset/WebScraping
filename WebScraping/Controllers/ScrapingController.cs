using Microsoft.AspNetCore.Mvc;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using System.Net;
using WebScraping.Models;
using Nest;

namespace WebScraping.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScrapingController : ControllerBase
    {

        private readonly IElasticClient elasticClient;

        public ScrapingController(IElasticClient elasticClient)
        {
            this.elasticClient = elasticClient;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            var url = "https://www.sozcu.com.tr/";
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(url); //URL adresine istek yolla html kaynak kodunu al.

            IList<HtmlNode> nodes = doc.QuerySelectorAll("div.container").QuerySelectorAll("section.mb-0")[0].QuerySelectorAll("div.row div.col-lg-4 div.news-card");

            var data = nodes.Select((node) =>
            {
                var image = DeCodingData(node.QuerySelector("a:nth-child(1) picture img").GetAttributeValue("src", "")).Trim();
                var name = DeCodingData(node.QuerySelector("a:nth-child(2)").InnerText).Trim();
                return new New
                {
                    name = name,
                    image = image
                };
            }).ToList();

            // Index içindeki belge sayýsýný kontrol et
            var countResponse = await elasticClient.CountAsync<New>(x => x.Index("news"));
            if (countResponse.IsValid && countResponse.Count > 0)
            {
                // Eðer içerik varsa sil
                var deleteResponse = await elasticClient.DeleteByQueryAsync<New>(x => x
                    .Index("news")
                    .Query(q => q.MatchAll()) // Tüm belgeleri seç
                );
            }

            // Elasticsearch json formatýnda verileri kaydet
            foreach (var item in data)
            {
                var response = await elasticClient.IndexAsync(item, x => x.Index("news"));
                if (!response.IsValid)
                {
                    Console.WriteLine($"Hata: {response.DebugInformation}");
                }
            }

            return Ok(data);
        }

        private String DeCodingData(String encodeData)
        {
            var decodedData = WebUtility.HtmlDecode(encodeData);
            return decodedData;
        }
    }
}
