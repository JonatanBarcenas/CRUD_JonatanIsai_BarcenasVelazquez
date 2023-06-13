using AutoMapper;
using GymManger.DataAccess.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Passengers.ApplicationServices;
using Passengers.ApplicationServices.Passengers;
using Passengers.Core.Passengers;
using Passengers.DataAccess;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Passengers.UnitTest
{
    [TestFixture]
    public class PassengersTest
    {
        private IPassengersAppService _passengersAppService;
        private IRepository<int, Passenger> _repository;
       

        [SetUp]
        public void Setup()
        {
           
            var services = new ServiceCollection();
            services.AddDbContext<PassengersDataContext>(options =>
                options.UseInMemoryDatabase("PassengersDatabase"));

            services.AddTransient<IRepository<int, Passenger>, Repository<int, Passenger>>();
            services.AddTransient<IPassengersAppService, PassengersAppService>();

            var serviceProvider = services.BuildServiceProvider();
            _repository = serviceProvider.GetService<IRepository<int, Passenger>>();
            _passengersAppService = serviceProvider.GetService<IPassengersAppService>();
        }

        [Test]
        public async Task AddPassengerAsync_ShouldAddPassenger()
        {
            
            int memberId = 1;
            var passenger = new Passenger { Id = memberId, FirstName = "John Doe" };

            
            await _passengersAppService.AddPassengerAsync(passenger);

            var addedPassenger = await _passengersAppService.GetPassengerAsync(memberId);
            Assert.IsNotNull(addedPassenger);
            Assert.AreEqual(memberId, addedPassenger.Id);
            Assert.AreEqual(passenger.FirstName, addedPassenger.FirstName);
        }

        [Test]
        public async Task DeletePassengerAsync_ShouldDeletePassenger()
        {
           
            int memberId = 1;
            var passenger = new Passenger { Id = memberId, FirstName = "John Doe" };
            await _passengersAppService.AddPassengerAsync(passenger);

          
            await _passengersAppService.DeletePassengerAsync(memberId);

         
            var deletedPassenger = await _passengersAppService.GetPassengerAsync(memberId);
            Assert.IsNull(deletedPassenger);
        }

        [Test]
        public async Task EditPassengerAsync_ShouldUpdatePassenger()
        {
           
            int memberId = 1;
            var passenger = new Passenger { Id = memberId, FirstName = "John Doe" };
            await _passengersAppService.AddPassengerAsync(passenger);

            string updatedName = "Jane Smith";
            passenger.FirstName = updatedName;
            await _passengersAppService.EditPassengerAsync(passenger);

           
            var updatedPassenger = await _passengersAppService.GetPassengerAsync(memberId);
            Assert.IsNotNull(updatedPassenger);
            Assert.AreEqual(updatedName, updatedPassenger.FirstName);
        }

        [Test]
        public async Task GetPassengerAsync_ShouldReturnPassenger()
        {
            
            int memberId = 1;
            var passenger = new Passenger { Id = memberId, FirstName = "John Doe" };
            await _passengersAppService.AddPassengerAsync(passenger);

          
            var retrievedPassenger = await _passengersAppService.GetPassengerAsync(memberId);
            
            Assert.IsNotNull(retrievedPassenger);
            Assert.AreEqual(memberId, retrievedPassenger.Id);
            Assert.AreEqual(passenger.FirstName, retrievedPassenger.FirstName);
        }

        [Test]
        public async Task GetPassengersAsync_ShouldReturnAllPassengers()
        {
            // Arrange
            var passengers = new List<Passenger>
            {
                new Passenger { Id = 1, FirstName = "John Doe" },
                new Passenger { Id = 2, FirstName = "Jane Smith" }
            };

            foreach (var passenger in passengers)
            {
                await _passengersAppService.AddPassengerAsync(passenger);
            }

            var retrievedPassengers = await _passengersAppService.GetPassengersAsync();

           
            Assert.IsNotNull(retrievedPassengers);
            Assert.AreEqual(passengers.Count, retrievedPassengers.Count);
            foreach (var passenger in passengers)
            {
                Assert.IsTrue(retrievedPassengers.Any(p => p.Id == passenger.Id && p.FirstName == passenger.FirstName));
            }
        }

    }
}