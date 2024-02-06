using Norion.TollCalculator.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Norion.TollCalculator.Domain.Context;

public interface IVehicleContext
{
    Task<Vehicle> GetVehicle(Guid id);
    Task AddVehicle(Vehicle vehicle);
    Task AddPassage(Vehicle vehicle, DateTime passageTime);
}
