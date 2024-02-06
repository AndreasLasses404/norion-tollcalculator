using Norion.TollCalculator.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Norion.TollCalculator.Domain.Service;

public interface ITollService
{
    Task<int> GetTotalTollFee(Guid id);
    Task<Guid> AddVehicle(Vehicle vehicle);
    Task AddPassage(Guid id, DateTime passageTime);
}
