using Journeys.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journeys.ApplicationServices.Journeys
{
    public interface IJourneyAppService
    {
        Task<List<Journey>> GetJourneysAsync();

        Task AddJourneyAsync(Journey journey);

        Task DeleteJourneyAsync(int journeyId);

        Task<Journey> GetJourneyAsync(int journeyId);

        Task EditJourneyAsync(Journey journey);
    }
}
