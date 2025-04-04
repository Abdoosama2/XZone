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
        private readonly IDeviceService deviceService;
        private readonly ICategoryService categoryService1;

        public GameController(IGameService gameService, ICategoryService categoryService,IDeviceService deviceService )
        {
           // this.mapper = mapper;
            this.gameService = gameService;
            this.categoryService = categoryService;
            this.deviceService = deviceService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Create()
        {
            var categoryResponse = await categoryService.GetAllAsync<ApiResponse>();
            List<CategoryDTO> categories = new List<CategoryDTO>();
            if (categoryResponse != null && categoryResponse.IsSuccess)
            {
                categories = JsonConvert.DeserializeObject<List<CategoryDTO>>(Convert.ToString(categoryResponse.Result));
            }

            var deviceResponse = await deviceService.GetAllAsync<ApiResponse>();
            List<DeviceDTO> devices = new List<DeviceDTO>();
            if (deviceResponse != null && deviceResponse.IsSuccess)
            {
                devices = JsonConvert.DeserializeObject<List<DeviceDTO>>(Convert.ToString(deviceResponse.Result));
            }


            var gameCreateDTO = new GameCreateDTO
            {
                Categories = categories.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }),
                devices= devices.Select(d=> new SelectListItem
                {
                    Text = d.Name,
                    Value=d.Id.ToString()
                })

               
            };



            return View(gameCreateDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GameCreateDTO gameCreateDTO)
        {

            if (ModelState.IsValid)
            {
                var response = await gameService.CreateAsync<ApiResponse>(gameCreateDTO);
                if (response != null && response.IsSuccess)
                {
                    // TempData["success"] = "Game Created Successfully";
                    return RedirectToAction("index");


                }
            }
              //  TempData["error"] = "An error occuerd";
                return RedirectToAction("CreateVilla", gameCreateDTO);

            

        }
    }
}
