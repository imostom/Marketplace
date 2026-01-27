using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Core.Common.Models
{
    public class AppSettings
    {
        public string AddInventoryProcedure { get; set; } = "";
        public string GetInventoryProcedure { get; set; } = "";
        public string GetInventoryByIDProcedure { get; set; } = "";
        
    }
}