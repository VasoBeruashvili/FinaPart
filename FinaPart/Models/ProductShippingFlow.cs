using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinaPart.Models
{
    [Table("ProductShippingFlow", Schema = "doc")]
    public class ProductShippingFlow
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("general_id")]
        public int? GeneralId { get; set; }

        [ForeignKey("GeneralId")]
        public GeneralDocs GeneralDoc { get; set; }

        [Column("product_id")]
        public int? ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Products Product { get; set; }

        [Column("unit_id")]
        public int? UnitId { get; set; }

        [ForeignKey("UnitId")]
        public Units Unit { get; set; }

        [Column("rest")]
        public double? Rest { get; set; }

        [Column("price")]
        public double? Price { get; set; }
    }
}
