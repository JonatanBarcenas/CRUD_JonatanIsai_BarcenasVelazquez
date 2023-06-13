using GymManger.DataAccess.Repositories;
using Journeys.ApplicationServices.Journeys;
using Journeys.Core;
using Journeys.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Journeys.UnitTest
{
    [TestFixture]
    public class JourneysTest
    {
        private IJourneyAppService _journeyAppService;
        private IRepository<int, Journey> _repository;

        [SetUp]
        public void Setup()
        {
           
            var services = new ServiceCollection();
            services.AddDbContext<JourneyDataContext>(options =>
                options.UseInMemoryDatabase("JourneysDatabase"));

            services.AddTransient<IRepository<int, Journey>, Repository<int, Journey>>();
            services.AddTransient<IJourneyAppService, JourneyAppService>();

            var serviceProvider = services.BuildServiceProvider();
            _repository = serviceProvider.GetService<IRepository<int, Journey>>();
            _journeyAppService = serviceProvider.GetService<IJourneyAppService>();
        }

        [Test]
        public async Task AddJourneyAsync_AddsJourneyToRepository()
        {
        
            var journey = new Journey
            {
                Id = 1,
                DestinationId = 2,
                OriginId = 3,
                Depature = DateTime.UtcNow,
                Arrival = DateTime.UtcNow.AddHours(2)
            };

    
            await _journeyAppService.AddJourneyAsync(journey);

            var addedJourney = await _repository.GetAsync(journey.Id);
            Assert.NotNull(addedJourney);
            Assert.AreEqual(journey.DestinationId, addedJourney.DestinationId);
            Assert.AreEqual(journey.OriginId, addedJourney.OriginId);
            Assert.AreEqual(journey.Depature, addedJourney.Depature);
            Assert.AreEqual(journey.Arrival, addedJourney.Arrival);
        }

        [Test]
        public async Task DeleteJourneyAsync_RemovesJourneyFromRepository()
        {
          
            var journey = new Journey
            {
                Id = 1,
                DestinationId = 2,
                OriginId = 3,
                Depature = DateTime.UtcNow,
                Arrival = DateTime.UtcNow.AddHours(2)
            };
            await _repository.AddAsync(journey);

            await _journeyAppService.DeleteJourneyAsync(journey.Id);

            var deletedJourney = await _repository.GetAsync(journey.Id);
            Assert.IsNull(deletedJourney);
        }

        [Test]
        public async Task EditJourneyAsync_UpdatesJourneyInRepository()
        {
           
            var journey = new Journey
            {
                Id = 1,
                DestinationId = 2,
                OriginId = 3,
                Depature = DateTime.UtcNow,
                Arrival = DateTime.UtcNow.AddHours(2)
            };
            await _repository.AddAsync(journey);

           
            journey.DestinationId = 4;

           
            await _journeyAppService.EditJourneyAsync(journey);

           
            var updatedJourney = await _repository.GetAsync(journey.Id);
            Assert.NotNull(updatedJourney);
            Assert.AreEqual(journey.DestinationId, updatedJourney.DestinationId);
        }

        [Test]
        public async Task GetJourneyAsync_ReturnsJourneyFromRepository()
        {
          
            var journey = new Journey
            {
                Id = 1,
                DestinationId = 2,
                OriginId = 3,
                Depature = DateTime.UtcNow,
                Arrival = DateTime.UtcNow.AddHours(2)
            };
            await _repository.AddAsync(journey);

            var retrievedJourney = await _journeyAppService.GetJourneyAsync(journey.Id);

            Assert.NotNull(retrievedJourney);
            Assert.AreEqual(journey.DestinationId, retrievedJourney.DestinationId);
            Assert.AreEqual(journey.OriginId, retrievedJourney.OriginId);
            Assert.AreEqual(journey.Depature, retrievedJourney.Depature);
            Assert.AreEqual(journey.Arrival, retrievedJourney.Arrival);
        }

        [Test]
        public async Task GetJourneysAsync_ReturnsAllJourneysFromRepository()
        {
           
            var journeys = new List<Journey>
            {
                new Journey
                {
                    Id = 1,
                    DestinationId = 2,
                    OriginId = 3,
                    Depature = DateTime.UtcNow,
                    Arrival = DateTime.UtcNow.AddHours(2)
                },
                new Journey
                {
                    Id = 2,
                    DestinationId = 4,
                    OriginId = 5,
                    Depature = DateTime.UtcNow,
                    Arrival = DateTime.UtcNow.AddHours(3)
                }
            };

            foreach (var journey in journeys)
            {
                await _repository.AddAsync(journey);
            }

            var retrievedJourneys = await _journeyAppService.GetJourneysAsync();

            Assert.NotNull(retrievedJourneys);
            Assert.AreEqual(journeys.Count, retrievedJourneys.Count);

            foreach (var journey in journeys)
            {
                Assert.IsTrue(retrievedJourneys.Any(j => j.Id == journey.Id
                    && j.DestinationId == journey.DestinationId
                    && j.OriginId == journey.OriginId
                    && j.Depature == journey.Depature
                    && j.Arrival == journey.Arrival));
            }
        }
    }
}