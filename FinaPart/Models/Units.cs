using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FinaPart.Models
{
    [Table("Units", Schema = "book")]
    public class Units
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("full_name")]
        public string FullName { get; set; }

        [Column("rs_id")]
        public int? RsId { get; set; }
    }
}