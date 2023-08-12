using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProyectoWebApis.DTOs;
using ProyectoWebApis.Helpers;
using ProyectoWebApis.Models;
using ProyectoWebApis.Services;

namespace ProyectoWebApis.Controllers
{
    [ApiController]
    [Route("Api/[Controller]")]
    public class RecordController : Controller
    {
        private readonly RecordService _recordService;
        private readonly UserManager<User> _userManager;
        private readonly OperationService _operationService;

        public RecordController(RecordService recordService, UserManager<User> userManager, OperationService operationService)
        {
            _recordService = recordService;
            _userManager = userManager;
            _operationService = operationService;
        }

        [HttpPost("GenerateRecord")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GenerateRecord(RecordCreateDTO createDTO, [FromQuery] string UserId)
        {
            if (createDTO.NumberOne.Equals("0") || createDTO.NumberTwo.Equals("0"))
            {

                return BadRequest();
            }
            var userIdentity = HttpContext.User.Identity;

            if (userIdentity.IsAuthenticated)
            {
                var user = await _userManager.FindByIdAsync(UserId);

                var (success, statusCode, errorMessages) = await _recordService.CreateRecordAsync(createDTO, user);

                if (success)
                {
                    OperationResultDTO op = new OperationResultDTO()
                    {
                        dynamicOne = createDTO.NumberOne,
                        dynamicTwo = createDTO.NumberTwo,
                        dynamicThree = _operationService.GenerateCalculation(createDTO.Operation_Id, createDTO.NumberOne, createDTO.NumberTwo)
                    };

                    return Ok(op);
                }
                return StatusCode(statusCode, new { errors = errorMessages });
            }
            return Unauthorized();
        }

        [HttpPut("SoftDelete")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult DeleteRecord([FromQuery] int recordId)
        {
            if (_recordService.GetRecordById(recordId) == null)
            {
                return BadRequest();
            }
            var (success, statusCode, errorMessages) = _recordService.Delete(recordId);

            return Ok(errorMessages);
        }

        [HttpGet("GetAllRecordByUser")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<RecordToShowDTO>))]
        public async Task<ActionResult<List<RecordToShowDTO>>> GetAllMyRecors([FromQuery] PaginationDTO paginationDTO)
        {
            var queryable = _recordService.GetQueryable();
            await HttpContext.InsertPaginationParametersInHeader(queryable);
            return await _recordService.GetAll(paginationDTO);
        }

    }
}


