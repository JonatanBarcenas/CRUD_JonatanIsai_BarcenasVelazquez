using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Tickets.Core.Checkers
{
    public class Checker
    {

        private readonly HttpClient _httpClient;
        private readonly string _jwtToken;

        public Checker(HttpClient httpClient, string jwtToken)
        {
            _httpClient = httpClient;
            _jwtToken = jwtToken;
        }

        public async Task<bool> CheckJourneyExists(int journeyId)
        {

            var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:44389/api/Journeys/{journeyId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken);

            var response = await _httpClient.SendAsync(request);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> CheckPassengerExists(int passengerId)
        {

            var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:44312/api/Passengers/{passengerId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken);

            var response = await _httpClient.SendAsync(request);

            return response.IsSuccessStatusCode;
        }
    }
}
