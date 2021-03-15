using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using RateGame.DAL;
using RateGame.Services.DTO;

namespace RateGame
{
    public interface IRawgIOService
    {
        Task<List<Game>> Download();
    }

    public class RawgIOService : IRawgIOService
    {
        const int PageSize = 40;
        private IHttpClientFactory _httpClientFactory;

        public RawgIOService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<Game>> Download()
        {
            //todo constants
            HttpClient client = _httpClientFactory.CreateClient("rawgio");

            var request = new HttpRequestMessage { Method = HttpMethod.Get };
            //TODO batch
            request.RequestUri = new System.Uri($"?page_size={PageSize}", System.UriKind.Relative);
            var response = await client.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var result = await response.Content
                .ReadFromJsonAsync<RawgIOGamesDTO>(new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true, MaxDepth = 200 });

            var mapped = result.results.Select(x => MapFromDTO(x));

            return mapped.ToList();
        }

        private static Game MapFromDTO(RawgIOGamesDTO.Result gameDto)
        {
            return new Game
            {
                Title = gameDto.name,
                UrlSlug = gameDto.slug,
                Year = gameDto.released
            };
        }
    }
}
