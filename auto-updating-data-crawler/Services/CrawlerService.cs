using auto_updating_data_crawler.Models;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace auto_updating_data_crawler.Services
{
    public class CrawlerService : ICrawlerService
    {
        public async Task<byte[]> ExportToExcel(List<Weather> LstWeather)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Users");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Date";
                worksheet.Cell(currentRow, 2).Value = "Large Temp";
                worksheet.Cell(currentRow, 2).Value = "Small Temp";
                foreach (var weather in LstWeather)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = weather.Date;
                    worksheet.Cell(currentRow, 2).Value = weather.LargeTemp;
                    worksheet.Cell(currentRow, 3).Value = weather.SmallTemp;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return content;
                }
            }
        }

        public async Task<string> GetHtmlOfPage(string url)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "C# console program");

            var content = await client.GetStringAsync(url);
            return content;
        }

        public async Task<List<Weather>> GetWeathers()
        {
            string url = "https://kttv.gov.vn/Kttvsite/vi-VN/1/ho-chi-minh-w15.html";
            string html = await GetHtmlOfPage(url);
            var weas = Regex.Matches(html, @"<div class=""item-days-wt"">(.*?)</div></div>", RegexOptions.Singleline);
            List<Weather> Lst_Weather = new List<Weather>();
            foreach(var w in weas)
            {
                string date = Regex.Match(w.ToString(), @"<span>(.*?)</span></div><div class=""icon-days-wt"">", RegexOptions.Singleline).Value.Replace(@"</span></div><div class=""icon-days-wt"">", "").Replace("<span>", "");
                
                string largeTemp = Regex.Match(w.ToString(), @"<span class=""large-temp"">(.*?)</span>", RegexOptions.Singleline).Value.Replace(@"<span class=""large-temp"">", "").Replace(@"</span>", "");
                string smallTemp = Regex.Match(w.ToString(), @"<span class=""small-temp"">(.*?)</span>", RegexOptions.Singleline).Value.Replace(@"<span class=""small-temp"">", "").Replace(@"</span>", "");
                string textTemp = Regex.Match(w.ToString(), @"<div class=""text-temp"">(.*?)</div>", RegexOptions.Singleline).Value.Replace(@"<div class=""text-temp"">", "").Replace(@"</div>", "");


                Weather wea = new Weather {
                    Date = DateTime.Parse(date),
                    LargeTemp = largeTemp,
                    SmallTemp = smallTemp,
                    TextTemp = textTemp
                };
                Lst_Weather.Add(wea);
            }

            return Lst_Weather;
        }
    }
}
