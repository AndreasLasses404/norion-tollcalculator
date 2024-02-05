using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Norion.TollCalculator.Domain.Models;

public class MotorBike : Vehicle
{
    public MotorBike()
    {
        IsTollExempt = false;
    }
    public string GetVehicleType()
    {
        return "Motorbike";
    }
}
