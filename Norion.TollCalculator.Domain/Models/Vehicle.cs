using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Norion.TollCalculator.Domain.Models
{
    public interface IVehicle
    {
        bool IsTollExempt { get; set; }
        string GetVehicleType();
        DateTime LastPassage { get; set; }
        List<DateTime> TotalDailyPassages { get; set; }
    }
}
