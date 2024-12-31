using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using XZone.Models;
using XZone.Models.DTO;
using XZone.Models.DTO.DeviceDTOs;
using XZone.Repository;
using XZone.Repository.IRepository;

namespace XZone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceRepository deviceRepository;
        private readonly IMapper mapper;
        private ApiResponse _response;

        public DeviceController(IDeviceRepository deviceRepository, IMapper mapper)
        {
            this.deviceRepository = deviceRepository;
            this.mapper = mapper;
            this._response = new ApiResponse();
        }

        [HttpGet("GetAll")]

        public async Task<ActionResult<ApiResponse>> GetAll()
        {
            var DeviceList = await deviceRepository.GetAllAsync();
            if (DeviceList == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);

            }
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            var newDevice = mapper.Map<List<DeviceDTO>>(DeviceList);
            _response.Result = newDevice;
            return Ok(_response);
        }



        [HttpGet("GetDeviceById{Id}")]

        public async Task<ActionResult<ApiResponse>> GetDeviceById(int Id)
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
            var Device = await deviceRepository.GetAsync(x => x.Id == Id);
            if (Device == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("Device Not Found");
                return NotFound(_response);

            }
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.Result = mapper.Map<DeviceDTO>(Device);
            return Ok(_response);

        }

        [HttpPost("AddDevice")]
        public async Task<ActionResult<ApiResponse>> CreateDevice([FromBody] DeviceCreateDTO DeviceCreateDto)
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
            var NewDevice = mapper.Map<Device>(DeviceCreateDto);
            await deviceRepository.CreateAsync(NewDevice);
            _response.StatusCode = HttpStatusCode.Created;
            _response.IsSuccess = true;
            return Ok(_response);

        }

        [HttpDelete("DeleteDevice{Id}")]
        public async Task<ActionResult<ApiResponse>> DeleteDevice(int Id)
        {

            if (Id == 0)
            {
                return BadRequest();
            }
            var Device = await deviceRepository.GetAsync(x => x.Id == Id);
            if (Device == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("Device Not Found");
                return NotFound(_response);

            }
            await deviceRepository.DeleteAsync(Device);
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpPut("UpdateDevice{Id}")]

        public async Task<ActionResult<ApiResponse>> UpdateDevice(int Id, [FromBody] DeviceUpdatedDTO DeviceUpdated)
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
            var DeviceInDb = await deviceRepository.GetAsync(x => x.Id == Id);
            if (DeviceInDb == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("Device Not Found");
                return NotFound(_response);
            }

                DeviceInDb = mapper.Map(DeviceUpdated, DeviceInDb);
            await deviceRepository.UpdateAsync(DeviceInDb);
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.Result = DeviceUpdated;
            return Ok(_response);



        }

    }
}
