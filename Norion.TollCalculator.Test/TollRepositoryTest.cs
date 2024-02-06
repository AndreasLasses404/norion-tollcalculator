using Moq;
using Norion.TollCalculator.Domain.Context;
using Norion.TollCalculator.Domain.Models;
using Norion.TollCalculator.Infrastructure.Context;
using Norion.TollCalculator.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Norion.TollCalculator.Test;

public class TollRepositoryTest
{
    private readonly Mock<IVehicleContext> _vehicleMock = new Mock<IVehicleContext>();
    private readonly TollRepository _tollRepository;
    public TollRepositoryTest()
    {
        _tollRepository = new TollRepository(_vehicleMock.Object);
    }

    [Fact]
    public async Task AddVehicle_ShouldAddVehicle()
    {
        Car car = new Car { Id = Guid.NewGuid() };

        _vehicleMock.Setup(x => x.AddVehicle(It.IsAny<Vehicle>()));
        _vehicleMock.Setup(x => x.GetVehicle(It.IsAny<Guid>())).ReturnsAsync(car);

        var result = await _tollRepository.AddVehicle(car);
        var fetchedVehicle = await _tollRepository.GetVehicle(car.Id);

        Assert.Equal(car, fetchedVehicle);
    }

    [Fact]
    public async Task AddPassage_ShouldAddPassage()
    {
        Car car = new Car { Id = Guid.NewGuid() };

        _vehicleMock.Setup(x => x.GetVehicle(It.IsAny<Guid>())).ReturnsAsync(car);
        _vehicleMock.Setup(x => x.AddPassage(It.IsAny<Vehicle>(), It.IsAny<DateTime>()));

        await _tollRepository.AddPassage(car.Id, DateTime.Now);

        _vehicleMock.Verify(x => x.AddPassage(It.IsAny<Vehicle>(), It.IsAny<DateTime>()), Times.Once);
        _vehicleMock.Verify(x => x.GetVehicle(It.IsAny<Guid>()), Times.Once);
    }
}
