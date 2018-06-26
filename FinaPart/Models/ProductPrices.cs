using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FinaPart.Models
{
    [Table("ProductPrices", Schema = "book")]
    public class ProductPrices
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("price_id")]
        public int? PriceId { get; set; }

        [Column("manual_val")]
        public double? ManualVal { get; set; }

        [Column("product_id")]
        public int? ProductId { get; set; }

        [Column("manual_currency_id")]
        public int? ManualCurrencyId { get; set; }
    }
}