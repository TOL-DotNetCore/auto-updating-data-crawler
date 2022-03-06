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
        [Route("[action]")]
        public async Task<IActionResult> PushWeather()
        {
            await _crawlerService.CrawlAndUploadToS3();
            return Ok();
        }

        [HttpGet()]
        public IActionResult Crawl()
        {
            RecurringJob.AddOrUpdate(() =>_crawlerService.CrawlAndUploadToS3(), Cron.Minutely);
            //Cron 30 minutes "*/30 * * * *"
            return Ok("Succesfully");
        }
    }
}
