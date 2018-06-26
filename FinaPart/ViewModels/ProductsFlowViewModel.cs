

using Newtonsoft.Json;
using System;

namespace FinaPart
{
    public class ProductsFlowViewModel
    {
        [JsonProperty("id")] //+ +
        public int Id { get; set; }

        [JsonProperty("product_id")] //+ +
        public int ProductId { get; set; }

        [JsonProperty("name")] //+ +
        public string ProductName { get; set; }

        [JsonProperty("product_path")] //+ +
        public string ProductPath { get; set; }

        [JsonProperty("product_group_id")] //+ +
        public int ProductGroupId { get; set; }

        [JsonProperty("code")] //+ +
        public string ProductCode { get; set; }

        [JsonProperty("quantity")] //+ 1 +
        public double Quantity { get; set; }

        [JsonProperty("price")] //+ erteulis fasi +
        public double UnitCost { get; set; }

        [JsonProperty("self_cost")] //+ erteulis fasi +
        public double SelfCost { get; set; }

        [JsonProperty("originalPrice")] //+ erteulis fasi +
        public double OriginalUnitCost { get; set; }

        [JsonProperty("total")] //+ erteulis fasi +
        public double Total { get; set; }

        [JsonProperty("store_id")] //+ 1 +
        public int StoreId { get; set; }

        [JsonProperty("vat")] //+ gamotvladi +
        public decimal VatPercent { get; set; }

        [JsonProperty("vatVal")] //+ gamotvladi +
        public decimal VatValue { get; set; }

        [JsonProperty("is_order")]
        public byte IsOrder { get; set; }

        [JsonProperty("parent_product_id")]
        public int ParentProductId { get; set; } = 0;

        [JsonProperty("unit_id")] //+ query dan +
        public int? UnitId { get; set; }

        [JsonProperty("unit_name")] //+ query dan +
        public string UnitName { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("discountPercent")] //+ 0 jer +
        public double DiscountPercent { get; set; }

        [JsonProperty("discount_value")]
        public double DiscountValue { get; set; }

        [JsonProperty("uid")] //+ rac gadmomeca davabruno ???
        public Guid Uid { get; set; }

        [JsonProperty("product_vat_type")] //+ query dan +
        public byte ProductVatType { get; set; }

        [JsonProperty("departmentName")] //+ query dan +
        public string DepartmentName { get; set; }

        [JsonProperty("rest")] //+ query dan +
        public double Rest { get; set; }

        [JsonProperty("vendorId")]
        public int VendorId { get; set; }
    }
}
