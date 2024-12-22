using Client.Models;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly IElasticClient elasticClient;

        public HomeController(IElasticClient elasticClient)
        {
            this.elasticClient = elasticClient;
        }

        public async Task<IActionResult> Index()
        {

            var response = await elasticClient.SearchAsync<News>(x => x.Index("news").MatchAll().Size(10000));

            return View(response.Documents.ToList());
        }
    }
}
