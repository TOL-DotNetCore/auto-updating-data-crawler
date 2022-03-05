using auto_updating_data_crawler.Services;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace auto_updating_data_crawler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CrawlerController : ControllerBase
    {
        private readonly ICrawlerService _crawlerService;
        public CrawlerController(ICrawlerService crawlerService)
        {
            _crawlerService = crawlerService;
        }


        [HttpGet()]
        public async Task<IActionResult> GetWeather()
        {
            var result = await _crawlerService.GetWeathers();
            return Ok(result);
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult Confirm()
        {
            int timeInSeconds = 30;
            var parentJobId = BackgroundJob.Schedule(() => Console.WriteLine("You asked to be unsubscribed!"), TimeSpan.FromSeconds(timeInSeconds));

            BackgroundJob.ContinueJobWith(parentJobId, () => Console.WriteLine("You were unsubscribed!"));

            return Ok("Confirmation job created!");
        }
    }
}
