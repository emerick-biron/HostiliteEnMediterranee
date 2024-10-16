using HostiliteEnMediterranee.Models.Dto;
using HostiliteEnMediterranee.Models.Requests;
using HostiliteEnMediterranee.Models.Responses;
using System.Net.Http.Json;


namespace HostiliteEnMediterranee.Client.Services
{
    public class GameApiService
    {
        private readonly HttpClient _httpClient;
        private readonly bool DebugMode = true;

        public GameApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<StartGameResponse> StartGameAsync()
        {
            if (DebugMode)
            {
                List<ShipDto> ships = new List<ShipDto>();
                List<CoordinatesDto> coords = new List<CoordinatesDto>();
                coords.Add(new CoordinatesDto(0, 0));
                coords.Add(new CoordinatesDto(1, 0));
                coords.Add(new CoordinatesDto(2, 0));
                
                ships.Add(new ShipDto('C', coords));
                return new StartGameResponse(Guid.NewGuid(), ships);
            }
            var response = await _httpClient.PostAsync("/api/games/start", null);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<StartGameResponse>();
        }

        public async Task<ShootingResponse> SendShootAsync(ShootingRequest shootingRequest, Guid gameId)
        {
            if (DebugMode)
            {
                Random random = new Random();
                bool hasHit = random.Next(0, 6) == 1;
                List<CoordinatesDto> coords = new List<CoordinatesDto>();
                if (!hasHit) {
                    coords.Add(new CoordinatesDto(random.Next(0, 3), 0));
                }
                return new ShootingResponse(GameStatusDto.InProgress, "", hasHit, coords);
            }
            var response = await _httpClient.PostAsJsonAsync($"/api/games/{gameId}/shoot", shootingRequest);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ShootingResponse>();
        }
    }
}
