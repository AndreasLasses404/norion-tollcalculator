using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Norion.TollCalculator.Domain.Models
{
    public class Vehicle
    {
        public Guid Id { get; set; }
        public bool IsTollExempt { get; set; }
        public DateTime LastPassage { get; set; }
        public List<DateTime> TotalDailyPassages { get; set; } = new();
    }
}
