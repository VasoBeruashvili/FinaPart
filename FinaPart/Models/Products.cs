using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinaPart.Models
{
    [Table("Products", Schema = "book")]
    public class Products
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("path")]
        public string Path { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("unit_id")]
        public int? UnitId { get; set; }

        [Column("code")]
        public string Code { get; set; }

        [Column("group_id")]
        public int? GroupId { get; set; }
    }
}