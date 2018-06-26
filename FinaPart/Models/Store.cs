using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FinaPart.Models
{
    [Table("Stores", Schema = "book")]
    public class Store
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("group_id")]
        public int? GroupId { get; set; }

        [Column("path")]
        public string Path { get; set; }

        [Column("address")]
        public string Address { get; set; }

        [Column("name")]
        public string Name { get; set; }
    }
}