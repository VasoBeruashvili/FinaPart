using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinaPart.Models
{
    [Table("Contragents", Schema = "book")]
    public class Contragents
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }
        public string name { get; set; }
        public string short_name { get; set; }
        public int? acc_use { get; set; }
        public string path { get; set; }
        public string account { get; set; }
        public string account2 { get; set; }
        public double? tax { get; set; }
        public double? min_tax { get; set; }
        public int? country_id { get; set; }
        public int? vat_type { get; set; }
        public int? group_id { get; set; }
        public string code { get; set; }
        public int? type { get; set; }
        public string address { get; set; }

        [Column("e_mail")]
        public string email { get; set; }

        [Column("tel")]
        public string phone { get; set; }

        public DateTime birth_date { get; set; }
        public int client_id { get; set; }
        public int cons_period { get; set; }
    }
}