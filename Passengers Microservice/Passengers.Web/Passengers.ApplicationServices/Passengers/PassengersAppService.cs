using GymManger.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Passengers.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Passengers.ApplicationServices.Passengers
{
    public class PassengersAppService : IPassengersAppService
    {
        private readonly IRepository<int, Passenger> _repository;

        public PassengersAppService(IRepository<int, Passenger> repository)
        {
            _repository = repository;
        }
       

        public async Task<int> AddPassengerAsync(Passenger passenger)
        {
           await _repository.AddAsync(passenger);
            return passenger.Id;
        }

        public async Task DeletePassengerAsync(int memberId)
        {
           await _repository.DeleteAsync(memberId);
        }

        public async Task EditPassengerAsync(Passenger passenger)
        {
           await _repository.UpdateAsync(passenger);
        }

        public async Task<Passenger> GetPassengerAsync(int memberId)
        {
           return await _repository.GetAsync(memberId);
        }

        public async Task<List<Passenger>> GetPassengersAsync()
        {
            return await _repository.GetAll().ToListAsync();
        }
    }
}
