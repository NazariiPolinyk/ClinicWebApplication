using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClinicWebApplication.DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using ClinicWebApplication.Interfaces;
using ClinicWebApplication.Web.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using ClinicWebApplication.BusinessLayer.Services.AuthenticationService;
using ClinicWebApplication.BusinessLayer.Services.InputValidationService;
using ClinicWebApplication.BusinessLayer.Specification.DoctorSpecification;
using ClinicWebApplication.BusinessLayer.Services.EmailService;



namespace ClinicWebApplication.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IRepository<Doctor> _doctorRepository;
        private readonly IMapper _mapper;
        private readonly IAuthService<Doctor> _authService;
        private readonly ILogger<DoctorsController> _logger;
        private readonly IMailService _mailService;

        public DoctorsController(IRepository<Doctor> doctorRepository, IMapper mapper, IAuthService<Doctor> authService, ILogger<DoctorsController> logger, IMailService mailService)
        {
            _doctorRepository = doctorRepository;
            _mapper = mapper;
            _authService = authService;
            _logger = logger;
            _mailService = mailService;
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public IActionResult Authenticate([FromForm] AuthenticateModel model)
        {
            var doctor = _authService.Authenticate(model.Login, model.Password);

            if (doctor == null) return BadRequest(new { message = "Email or password is incorrect" });

            _logger.LogInformation($"Doctor \"{model.Login}\" was authenticated");

            return Ok(doctor);
        }
        [HttpGet]
        [Authorize(Roles = "Patient, Admin")]
        public async Task<IEnumerable<DoctorViewModel>> Get()
        {
            var doctors = await _doctorRepository.GetAll();

            _logger.LogInformation($"{User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value} " +
                $" \"{this.User.Identity.Name}[{User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value}]\" received information about all doctors.");

            return _mapper.Map<IEnumerable<Doctor>, IEnumerable<DoctorViewModel>>(doctors);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("{id}")]
        [Authorize(Roles = "Patient, Doctor, Admin")]
        public async Task<ActionResult<DoctorViewModel>> Get(int id)
        {
            if (User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value == Role.Doctor &&
                Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value) != id) return BadRequest();
            var doctorsWithSpecification = await _doctorRepository.FindWithSpecification(new DoctorWithFeedbacksSpecification(id));
            var doctor = doctorsWithSpecification.SingleOrDefault();
            if (doctor == null) return NotFound();
            var doctorViewModel = _mapper.Map<DoctorViewModel>(doctor);

            _logger.LogInformation($"{User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value} " +
                $" \"{this.User.Identity.Name}[{User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value}]\" received information about {doctor.Email}[{doctor.Id}] patient.");

            return new ObjectResult(doctorViewModel);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<Doctor>> Post(Doctor doctor)
        {
            if (doctor == null) return BadRequest();
            var validationResult = InputValidation.ValidateDoctor(doctor);
            if (validationResult.result == false) return BadRequest(new { message = validationResult.error });
            await _doctorRepository.Insert(doctor);

            _logger.LogInformation($"New doctor \"{doctor.Email}\" was created.");

            return Ok(doctor);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<Doctor>> Put(Doctor doctor)
        {
            if (doctor == null ||
                doctor.Id != Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value)) return BadRequest();
            if (await _doctorRepository.GetById(doctor.Id) == null) return NotFound();
            await _doctorRepository.Update(doctor);

            _logger.LogInformation($"{User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value} " +
                $" \"{this.User.Identity.Name}[{User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value}]\" changed his data.");

            return Ok(doctor);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Doctor>> Delete(int id)
        {
            Doctor doctor = await _doctorRepository.GetById(id);
            if (doctor == null) return NotFound();
            await _doctorRepository.Delete(doctor);

            _logger.LogInformation($"Doctor \"{doctor.Email}[{doctor.Id}]\" was deleted.");

            return Ok();
        }
    
    }
}
