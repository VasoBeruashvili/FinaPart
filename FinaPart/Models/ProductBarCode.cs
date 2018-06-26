using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FinaPart.Models
{
    [Table("ProductBarCodes", Schema = "book")]
    public class ProductBarCode
    {
        [Key]
        [Column("id")]
        [JsonProperty("id")]
        public int Id { get; set; }
        
        [JsonProperty("productId")]
        public int? product_id { get; set; }

        [Column("barcode")]
        [JsonProperty("barcode")]
        public string Barcode { get; set; }
    }
}