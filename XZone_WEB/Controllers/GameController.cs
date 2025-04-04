using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;
using XZone_WEB.Models;
using XZone_WEB.Models.DTO;
using XZone_WEB.Models.DTO.GameDTOs;
using XZone_WEB.Service.IService;
using XZoneUtility;

namespace XZone_WEB.Controllers
{
    public class GameController : Controller
    {
        //private readonly IMapper mapper;
        private readonly IGameService gameService;
        private readonly ICategoryService categoryService;

        public GameController(IGameService gameService, ICategoryService categoryService)
        {
           // this.mapper = mapper;
            this.gameService = gameService;
            this.categoryService = categoryService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Create()
        {
            var response = await categoryService.GetAllAsync<ApiResponse>();
            List<CategoryDTO> categories = new List<CategoryDTO>();
            if (response != null && response.IsSuccess)
            {
                categories = JsonConvert.DeserializeObject<List<CategoryDTO>>(Convert.ToString(response.Result));


            }
            var gameCreateDTO = new GameCreateDTO
            {
                Categories = categories.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                })
            };


            return View();
        }

        //[HttpPost]

        //public async Task<IActionResult> Create(GameCreateDTO gameCreateDTO)
        //{
            
        //}
    }
}
