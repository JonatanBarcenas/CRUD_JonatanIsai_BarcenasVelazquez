using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Passengers.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tickets.ApplicationServices.Tickets;
using Tickets.Core;
using Tickets.Core.Checkers;

namespace Tickets.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
       private readonly ITicketsAppService _appService;
        private ILogger _logger;
        private readonly Checker _checker;

        public TicketsController(ITicketsAppService appService, ILogger<TicketsController> logger, Checker checker)
        {
            _appService = appService;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _checker = checker;
        }

        // GET: api/<PassengersController>
        [HttpGet]
        public async Task<IEnumerable<Ticket>> Get()
        {
            var ticket = await _appService.GetTicketsAsync();

            _logger.LogInformation("Total tickets retrieved: " + ticket?.Count);
            return ticket;
        }

        // GET api/<PassengersController>/5
        [HttpGet("{id}")]
        public async Task<Ticket> Get(int id)
        {
            var ticket = await _appService.GetTicketAsync(id);
            _logger.LogInformation("Get ticket: " + id);
            return ticket;
        }

        // POST api/<PassengersController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Ticket ticket)
        {
            _logger.LogInformation($"Adding ticket: {ticket}");

            var journeyExists = await _checker.CheckJourneyExists(ticket.JourneyId);
            var passengerExists = await _checker.CheckPassengerExists(ticket.PassengerId);

            if (!journeyExists || !passengerExists)
            {
                _logger.LogInformation("Journey or Passenger does not exist. Cannot create ticket.");
                return BadRequest("Journey or Passenger does not exist.");
            }

            await _appService.AddTicketAsync(ticket);
            return Ok();
        }

        // PUT api/<PassengersController>/5
        [HttpPut("{id}")]
        public async void Put(int id, [FromBody] Ticket ticket)
        {
            ticket.Id = id;
            _logger.LogInformation($"editing ticket {ticket}");
            await _appService.EditTicketAsync(ticket);
        
        }

        // DELETE api/<PassengersController>/5
        [HttpDelete("{id}")]
        public async void Delete(int id)
        {
            _logger.LogInformation("dropping a ticket...");
            await _appService.DeleteTicketAsync(id);
  
        }
    }
}
