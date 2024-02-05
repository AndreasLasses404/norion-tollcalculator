using Norion.TollCalculator.Api.DTOs;
using Norion.TollCalculator.Domain.Models;

namespace Norion.TollCalculator.Api.Mappers;

public static class VehicleTypeMapper
{
    public static Car ToCar(this Vehicle vehicle) =>
        new Car()
        {
        };

    public static Diplomat ToDiplomat(this PostVehicleDto vehicle) =>
    new()
    {
    };

    public static Emergency ToEmergency(this PostVehicleDto vehicle) =>
    new()
    {
    };

    public static Foreign ToForeign(this PostVehicleDto vehicle) =>
    new()
    {
    };

    public static Military ToMilitary(this PostVehicleDto vehicle) =>
    new()
    {
    };

    public static MotorBike ToMotorBike(this PostVehicleDto vehicle) =>
    new()
    {
    };

    public static Tractor ToTractor(this PostVehicleDto vehicle) =>
    new()
    {
    };

}
