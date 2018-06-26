using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FinaPart.Models
{
    [Table("ProductCancel", Schema = "doc")]
    public class ProductCancel
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("general_id")]
        public int GeneralId { get; set; }

        [ForeignKey("GeneralId")]
        public GeneralDocs GeneralDocs { get; set; }

        [Column("account_id")]
        public int? AccountId { get; set; }

        [Column("staff_id")]
        public int? StaffId { get; set; }
    }
}