using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinaPart.Models
{
    public class ProductViewModel
    {
        public int ID { get; set; }
        
        public string Path { get; set; }
        
        public string Name { get; set; }
        
        public int? UnitId { get; set; }
        
        public string Code { get; set; }
        
        public int? GroupId { get; set; }
        




        public double? Price { get; set; }
        
        [JsonProperty("currencyType")]
        public int? CurrencyType { get; set; }
        
        [JsonProperty("currency")]
        public string Currency { get; set; }
        
        [JsonProperty("barcode")]
        public string Barcode { get; set; }
        
        [JsonProperty("unitName")]
        public string UnitName { get; set; }
        
        [JsonProperty("groupName")]
        public string GroupName { get; set; }

        
        [JsonProperty("productImages")]
        public List<ProductImages> ProductImages { get; set; }
    }
}
