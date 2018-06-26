using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinaPart
{
    public class ProductMoveViewModel
    {
        public DateTime? ActivateDate { get; set; }
        public bool? Avto { get; set; }
        public string Comment { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public double? DiscountPercent { get; set; }
        public string DriverCardNumber { get; set; }
        public string DriverName { get; set; }
        public string FirnishNumber { get; set; }
        public int? IsWaybill { get; set; }
        public bool? Other { get; set; }
        public bool? Railway { get; set; }
        public string RecieverIdNum { get; set; }
        public string RecieverName { get; set; }
        public string ResponsablePerson { get; set; }
        public DateTime? ResponsablePersonDate { get; set; }
        public string ResponsablePersonNum { get; set; }
        public string SenderIdNum { get; set; }
        public string SenderName { get; set; }
        public DateTime? TransportBeginDate { get; set; }
        public int? TransportCostPayer { get; set; }
        public string TransportEndPlace { get; set; }
        public string TransporterIdNum { get; set; }
        public string TransporterName { get; set; }
        public string TransportModel { get; set; }
        public string TransportNumber { get; set; }
        public string TransportStartPlace { get; set; }
        public string TransportText { get; set; }
        public int? TransportTypeId { get; set; }
        public double? WaybillCost { get; set; }
        public int? WaybillId { get; set; }
        public int? WaybillStatus { get; set; }
        public int? WaybillType { get; set; }

        public GeneralViewModel General { get; set; } = new GeneralViewModel();

        public List<ProductsFlowViewModel> Products { get; set; } = new List<ProductsFlowViewModel>();
    }
}
