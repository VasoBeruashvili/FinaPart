using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FinaPart.Models
{
    [Table("ProductsFlow", Schema = "doc")]
    public class ProductsFlow
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("general_id")]
        public int GeneralId { get; set; }
        [ForeignKey("GeneralId")]
        public GeneralDocs GeneralDocs { get; set; }

        [Column("product_id")]
        public int ProductId { get; set; }

        [Column("product_tree_path")]
        public string ProductTreePath { get; set; }

        [Column("amount")]
        public double? Amount { get; set; }

        [Column("price")]
        public double? Price { get; set; }

        [Column("store_id")]
        public int? StoreId { get; set; }

        [Column("vat_percent")]
        public decimal? VatPercent { get; set; }

        [Column("self_cost")]
        public double? SelfCost { get; set; }

        [Column("coeff")]
        public int? Coeff { get; set; }

        [Column("is_order")]
        public byte? IsOrder { get; set; }

        [Column("is_expense")]
        public byte? IsExpense { get; set; }

        [Column("is_move")]
        public byte? IsMove { get; set; }

        [Column("visible")]
        public byte? Visible { get; set; }

        [Column("parent_product_id")]
        public int? ParentProductId { get; set; }

        [Column("ref_id")]
        public int? RefId { get; set; }

        [Column("unit_id")]
        public int? UnitId { get; set; }

        [Column("comment")]
        public string Comment { get; set; }

        [Column("discount_percent")]
        public double? DiscountPercent { get; set; }

        [Column("discount_value")]
        public double? DiscountValue { get; set; }

        [Column("original_price")]
        public double? OriginalPrice { get; set; }

        [Column("in_id")]
        public int? InId { get; set; }

        [Column("vendor_id")]
        public int? VendorId { get; set; }

        [Column("cafe_status")]
        public byte? CafeStatus { get; set; }

        [Column("out_id")]
        public int? OutId { get; set; }

        [Column("sub_id")]
        public int? SubId { get; set; }

        [Column("service_product_id")]
        public int? ServiceProductId { get; set; }

        [Column("uid")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public Guid Uid { get; set; }

        [Column("service_staff_id")]
        public int? ServiceStaffId { get; set; }

        [Column("staff_salary")]
        public double? StaffSalary { get; set; }

        [Column("product_bonus")]
        public double? ProductBonus { get; set; }

        [Column("cafe_comment")]
        public string CafeComment { get; set; }

        [Column("cafe_send_date")]
        public DateTime? CafeSendDate { get; set; }

        //[Column("ref_uid")]
        //public string RefUid { get; set; }

        //[Column("excise")]
        //public double Excise { get; set; }

        [ForeignKey("ProductId")]
        public Products Products { get; set; }

        [ForeignKey("UnitId")]
        public Units Unit { get; set; }

        public ProductsFlow()
        {
            this.ProductTreePath = string.Empty;
            this.VatPercent = 0;
            this.SelfCost = 0;
            this.ParentProductId = 0;
            this.RefId = 0;
            this.DiscountPercent = 0;
            this.DiscountValue = 0;
            this.CafeStatus = 0;
            this.SubId = 0;
            this.ServiceProductId = 0;
            this.StaffSalary = 0;
            this.ProductBonus = 0;
            this.IsOrder = 0;
            this.IsExpense = 0;
            this.IsMove = 0;
            this.Visible = 1;
            this.InId = 0;
            this.OutId = 0;
            this.VendorId = 0;
            this.OriginalPrice = 0;
            this.Comment = string.Empty;
        }
    }
}