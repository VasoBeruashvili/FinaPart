using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FinaPart.Models
{
    [Table("ProductMove", Schema = "doc")]
    public class ProductMove
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("general_id")]
        public int GeneralId { get; set; }

        [Column("sender_IdNum")]
        public string SenderIdNum { get; set; } = string.Empty;

        [Column("sender_name")]
        public string SenderName { get; set; } = string.Empty;

        [Column("transp_start_place")]
        public string TransportStartPlace { get; set; } = string.Empty;

        [Column("transporter_name")]
        public string TransporterName { get; set; } = string.Empty;

        [Column("transporter_IdNum")]
        public string TransporterIdNum { get; set; } = string.Empty;

        [Column("reciever_IdNum")]
        public string RecieverIdNum { get; set; } = string.Empty;

        [Column("reciever_name")]
        public string RecieverName { get; set; } = string.Empty;

        [Column("transp_end_place")]
        public string TransportEndPlace { get; set; } = string.Empty;

        [Column("responsable_person")]
        public string ResponsablePerson { get; set; } = string.Empty;

        [Column("responsable_person_num")]
        public string ResponsablePersonNum { get; set; } = string.Empty;

        [Column("responsable_person_date")]
        public DateTime? ResponsablePersonDate { get; set; } = DateTime.Now;

        [Column("transport_model")]
        public string TransportModel { get; set; } = string.Empty;

        [Column("transport_number")]
        public string TransportNumber { get; set; } = string.Empty;

        [Column("driver_card_number")]
        public string DriverCardNumber { get; set; } = string.Empty;

        [Column("avto")]
        public bool? Avto { get; set; } = false;

        [Column("railway")]
        public bool? Railway { get; set; } = false;

        [Column("other")]
        public bool? Other { get; set; } = false;

        [Column("discount_percent")]
        public double? DiscountPercent { get; set; } = 0;

        [Column("is_waybill")]
        public int? IsWaybill { get; set; } = 1;

        [Column("waybill_id")]
        public int? WaybillId { get; set; } = 0;

        [Column("waybill_type")]
        public int? WaybillType { get; set; } = 1;

        [Column("waybill_cost")]
        public double? WaybillCost { get; set; } = 0;

        [Column("delivery_date")]
        public DateTime? DeliveryDate { get; set; } = DateTime.Now;

        [Column("waybill_status")]
        public int? WaybillStatus { get; set; } = -1;

        [Column("transport_begin_date")]
        public DateTime? TransportBeginDate { get; set; } = DateTime.Now;

        [Column("activate_date")]
        public DateTime? ActivateDate { get; set; } = DateTime.Today.AddSeconds(10);

        [Column("transport_cost_payer")]
        public int? TransportCostPayer { get; set; } = 1;

        [Column("transport_type_id")]
        public int? TransportTypeId { get; set; } = 1;

        [Column("driver_name")]
        public string DriverName { get; set; } = string.Empty;

        [Column("waybill_num")]
        public string WaybillNum { get; set; }

        [Column("staff_id")]
        public int? StaffId { get; set; } = 0;

        [Column("is_foreign")]
        public byte? IsForeign { get; set; } = 0;

        [Column("comment")]
        public string Comment { get; set; } = string.Empty;

        [Column("location_type")]
        public bool LocationType { get; set; }

        [Column("transport_text")]
        public string TransportText { get; set; }

        [Column("chek_status")]
        public int CheckStatus { get; set; }

        [Column("category_id")]
        public byte CategoryId { get; set; }

        [ForeignKey("GeneralId")]
        public GeneralDocs GeneralDoc { get; set; }
    }
}