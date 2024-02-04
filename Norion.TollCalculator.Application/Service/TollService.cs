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
    public TollService(ITollRepository repository) : base()
    {
        _repository = repository;

    }
    public async Task<int> GetTollFee(IVehicle vehicle)
    {
        return await _repository.GetTotalTollFee(vehicle);
    }
}
