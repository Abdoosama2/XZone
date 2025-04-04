using XZone_WEB.Models.DTO.GameDTOs;
using XZone_WEB.Service.IService;
using XZoneUtility;

namespace XZone_WEB.Service
{
    public class GameService : BaseService, IGameService
    {

        private readonly IHttpClientFactory _factory;
        private string _GameURL;
        public GameService(IHttpClientFactory httpClientFactory ,IConfiguration config) : base(httpClientFactory)
        {
            _factory = httpClientFactory;
            _GameURL = config.GetValue<string>("ServiceUrls:XZoneAPI");
        }

        public Task<T> CreateAsync<T>(GameCreateDTO GameDto, string token)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType=SD.ApiType.Post,
                Data= GameDto,
                URL=_GameURL+"/api/Game/",
                Token=token,

            });
        }

        public Task<T> DeleteAsync<T>(int id, string token)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.Delete,
              
                URL = _GameURL + "/api/Game/"+id,
                Token = token,

            });
        }

        public Task<T> GetAllAsync<T>()
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.Get,
              
                URL = _GameURL + "/api/Game/",
               // Token = token,

            });
        }

        public Task<T> GetAsync<T>(int id, string token)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.Get,
               
                URL = _GameURL + "/api/Game/"+id,
                Token = token,

            });
        }

        public Task<T> UpdateAsync<T>(GameUpdateDTO GameDto, string token)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.Put,
                Data = GameDto,
                URL = _GameURL + "/api/Game/"+GameDto.Id,
                Token = token,

            });
        }
    }
}
