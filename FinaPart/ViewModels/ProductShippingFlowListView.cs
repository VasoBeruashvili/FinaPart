using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinaPart.ViewModels
{
    public class ProductShippingFlowListView
    {
        public int Id { get; set; }

        public int GeneralId { get; set; }

        public string StartPlace { get; set; }

        public int? ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductCode { get; set; }

        public string UnitName { get; set; }

        public double? Price { get; set; }

        public double? Rest { get; set; }
    }
}
