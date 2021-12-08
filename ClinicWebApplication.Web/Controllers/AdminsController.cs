using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClinicWebApplication.DataLayer.Models;
using ClinicWebApplication.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using ClinicWebApplication.BusinessLayer.Services.AuthenticationService;

namespace ClinicWebApplication.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        private readonly IRepository<Admin> _adminRepository;
        private readonly IAuthService<Admin> _authService;
        private readonly ILogger<AdminsController> _logger;

        public AdminsController(IRepository<Admin> adminRepository, IAuthService<Admin> authService, ILogger<AdminsController> logger)
        {
            _adminRepository = adminRepository;
            _authService = authService;
            _logger = logger;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public IActionResult Authenticate([FromForm] AuthenticateModel model)
        {
            var admin = _authService.Authenticate(model.Login, model.Password);

            if (admin == null) return BadRequest(new { message = "Email or password is incorrect" });

            _logger.LogInformation($"Admin \"{model.Login}\" was authenticated");

            return Ok(admin);
        }
    }
}
