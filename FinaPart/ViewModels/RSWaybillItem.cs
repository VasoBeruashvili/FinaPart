using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinaPart.ViewModels
{
    public class RSWaybillGoods
    {
        public int RSUnitId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int ProcessType { get; set; }
        public int VatType { get; set; }
        public string UnitName { get; set; }
        public double Price { get; set; }
        public double Amount { get; set; }
    }

    public class RSWaybillItem
    {
        public enum WaybillDocOperation { ProductOut, ProductShipping, ProductMove, CustomerReturns }
        public WaybillDocOperation OperationMode { get; set; }
        public double FullAmount { get; set; }
        public bool IsOperationVat { get; set; }
        public bool IsCompanyVat { get; set; }
        public string ContragentCode { get; set; }
        public string ContragentName { get; set; }
        public string ContragentAddress { get; set; }
        public int ParamId1 { get; set; }
        public int ParamId2 { get; set; }
        public int PayType { get; set; }
        public int ParentWaybillId { get; set; }
        public bool IsResident { get; set; }
        public int WaybillType { get; set; }
        public int TransportType { get; set; }
        public int TransportCostPayer { get; set; }
        public DateTime TransportBeginDate { get; set; }
        public bool CheckBayerTin { get; set; }
        public string StartPlace { get; set; }
        public string EndPlace { get; set; }
        public string TransporterIdNum { get; set; }
        public string DriverName { get; set; }
        public string TransportNumber { get; set; }
        public double WaybillCost { get; set; }
        public string Sender { get; set; }
        public string Reciever { get; set; }
        public string Comment { get; set; }
        public string TransportText { get; set; }

        public int WaybillId { get; set; }
        public int WaybillStatus { get; set; }
        public string WaybillNum { get; set; }
        public string Error { get; set; }

        public List<RSWaybillGoods> GoodList { get; set; }
    }
}
