using Journeys.ApplicationServices.Journeys;
using Journeys.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Passengers.Web.Controllers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Journeys.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class JourneysController : ControllerBase
    {
        private readonly IJourneyAppService _appService;
        private readonly ILogger _logger;
        public JourneysController(IJourneyAppService appService, ILogger<JourneysController> logger)
        {
            _appService = appService;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        }
        // GET: api/<PassengersController>
        [HttpGet]
        public async Task<IEnumerable<Journey>> Get()
        {
            var journeys = await _appService.GetJourneysAsync();

            _logger.LogInformation("Total journeys retrieved: " + journeys?.Count);
            return journeys;
        }

        // GET api/<PassengersController>/5
        [HttpGet("{id}")]
        public async Task<Journey> Get(int id)
        {
            var journey = await _appService.GetJourneyAsync(id);
            _logger.LogInformation("Get journey: " + id);
            return journey;
        }

        // POST api/<PassengersController>
        [HttpPost]
        public void Post([FromBody] Journey journey)
        {
            _logger.LogInformation($"adding journey {journey}");
            _appService.AddJourneyAsync(journey);
        }

        // PUT api/<PassengersController>/5
        [HttpPut("{id}")]
        public async void Put(int id, [FromBody] Journey journey)
        {
            journey.Id = id;
            _logger.LogInformation($"editing journey {journey}");
            await _appService.EditJourneyAsync(journey);
        }

        // DELETE api/<PassengersController>/5
        [HttpDelete("{id}")]
        public async void Delete(int id)
        {
            _logger.LogInformation("dropping a journey...");
            await _appService.DeleteJourneyAsync(id);
        }
    }
}
