using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using XZone.Models;
using XZone.Models.DTO.GameDTOs;
using XZone.Models.DTO;
using XZone.Repository;
using XZone.Repository.IRepository;

namespace XZone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameRepository gameRepository;
        private readonly IMapper mapper;
        private readonly ICategoryRepository categoryRepository;
        private ApiResponse _response;

        public GameController(IGameRepository gameRepository ,IMapper mapper,ICategoryRepository categoryRepository)
        {
            this.gameRepository = gameRepository;
            this.mapper = mapper;
            this.categoryRepository = categoryRepository;
            this._response = new ApiResponse();
        }
        [HttpGet]

        public async Task<ActionResult<ApiResponse>> GetAll()
        {
            var GameList = await gameRepository.GetAllAsync(IncludeProperties:"Category");
            if (GameList == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);

            }
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            var newGame = mapper.Map<List<GameDTO>>(GameList);
            _response.Result = newGame;
            return Ok(_response);
        }



        [HttpGet("{Id}")]

        public async Task<ActionResult<ApiResponse>> GetGameById(int Id)
        {

            if (Id == 0)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("Invalid id");
                return BadRequest(_response);

            }
            var Game = await gameRepository.GetAsync(x => x.Id == Id);
            if (Game == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("Game Not Found");
                return NotFound(_response);

            }
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.Result = mapper.Map<GameDTO>(Game);
            return Ok(_response);

        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateGame([FromBody] GameCreateDTO GameCreateDto)
        {
            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                var errors = ModelState.Values
             .SelectMany(v => v.Errors)
             .Select(e => e.ErrorMessage)
             .ToList();
                _response.ErrorMessages = errors;
                return BadRequest(_response);
            }

            if (GameCreateDto.CategoryId == 0||(await categoryRepository.GetAsync(x=>x.Id==GameCreateDto.CategoryId)==null))
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("Invalid Category ID");
                return BadRequest(_response);
            }
            var NewGame = mapper.Map<Game>(GameCreateDto);
            await gameRepository.CreateAsync(NewGame);
            _response.StatusCode = HttpStatusCode.Created;
            _response.IsSuccess = true;
            return Ok(_response);

        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult<ApiResponse>> DeleteGame(int Id)
        {

            if (Id == 0)
            {
                return BadRequest();
            }
            var Game = await gameRepository.GetAsync(x => x.Id == Id);
            if (Game == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("Game Not Found");
                return NotFound(_response);

            }
            await gameRepository.DeleteAsync(Game);
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpPut("{Id}")]

        public async Task<ActionResult<ApiResponse>> UpdateGame(int Id, [FromBody] GameUpdateDTO GameUpdated)
        {
            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                var errors = ModelState.Values
             .SelectMany(v => v.Errors)
             .Select(e => e.ErrorMessage)
             .ToList();
                _response.ErrorMessages = errors;
                return BadRequest(_response);
            }
            if (Id == 0)
            {
                return BadRequest();
            }
            if (GameUpdated.CategoryId == 0 || (await categoryRepository.GetAsync(x => x.Id == GameUpdated.CategoryId) == null))
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("Invalid Category ID");
                return BadRequest(_response);
            }
            var GameInDb = await gameRepository.GetAsync(x => x.Id == Id);
            if (GameInDb == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("Game Not Found");
                return NotFound(_response);
            }

            GameInDb = mapper.Map(GameUpdated, GameInDb);
            await gameRepository.UpdateAsync(GameInDb);
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.Result = GameUpdated;
            return Ok(_response);



        }

    }
}
