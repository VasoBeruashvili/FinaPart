using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinaPart.Models
{
    [Table("GroupProducts", Schema = "book")]
    public class GroupProduct
    {
        [Key]
        [Column("id")]
        [JsonProperty("id")]
        public int Id { get; set; }

        [Column("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [Column("path")]
        public string Path { get; set; }

        [Column("account")]
        public string Account { get; set; }
    }
}