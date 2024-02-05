using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Norion.TollCalculator.Domain.Models;

public class Car : Vehicle
{
    public Car()
    {
        IsTollExempt = false;
    }

    public string GetVehicleType()
    {
        return "Car";
    }
}
