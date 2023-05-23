using Passengers.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Passengers.ApplicationServices.Passengers
{
    public interface IPassengersAppService
    {
        Task<List<Passenger>> GetPassengersAsync();

        Task<int> AddPassengerAsync(Passenger passenger);

        Task DeletePassengerAsync(int memberId);

        Task<Passenger> GetPassengerAsync(int memberId);

        Task EditPassengerAsync(Passenger passenger);
    }
}
