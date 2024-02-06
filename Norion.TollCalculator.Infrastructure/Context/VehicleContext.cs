using Norion.TollCalculator.Domain.Context;
using Norion.TollCalculator.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Norion.TollCalculator.Infrastructure.Context;

public class VehicleContext : IVehicleContext
{
    public List<Vehicle> vehicles;
    public VehicleContext()
    {
        vehicles = new List<Vehicle>();
    }

    public async Task<Vehicle> GetVehicle(Guid id)
    {
        await Task.CompletedTask;
        return vehicles.FirstOrDefault(x => x.Id == id) ?? null;
    }

    public async Task AddVehicle(Vehicle vehicle)
    {
        await Task.CompletedTask;
        vehicles.Add(vehicle);
    }

    public async Task AddPassage(Vehicle vehicle, DateTime passageTime)
    {
        await Task.CompletedTask;
        vehicle.LastPassage = passageTime;
        vehicle.TotalDailyPassages.Add(passageTime);
    }
}
