using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Passengers.ApplicationServices.Passengers;
using Passengers.Core.Passengers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Passengers.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PassengersController : ControllerBase
    {
        private readonly IPassengersAppService _appService;
        private readonly ILogger _logger;
        public PassengersController(IPassengersAppService appService, ILogger<PassengersController> logger)
        {
            _appService = appService;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        }
        // GET: api/<PassengersController>
        [HttpGet]
        public async Task<IEnumerable<Passenger>> Get()
        {
            var passengers = await _appService.GetPassengersAsync();

            _logger.LogInformation("Total passengers retrieved: " + passengers?.Count);
            return passengers;
        }

        // GET api/<PassengersController>/5
        [HttpGet("{id}")]
        public async Task<Passenger> Get(int id)
        {
            var passenger = await _appService.GetPassengerAsync(id);
            _logger.LogInformation("Get user: "+id);
            return passenger;
        }

        // POST api/<PassengersController>
        [HttpPost]
        public void Post([FromBody] Passenger passengerDto)
        {
            _logger.LogInformation($"adding passenger {passengerDto}");
            _appService.AddPassengerAsync(passengerDto);
        }

        // PUT api/<PassengersController>/5
        [HttpPut("{id}")]
        public async void Put(int id, [FromBody] Passenger passenger)
        {
            passenger.Id = id;
            _logger.LogInformation($"editing passenger {passenger}");
            await _appService.EditPassengerAsync(passenger);
        }

        // DELETE api/<PassengersController>/5
        [HttpDelete("{id}")]
        public async void Delete(int id)
        {
            _logger.LogInformation("dropping a passenger...");
            await _appService.DeletePassengerAsync(id);
        }
    }
}
