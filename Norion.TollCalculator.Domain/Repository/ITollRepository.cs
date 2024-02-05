using Norion.TollCalculator.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Norion.TollCalculator.Domain.Repository
{
    public interface ITollRepository
    {
        Task<int> GetTotalTollFee(Guid id);
        Task AddPassage(Guid id, DateTime passageTime);
        Task<Guid> AddVehicle(Vehicle vehicle);

    }
}
