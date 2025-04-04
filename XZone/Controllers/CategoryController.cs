using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;
using XZone.Models;
using XZone.Models.DTO;
using XZone.Repository.IRepository;

namespace XZone.Controllers
{
    [Route("api/[controller]")]
   // [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository repository;
        private readonly IMapper mapper;
        protected ApiResponse _response;

        public CategoryController(ICategoryRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
            this._response = new ApiResponse();
        }

        //  [ProducesResponseType(StatusCodes.Status403Forbidden)]
        // [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAll()
        {
            var CategoryList = await repository.GetAllAsync();
            if (CategoryList == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);

            }
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            var newCategory = mapper.Map<List<CategoryDTO>>(CategoryList);
            _response.Result = newCategory;
            return Ok(_response);
        }

        [HttpGet("{Id}")]

        public async Task<ActionResult<ApiResponse>> GetCategoryById(int Id)
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
            var category = await repository.GetAsync(x => x.Id == Id);
            if (category == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("Category Not Found");
                return NotFound(_response);

            }
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.Result = mapper.Map<CategoryDTO>(category);
            return Ok(_response);

        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateCategory([FromBody] CategoryCreateDto categoryCreateDto)
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
            var NewCategory = mapper.Map<Category>(categoryCreateDto);
            await repository.CreateAsync(NewCategory);
            _response.StatusCode = HttpStatusCode.Created;
            _response.IsSuccess = true;
            return Ok(_response);

        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult<ApiResponse>> DeleteCategory(int Id)
        {

            if (Id == 0)
            {
                return BadRequest();
            }
            var category = await repository.GetAsync(x => x.Id == Id);
            if (category == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("Category Not Found");
                return NotFound(_response);

            }
            await repository.DeleteAsync(category);
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpPut("{Id}")]

        public async Task<ActionResult<ApiResponse>> UpdateCategory(int Id, [FromBody] CategoryUpdatedDTO categoryUpdated)
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
            var categoryInDb = await repository.GetAsync(x => x.Id==Id);
            if(categoryInDb == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("Category Not Found");
                return NotFound(_response);
            }

            categoryInDb.Name = categoryUpdated.Name;
            await repository.UpdateAsync(categoryInDb);
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.Result = categoryUpdated;
            return Ok(_response);



        }
    }
}