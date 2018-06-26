using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FinaPart.Models
{
    [Table("Companies", Schema = "book")]
    public class Companies
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        
        [Column("name")]
        public string Name { get; set; }

        [Column("code")]
        public string Code { get; set; }

        [Column("address")]
        public string Address { get; set; }

        [Column("info")]
        public string Info { get; set; }

        [Column("vat")]
        public bool? Vat { get; set; }

        [Column("tdate")]
        public DateTime? Tdate { get; set; }
    }
}