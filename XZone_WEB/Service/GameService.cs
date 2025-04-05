using Newtonsoft.Json;
using System.Net.Http.Headers;
using XZone_WEB.Models.DTO.GameDTOs;
using XZone_WEB.Service.IService;
using XZoneUtility;

namespace XZone_WEB.Service
{
    public class GameService : BaseService, IGameService
    {

        private readonly IHttpClientFactory _factory;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _imagesPath;

        private string _GameURL;
        public GameService(IHttpClientFactory httpClientFactory, IConfiguration config, IWebHostEnvironment webHostEnvironment) : base(httpClientFactory)
        {
            _factory = httpClientFactory;
            _GameURL = config.GetValue<string>("ServiceUrls:XZoneAPI");
            _webHostEnvironment = webHostEnvironment;
            _imagesPath = $"{webHostEnvironment.ContentRootPath}/assests/Images/Games";
        }

        public async Task<T> CreateAsync<T>(GameCreateDTO GameDto)
        {
            var coverName = $"{Guid.NewGuid()}{Path.GetExtension(GameDto.ImageURL.FileName)}";
            var path = Path.Combine(_imagesPath, coverName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await GameDto.ImageURL.CopyToAsync(stream);
            }

            // Set image path in DTO to be stored in the API/database
            GameDto.ImagePath = "/images/" + coverName;

            // Avoid sending file content to API
            GameDto.ImageURL = null;

            return await SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.Post,
                Data = GameDto,
                URL = _GameURL + "/api/Game/",
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
