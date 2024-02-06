using Moq;
using Norion.TollCalculator.Application.Service;
using Norion.TollCalculator.Domain.Models;
using Norion.TollCalculator.Domain.Repository;

namespace Norion.TollCalculator.Test;

public class TollServiceTest
{
    private readonly Mock<ITollRepository> _mock = new Mock<ITollRepository>();
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


    public static TheoryData<Vehicle, int> GetTotalFeeCases =
    new()
    {
        { 
            new Car
            {
                Id = Guid.NewGuid(),
                LastPassage = new DateTime(2023, 01, 19, 17, 25, 20),
                TotalDailyPassages = [new DateTime(2023, 01, 19, 08, 05, 20), new DateTime(2023, 01, 19, 14, 30, 20), new DateTime(2023, 01, 19, 17, 25, 20)] 
            },
            34 
        },
        {
            new MotorBike
            {
                Id = Guid.NewGuid(),
                LastPassage = new DateTime(2023, 01, 19, 17, 25, 20),
                TotalDailyPassages = [new DateTime(2023, 01, 19, 06, 00, 10), new DateTime(2023, 01, 19, 14, 59, 58), new DateTime(2023, 01, 19, 18, 29, 57)]
            },
            24
        },
        {
            new Car
            {
                Id = Guid.NewGuid(),
                LastPassage = new DateTime(2023, 01, 19, 17, 25, 20), 
                TotalDailyPassages = [new DateTime(2023, 01, 19, 08, 05, 20), new DateTime(2023, 01, 19, 14, 30, 20),new DateTime(2023, 01, 19, 15, 31, 20), new DateTime(2023, 01, 19, 16, 32, 20), new DateTime(2023, 01, 19, 17, 25, 20), new DateTime(2023, 01, 19, 18, 26, 20)]
            },
            60
        }
    };
    [Theory, MemberData(nameof(GetTotalFeeCases))]

    public async Task GetTotalTollFee_ShouldReturnCorrectFee(Vehicle vehicle, int expectedFee)
    {
        //Arrange
        _mock.Setup(r => r.GetVehicle(It.IsAny<Guid>())).ReturnsAsync(vehicle);

        //Act
        var result = await _service.GetTotalTollFee(vehicle.Id);

        //Assert
        _mock.Verify(r => r.GetVehicle(It.IsAny<Guid>()), Times.Once);
        Assert.Equal(expectedFee, result);

    }


    public static TheoryData<Vehicle> GetExemptVehicles =
    new()
    {
            {
                new Diplomat
                {
                    Id = Guid.NewGuid(),
                    LastPassage = new DateTime(2023, 01, 19, 17, 25, 20),
                    TotalDailyPassages = [new DateTime(2023, 01, 19, 08, 05, 20), new DateTime(2023, 01, 19, 14, 30, 20), new DateTime(2023, 01, 19, 17, 25, 20)]
                }
            
            },
            {
                new Emergency
                {
                    Id = Guid.NewGuid(),
                    LastPassage = new DateTime(2023, 01, 19, 17, 25, 20),
                    TotalDailyPassages = [new DateTime(2023, 01, 19, 08, 05, 20), new DateTime(2023, 01, 19, 14, 30, 20), new DateTime(2023, 01, 19, 17, 25, 20)]
                }

            },
            {
                new Foreign
                {
                    Id = Guid.NewGuid(),
                    LastPassage = new DateTime(2023, 01, 19, 17, 25, 20),
                    TotalDailyPassages = [new DateTime(2023, 01, 19, 08, 05, 20), new DateTime(2023, 01, 19, 14, 30, 20), new DateTime(2023, 01, 19, 17, 25, 20)]
                }

            },
            {
                new Military
                {
                    Id = Guid.NewGuid(),
                    LastPassage = new DateTime(2023, 01, 19, 17, 25, 20),
                    TotalDailyPassages = [new DateTime(2023, 01, 19, 08, 05, 20), new DateTime(2023, 01, 19, 14, 30, 20), new DateTime(2023, 01, 19, 17, 25, 20)]
                }

            },
            {
                new Tractor
                {
                    Id = Guid.NewGuid(),
                    LastPassage = new DateTime(2023, 01, 19, 17, 25, 20),
                    TotalDailyPassages = [new DateTime(2023, 01, 19, 08, 05, 20), new DateTime(2023, 01, 19, 14, 30, 20), new DateTime(2023, 01, 19, 17, 25, 20)]
                }

            }
    };
    [Theory, MemberData(nameof(GetExemptVehicles))]
    public async Task GetTotalTollFee_OnlyTollExemptVehicles(Vehicle vehicle)
    {
        //Arrange
        _mock.Setup(r => r.GetVehicle(It.IsAny<Guid>())).ReturnsAsync(vehicle);

        //Act
        var result = await _service.GetTotalTollFee(vehicle.Id);

        //Assert
        _mock.Verify(r => r.GetVehicle(It.IsAny<Guid>()), Times.Once);
        Assert.Equal(0, result);
    }

    public static TheoryData<Vehicle> GetVehicleOnWeekend =
     new()
     {
        {
            new Car
            {
                Id = Guid.NewGuid(),
                LastPassage = new DateTime(2023, 01, 21, 17, 25, 20),
                TotalDailyPassages = [new DateTime(2023, 01, 19, 21, 05, 20), new DateTime(2023, 01, 21, 14, 30, 20), new DateTime(2023, 01, 21, 17, 25, 20)]
            }
        },
        {
            new Car
            {
                Id = Guid.NewGuid(),
                LastPassage = new DateTime(2023, 01, 22, 17, 25, 20),
                TotalDailyPassages = [new DateTime(2023, 01, 22, 08, 05, 20), new DateTime(2023, 01, 22, 14, 30, 20), new DateTime(2023, 01, 22, 17, 25, 20)]
            }
         },   
         {
            new Car
            {
                Id = Guid.NewGuid(),
                LastPassage = new DateTime(2023, 01, 22, 17, 25, 20),
                TotalDailyPassages = [new DateTime(2023, 12, 24, 08, 05, 20), new DateTime(2023, 12, 25, 14, 30, 20), new DateTime(2023, 01, 22, 17, 25, 20)]
            }
         },


     };
    [Theory, MemberData(nameof(GetVehicleOnWeekend))]
    public async Task GetTotalTollFee_Weekdays_ShouldReturnZero(Vehicle vehicle)
    {
        //Arrange
        _mock.Setup(r => r.GetVehicle(It.IsAny<Guid>())).ReturnsAsync(vehicle);

        //Act
        var result = await _service.GetTotalTollFee(vehicle.Id);

        //Assert
        _mock.Verify(r => r.GetVehicle(It.IsAny<Guid>()), Times.Once);
        Assert.Equal(0, result);
    }
}


