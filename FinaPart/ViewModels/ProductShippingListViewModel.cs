using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinaPart.ViewModels
{
    public class ProductShippingListViewModel
    {
        public int id { get; set; }

        public DateTime tdate { get; set; }

        public string doc_num { get; set; }

        public string purpose { get; set; }

        public decimal amount { get; set; }

        public string currency_name { get; set; }

        public string bort_name { get; set; }

        public string staff_name { get; set; }

        public string rs_status { get; set; }

        public int? waybill_id { get; set; }

        public int? waybill_status { get; set; }

        public string transp_start_place { get; set; }
    }
}
