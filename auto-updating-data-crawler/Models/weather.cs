using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace auto_updating_data_crawler.Models
{
    public class Weather
    {
        public string Date { get; set; }
        public string LargeTemp { get; set; }
        public string SmallTemp { get; set; }
        public string TextTemp { get; set; }
    }
}
