using Azure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoWebApis.DTOs;
using ProyectoWebApis.Services;

namespace ProyectoWebApis.Controllers
{
    [ApiController]
    [Route("Api/[Controller]")]
    public class OperationController : Controller
    {
        private readonly OperationService _operationService;

        public OperationController(OperationService operationService)
        {
            _operationService = operationService;
        }

        [HttpPost("CreateOperation")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult CreateOperation([FromBody] OperationCreateDTO createDTO)
        {
            if(createDTO == null)
            {
                ModelState.AddModelError("", "Operation is null!");
                return StatusCode(422, ModelState);
            }

            var result = _operationService.CreateOperation(createDTO);

            if(result.Succes) 
            {
                return Ok("Succesfully created");
            }
            else
            {
                return StatusCode(result.StatusCode, result.ErrorMessage);
            }
        }

        [HttpGet("GetOperations")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<OperationToShowDTO>))]
        public IActionResult GetAllOperations()
        {
            var operations = _operationService.GetAllOperations();
          
            return Ok(operations);
        }
    }
}
