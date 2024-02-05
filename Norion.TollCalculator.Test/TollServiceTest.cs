using Moq;
using Norion.TollCalculator.Application.Service;
using Norion.TollCalculator.Domain.Models;
using Norion.TollCalculator.Domain.Service;
using Norion.TollCalculator.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Norion.TollCalculator.Test;

public class TollServiceTest
{
    private readonly Mock<TollRepository> _mock = new Mock<TollRepository>();
    private readonly TollService _service;
    public TollServiceTest()
    {
        _service = new TollService(_mock.Object);
    }

    [Fact]
    public async Task AddPassage_ShouldAddPassageToExistingVehicle()
    {
        //Arrange
        var vehicle = new Car
        {
            Id = Guid.NewGuid(),
            IsTollExempt = false
        };
        _mock.Setup(r => r.AddPassage(It.IsAny<Guid>(), It.IsAny<DateTime>()));

        //Act
        await _service.AddPassage(vehicle.Id, DateTime.Now);

        //Assert
        _mock.Verify(r => r.AddPassage(It.IsAny<Guid>(), It.IsAny<DateTime>()), Times.Once);
    }

    [Fact]
    public async Task AddVehicle_ShouldAddValidVehicle()
    {
        //Arrange
        Car car = new Car { Id = Guid.NewGuid(), IsTollExempt = false };
        _mock.Setup(r => r.AddVehicle(It.IsAny<Vehicle>())).ReturnsAsync(car.Id);

        //Act
        var result = await _service.AddVehicle(car);

        //Assert
        _mock.Verify(r => r.AddVehicle(It.IsAny<Vehicle>()), Times.Once);
        Assert.Equal(car.Id, result);
    }

    [Fact]
    public async Task AddVehicle_ShouldFailThrowException()
    {
        //Arrange
        Car car = new Car()
        {
            Id = Guid.NewGuid()
        };
        _mock.Setup(r => r.AddVehicle(It.IsAny<Vehicle>())).ThrowsAsync(new NullReferenceException());

        
        //Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(async () => await _service.AddVehicle(car));
    }

}
