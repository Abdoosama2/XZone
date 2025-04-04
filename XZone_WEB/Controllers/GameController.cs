using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using XZone_WEB.Models;
using XZone_WEB.Service.IService;
using XZoneUtility;

namespace XZone_WEB.Controllers
{
    public class GameDto : Controller
    {
        private readonly IGameService gameService;
        private readonly IMapper mapper;

        public GameDto(IGameService gameService ,IMapper mapper)
        {
            this.gameService = gameService;
            this.mapper = mapper;
           
        }


        public async Task<IActionResult> GetAll()
        {
            List<GameDto> GamesList = new List<GameDto>();

            var response = await gameService.GetAllAsync<ApiResponse>();

            if(response!=null &&response.IsSuccess)
            {
                GamesList = JsonConvert.DeserializeObject<List<GameDto>>(Convert.ToString(response.Result));
            }
            return View(GamesList);
        }
    }
}
