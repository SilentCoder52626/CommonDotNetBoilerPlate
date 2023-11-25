using DomainModule.Dto;
using DomainModule.ServiceInterface;
using Microsoft.AspNetCore.Mvc;
using WebApp.Helper;
using WebApp.Models;

namespace WebApp.Areas.API.Controllers
{
    [Route("api/Setting")]
    [ApiController]
    public class SettingApiController : Controller
    {
        private readonly AppSettingsServiceInterface _appSettingService;

        public SettingApiController(AppSettingsServiceInterface appSettingRepo)
        {
            _appSettingService = appSettingRepo;
        }
        [HttpPost("Update")]
        public IActionResult Update(List<AppSettingDto> model)
        {
            try
            {
                _appSettingService.BulkUpdateSetting(model);
                return Ok(new ApiResponseModel()
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Setting Updated.",
                });

            }
            catch(Exception ex)
            {
                CommonLogger.LogError(ex.Message, ex);
                return BadRequest(ex);
            }
        }
    }
}
