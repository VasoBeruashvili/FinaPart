using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinaPart.Models
{
    [Table("ProductImages", Schema = "book")]
    public class ProductImages
    {
        [Key]
        [Column("id")]
        [JsonProperty("id")]
        public int Id { get; set; }

        [Column("product_id")]
        [JsonProperty("productId")]
        public int? ProductId { get; set; }

        [Column("img")]
        [JsonProperty("img")]
        public byte[] Img { get; set; }
    }
}