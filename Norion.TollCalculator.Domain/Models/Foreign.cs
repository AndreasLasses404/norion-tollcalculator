using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Norion.TollCalculator.Domain.Models
{
    internal class Foreign : IVehicle
    {
        public bool IsTollExempt { get; set; } = true;
        public DateTime LastPassage { get; set; }
        public List<DateTime> TotalDailyPassages { get; set; } = new List<DateTime>();
        public string GetVehicleType()
        {
            return "Foreign";
        }
    }
}
