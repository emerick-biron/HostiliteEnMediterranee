using HostiliteEnMediterranee.Models.Dto;
using HostiliteEnMediterranee.Models.Requests;
using HostiliteEnMediterranee.Models.Responses;
using System.Net.Http.Json;


namespace HostiliteEnMediterranee.Client.Services
{
    public class GameApiService
    {
        private readonly HttpClient _httpClient;

        public GameApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<StartGameResponse> StartGameAsync()
        {
            var response = await _httpClient.PostAsync("/api/games/start", null);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<StartGameResponse>();
            //List<ShipDto> ships = new List<ShipDto>();
            //List<CoordinatesDto> coords = new List<CoordinatesDto>();
            //coords.Add(new CoordinatesDto(0, 0));
            //coords.Add(new CoordinatesDto(0, 1));
            //coords.Add(new CoordinatesDto(0, 2));
            //
            //ships.Add(new ShipDto('A', coords));
            //return new StartGameResponse(Guid.NewGuid(), ships);
        }

        public async Task<ShootingResponse> SendShootAsync(ShootingRequest shootingRequest)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/game/shoot", shootingRequest);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ShootingResponse>();
        }
    }
}
