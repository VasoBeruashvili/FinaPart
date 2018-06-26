using Newtonsoft.Json;
using System;

namespace FinaPart
{
    public class GeneralViewModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("tdate")]
        public DateTime Tdate { get; set; } = DateTime.Now;

        [JsonProperty("doc_num")]
        public long DocNum { get; set; }

        [JsonProperty("doc_num_prefix")]
        public string DocNumPrefix { get; set; } = string.Empty;

        [JsonProperty("purpose")]
        public string Purpose { get; set; }

        [JsonProperty("amount")]
        public double Amount { get; set; }

        [JsonProperty("currency_id")]
        public int CurrencyId { get; set; } = 1;

        [JsonProperty("rate")]
        public double Rate { get; set; } = 1.0;

        [JsonProperty("vat")]
        public double Vat { get; set; }

        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [JsonProperty("ref_id")]
        public int RefId { get; set; }

        [JsonProperty("param_id1")]
        public int? ParamId1 { get; set; }

        [JsonProperty("param_id2")]
        public int? ParamId2 { get; set; }

        [JsonProperty("status_id")]
        public int StatusId { get; set; }

        [JsonProperty("make_entry")]
        public bool MakeEntry { get; set; }

        [JsonProperty("project_id")]
        public int ProjectId { get; set; } = 1;

        [JsonProperty("uid")]
        public string Uid { get; set; } = Guid.NewGuid().ToString();

        [JsonProperty("sync_status")]
        public byte SyncStatus { get; set; }

        [JsonProperty("waybill_num")]
        public string WaybillNum { get; set; }

        [JsonProperty("store_id")]
        public int StoreId { get; set; }

        [JsonProperty("analytic_code")]
        public int AnalyticCode { get; set; }

        [JsonProperty("amount2")]
        public double Amount2 { get; set; }

        [JsonProperty("contragent_sub_id")]
        public int ContragentSubId { get; set; }

        [JsonProperty("contragent_id")]
        public int ContragentId { get; set; }

        [JsonProperty("contragent_name")]
        public string ContragentName { get; set; }

        [JsonProperty("contragent_last_name")]
        public string ContragentLastName { get; set; }

        [JsonProperty("sender_name")]
        public string SenderName { get; set; }

        [JsonProperty("sender_last_name")]
        public string SenderLastName { get; set; }

        [JsonProperty("bed_name")]
        public string BedName { get; set; }

        [JsonProperty("is_blocked")]
        public bool IsBlocked { get; set; }

        [JsonProperty("is_packed")]
        public bool IsPacked { get; set; }

        [JsonProperty("is_deleted")]
        public bool IsDeleted { get; set; }

        [JsonProperty("delete_user_id")]
        public int? DeleteUserId { get; set; }

        [JsonProperty("delete_date")]
        public DateTime? DeleteDate { get; set; }

        [JsonProperty("create_date")]
        public DateTime CreateDate { get; set; }

        [JsonProperty("house_id")]
        public int HouseId { get; set; } = 1;

        [JsonProperty("staff_id")]
        public int? StaffId { get; set; }

        [JsonProperty("staff_name")]
        public string StaffName { get; set; }

        public GeneralViewModel()
        {
            Tdate = DateTime.Now;
        }
    }
}
