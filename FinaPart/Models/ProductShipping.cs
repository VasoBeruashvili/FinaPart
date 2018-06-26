using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FinaPart.Models
{
    [Table("ProductShipping", Schema = "doc")]
    public class ProductShipping
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("transp_start_place")]
        public string TransportStartPlace { get; set; }

        [Column("transp_end_place")]
        public string TransportEndPlace { get; set; }

        [Column("transporter_IdNum")]
        public string TransporterIdNum { get; set; }

        [Column("transport_model")]
        public string TransportModel { get; set; }

        [Column("transport_number")]
        public string TransportNumber { get; set; }

        [Column("avto")]
        public bool? Avto { get; set; }

        [Column("railway")]
        public bool? Railway { get; set; }

        [Column("other")]
        public bool? Other { get; set; }

        [Column("reciever_name")]
        public string RecieverName { get; set; }

        [Column("sender_name")]
        public string SenderName { get; set; }

        [Column("comment")]
        public string Comment { get; set; }

        [Column("is_waybill")]
        public int? IsWaybill { get; set; } = 1;

        [Column("waybill_id")]
        public int? WaybillId { get; set; } = 0;

        [Column("transport_type_id")]
        public int? TransportTypeId { get; set; } = 1;

        [Column("waybill_status")]
        public int? WaybillStatus { get; set; } = -1;

        [Column("waybill_type")]
        public int? WaybillType { get; set; } = 4;

        [Column("waybill_cost")]
        public double? WaybillCost { get; set; } = 0;

        [Column("delivery_date")]
        public DateTime? DeliveryDate { get; set; } = DateTime.Now;

        [Column("transport_begin_date")]
        public DateTime? TransportBeginDate { get; set; } = DateTime.Now;

        [Column("activate_date")]
        public DateTime? ActivateDate { get; set; } = DateTime.Today.AddSeconds(10);

        [Column("transport_cost_payer")]
        public int? TransportCostPayer { get; set; } = 2;

        [Column("driver_name")]
        public string DriverName { get; set; }

        [Column("waybill_num")]
        public string WaybillNum { get; set; }

        [Column("is_foreign")]
        public byte? IsForeign { get; set; } = 0;

        [Column("transport_text")]
        public string TransportText { get; set; }

        [Column("general_id")]
        public int? GeneralId { get; set; }

        [ForeignKey("GeneralId")]
        public GeneralDocs GeneralDoc { get; set; }
    }
}