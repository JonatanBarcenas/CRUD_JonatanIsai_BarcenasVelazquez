using Microsoft.AspNetCore.Mvc;
using Passengers.ApplicationServices.Passengers;
using Passengers.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Passengers.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PassengersController : ControllerBase
    {
        private readonly IPassengersAppService _appService;
        public PassengersController(IPassengersAppService appService)
        {
            _appService = appService;
        }
        // GET: api/<PassengersController>
        [HttpGet]
        public async Task<IEnumerable<Passenger>> Get()
        {
            return await _appService.GetPassengersAsync();
        }

        // GET api/<PassengersController>/5
        [HttpGet("{id}")]
        public async Task<Passenger> Get(int id)
        {
            return await _appService.GetPassengerAsync(id);
        }

        // POST api/<PassengersController>
        [HttpPost]
        public void Post([FromBody] Passenger passenger)
        {
            _appService.AddPassengerAsync(passenger);
        }

        // PUT api/<PassengersController>/5
        [HttpPut("{id}")]
        public async void Put(int id, [FromBody] Passenger passenger)
        {
            passenger.Id = id;
            await _appService.EditPassengerAsync(passenger);
        }

        // DELETE api/<PassengersController>/5
        [HttpDelete("{id}")]
        public async void Delete(int id)
        {
            await _appService.DeletePassengerAsync(id);
        }
    }
}
