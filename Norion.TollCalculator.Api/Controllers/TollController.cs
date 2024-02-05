using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Norion.TollCalculator.Api.DTOs;
using Norion.TollCalculator.Api.Mappers;
using Norion.TollCalculator.Domain.Models;
using Norion.TollCalculator.Domain.Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Norion.TollCalculator.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TollController: ControllerBase 
{
    private readonly ITollService _service;
    private readonly IMapper _mapper;
    public TollController(ITollService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    // GET api/<TollController>/5
    [HttpGet("{id}")]
    public async Task<ActionResult> Get(Guid id)
    {
        try
        {
            return Ok(await _service.GetTollFee(id));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // POST api/<TollController>
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] PostVehicleDto vehicle)
    {
        try
        {
            Vehicle specificVehicle = new Vehicle();
            switch (vehicle.VehicleType)
            {
                case "Car":
                    specificVehicle = _mapper.Map<Car>(vehicle);
                    break;
                case "Diplomat":
                    specificVehicle = vehicle.ToDiplomat();
                    break;
                case "Emergency":
                    specificVehicle = vehicle.ToEmergency();
                    break;
                case "Foreign":
                    specificVehicle = vehicle.ToForeign();
                    break;
                case "Military":
                    specificVehicle = vehicle.ToMilitary();
                    break;
                case "MotorBike":
                    specificVehicle = vehicle.ToMotorBike();
                    break;
                case "Tractor":      
                    specificVehicle = vehicle.ToTractor();
                    break;
            }

            await  _service.AddVehicle(specificVehicle);
            await _service.AddPassage(specificVehicle.Id, DateTime.Now);
            return Ok(specificVehicle.Id);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

    // PUT api/<TollController>/5
    [HttpPut("{id}/Passages")]
    public async Task<ActionResult> Put(Guid id)
    {
        try
        {
            await _service.AddPassage(id, DateTime.Now);
            return Ok();
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
