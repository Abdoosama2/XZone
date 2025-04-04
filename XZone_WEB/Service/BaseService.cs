using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using XZone_WEB.Service.IService;
using XZoneUtility;
using XZone_WEB.Models;
    
namespace XZone_WEB.Service
{
    public class BaseService : IBaseService
    {
        
        public ApiRequest _request { get; set; }
        public IHttpClientFactory httpClientFactory { get; set; }

        public BaseService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
            this._request = new ApiRequest();
            
        }
        public async Task<T> SendAsync<T>(ApiRequest request)
        {
            try
            {
                var client = httpClientFactory.CreateClient("XZone");
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(request.URL);
                if (request.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(request.Data)
                        , Encoding.UTF8, "application/json");

                }
                switch (request.ApiType)
                {
                    case SD.ApiType.Post:
                        message.Method = HttpMethod.Post;
                        break;
                    case SD.ApiType.Put:
                        message.Method = HttpMethod.Put;
                        break;
                    case SD.ApiType.Delete:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;

                }
                HttpResponseMessage apiresonee = null;

                if (!string.IsNullOrEmpty(request.Token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", request.Token);
                }
                apiresonee = await client.SendAsync(message);
                var apicontent = await apiresonee.Content.ReadAsStringAsync();

                try
                {
                    ApiResponse ApiResponse = JsonConvert.DeserializeObject<ApiResponse>(apicontent);
                    if (ApiResponse != null && (apiresonee.StatusCode == System.Net.HttpStatusCode.BadRequest ||
                        apiresonee.StatusCode == System.Net.HttpStatusCode.NotFound))
                    {
                        ApiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                        ApiResponse.IsSuccess = false;
                        var result = JsonConvert.SerializeObject(ApiResponse);
                        var ReturnObj = JsonConvert.DeserializeObject<T>(result);
                        return ReturnObj;

                    }
                }
                catch (Exception ex)
                {

                    var ExpectionResult = JsonConvert.DeserializeObject<T>(apicontent);
                    return ExpectionResult;
                }
                var Apiresponse = JsonConvert.DeserializeObject<T>(apicontent);
                return Apiresponse;
            }
            catch (Exception ex)
            {
                var dto = new ApiResponse()
                {
                    ErrorMessages = new List<string> { Convert.ToString(ex.Message) },
                    IsSuccess = false
                };

                var result = JsonConvert.SerializeObject(dto);
                var ApiResponse = JsonConvert.DeserializeObject<T>(result);
                return ApiResponse;

            }
        


        }
    }
}
