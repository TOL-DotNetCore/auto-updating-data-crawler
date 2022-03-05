using auto_updating_data_crawler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace auto_updating_data_crawler.Services
{
    public interface ICrawlerService
    {
        Task<string> GetHtmlOfPage(string url);
        Task<List<Weather>> GetWeathers();
        Task<byte[]> ExportToExcel(List<Weather> LstWeather);
        Task CrawlAndUploadToS3();
    }
}
