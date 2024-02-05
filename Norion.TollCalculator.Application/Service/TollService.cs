using Norion.TollCalculator.Domain.Models;
using Norion.TollCalculator.Domain.Repository;
using Norion.TollCalculator.Domain.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Norion.TollCalculator.Application.Service;

public class TollService : ITollService
{ 
    private readonly ITollRepository _repository;

    public TollService(ITollRepository repository)
    {
        _repository = repository;
    }

    public async Task AddPassage(Guid id, DateTime passageTime)
    {
        await _repository.AddPassage(id, passageTime);
    }

    public async Task<Guid> AddVehicle(Vehicle vehicle)
    {
        return await _repository.AddVehicle(vehicle);
    }

    public async Task<int> GetTollFee(Guid id)
    {
        return await _repository.GetTotalTollFee(id);
    }
}
