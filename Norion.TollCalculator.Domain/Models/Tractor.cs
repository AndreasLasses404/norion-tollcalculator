using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Norion.TollCalculator.Domain.Models;

public class Tractor : Vehicle
{
    public Tractor()
    {
        IsTollExempt = true;
    }
    public string GetVehicleType()
    {
        return "Tractor";
    }
}
