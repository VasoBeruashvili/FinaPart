using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinaPart.ViewModels
{
    public class ProductShippingViewModel
    {
        public GeneralViewModel General { get; set; } = new GeneralViewModel();

        public string TransportStartPlace { get; set; }

        public string TransporterIdNum { get; set; }

        public string TransportEndPlace { get; set; }

        public string TransportNumber { get; set; }

        public string TransportModel { get; set; }

        public string DriverName { get; set; }

    }
}
