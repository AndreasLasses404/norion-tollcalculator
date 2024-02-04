using Microsoft.AspNetCore.Mvc;
using Norion.TollCalculator.Domain.Models;
using Norion.TollCalculator.Domain.Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Norion.TollCalculator.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TollController: ControllerBase 
{
    private readonly ITollService _service;
    public TollController(ITollService service)
    {
        _service = service;
    }
    // GET: api/<TollController>
    [HttpGet]
    public async Task<ActionResult> Get([FromBody] IVehicle vehicle)
    {
        return Ok(await _service.GetTollFee(vehicle));
    }

    // GET api/<TollController>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // POST api/<TollController>
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT api/<TollController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<TollController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
