using GymManger.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tickets.ApplicationServices.Tickets;
using Tickets.Core;
using Tickets.DataAccess;

namespace Tickets.UnitTest
{
    public class TicketsTests
    {
        private ITicketsAppService _ticketsAppService;
        private IRepository<int, Ticket> _repository;

        [SetUp]
        public void Setup()
        {
            
            var services = new ServiceCollection();
            services.AddDbContext<TicketsDataContext>(options =>
                options.UseInMemoryDatabase("TicketsDatabase"));

            services.AddTransient<IRepository<int, Ticket>, Repository<int, Ticket>>();
            services.AddTransient<ITicketsAppService, TicketsAppService>();

            var serviceProvider = services.BuildServiceProvider();
            _repository = serviceProvider.GetService<IRepository<int, Ticket>>();
            _ticketsAppService = serviceProvider.GetService<ITicketsAppService>();
        }

        [Test]
        public async Task AddTicketAsync_AddsTicketToRepository()
        {
            
            var ticket = new Ticket
            {
                Id = 1,
                JourneyId = 2,
                PassengerId = 3,
                Seat = 4
            };

            
            await _ticketsAppService.AddTicketAsync(ticket);

           
            var addedTicket = await _repository.GetAsync(ticket.Id);
            Assert.NotNull(addedTicket);
            Assert.AreEqual(ticket.JourneyId, addedTicket.JourneyId);
            Assert.AreEqual(ticket.PassengerId, addedTicket.PassengerId);
            Assert.AreEqual(ticket.Seat, addedTicket.Seat);
        }

        [Test]
        public async Task DeleteTicketAsync_RemovesTicketFromRepository()
        {
           
            var ticket = new Ticket
            {
                Id = 1,
                JourneyId = 2,
                PassengerId = 3,
                Seat = 4
            };
            await _repository.AddAsync(ticket);

            
            await _ticketsAppService.DeleteTicketAsync(ticket.Id);

           
            var deletedTicket = await _repository.GetAsync(ticket.Id);
            Assert.IsNull(deletedTicket);
        }

        [Test]
        public async Task EditTicketAsync_UpdatesTicketInRepository()
        {
           
            var ticket = new Ticket
            {
                Id = 1,
                JourneyId = 2,
                PassengerId = 3,
                Seat = 4
            };
            await _repository.AddAsync(ticket);

           
            ticket.Seat = 5;

          
            await _ticketsAppService.EditTicketAsync(ticket);

            
            var updatedTicket = await _repository.GetAsync(ticket.Id);
            Assert.NotNull(updatedTicket);
            Assert.AreEqual(ticket.Seat, updatedTicket.Seat);
        }

        [Test]
        public async Task GetTicketAsync_ReturnsTicketFromRepository()
        {
           
            var ticket = new Ticket
            {
                Id = 1,
                JourneyId = 2,
                PassengerId = 3,
                Seat = 4
            };
            await _repository.AddAsync(ticket);

       
            var retrievedTicket = await _ticketsAppService.GetTicketAsync(ticket.Id);

           
            Assert.NotNull(retrievedTicket);
            Assert.AreEqual(ticket.JourneyId, retrievedTicket.JourneyId);
            Assert.AreEqual(ticket.PassengerId, retrievedTicket.PassengerId);
            Assert.AreEqual(ticket.Seat, retrievedTicket.Seat);
        }

        [Test]
        public async Task GetTicketsAsync_ReturnsAllTicketsFromRepository()
        {
           
            var tickets = new List<Ticket>
            {
                new Ticket
                {
                    Id = 1,
                    JourneyId = 2,
                    PassengerId = 3,
                    Seat = 4
                },
                new Ticket
                {
                    Id = 2,
                    JourneyId = 5,
                    PassengerId = 6,
                    Seat = 7
                }
            };

            foreach (var ticket in tickets)
            {
                await _repository.AddAsync(ticket);
            }

            
            var retrievedTickets = await _ticketsAppService.GetTicketsAsync();

         
            Assert.NotNull(retrievedTickets);
            Assert.AreEqual(tickets.Count, retrievedTickets.Count);

            foreach (var ticket in tickets)
            {
                Assert.IsTrue(retrievedTickets.Any(t => t.Id == ticket.Id
                    && t.JourneyId == ticket.JourneyId
                    && t.PassengerId == ticket.PassengerId
                    && t.Seat == ticket.Seat));
            }
        }
    }
}