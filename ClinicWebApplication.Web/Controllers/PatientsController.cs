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
using ClinicWebApplication.BusinessLayer.Specification.PatientSpecification;
using ClinicWebApplication.Web.InputModels;

namespace ClinicWebApplication.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IRepository<Patient> _patientRepository;
        private readonly IMapper _mapper;
        private readonly IAuthService<Patient> _authService;
        private readonly ILogger<PatientsController> _logger;

        public PatientsController(IRepository<Patient> patientRepository, IMapper mapper, IAuthService<Patient> authService, ILogger<PatientsController> logger)
        {
            _patientRepository = patientRepository;
            _mapper = mapper;
            _authService = authService;
            _logger = logger;
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public IActionResult Authenticate([FromForm]AuthenticateModel model)
        {
            var patient = _authService.Authenticate(model.Login, model.Password);

            if (patient == null) return BadRequest(new { message = "Email or password is incorrect." });

            _logger.LogInformation($"Patient \"{model.Login}\" was authenticated.");

            return Ok(patient);
        }
        [HttpGet]
        [Authorize(Roles = "Doctor, Admin")]
        public async Task<IEnumerable<PatientViewModel>> Get()
        {
            var patients = await _patientRepository.GetAll();
            _logger.LogInformation($"{User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value} " +
                $" \"{this.User.Identity.Name}[{User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value}]\" received information about all patients.");
            return _mapper.Map<IEnumerable<Patient>, IEnumerable<PatientViewModel>>(patients);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("{id}")]
        [Authorize(Roles = "Doctor, Patient, Admin")]
        public async Task<ActionResult<PatientViewModel>> Get(int id)
        {
            if (User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value == Role.Patient &&
                Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value) != id) return BadRequest();
            var patientsWithSpecification = await _patientRepository.FindWithSpecification(new PatientWithMedicalCardRecordsSpecification(id));
            var patient = patientsWithSpecification.SingleOrDefault();
            if (patient == null) return NotFound();
            var patientViewModel = _mapper.Map<PatientViewModel>(patient);
            _logger.LogInformation($"{User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value} " +
                $" \"{this.User.Identity.Name}[{User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value}]\" received information about {patient.Email}[{patient.Id}] patient.");
            return new ObjectResult(patientViewModel);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<Patient>> Post([FromForm] PatientInputModel patientInputModel)
        {
            if (patientInputModel == null) return BadRequest();
            Patient patient = new Patient
            {
                Name = patientInputModel.Name,
                Phone = patientInputModel.Phone,
                BirthDate = patientInputModel.BirthDate,
                Email = patientInputModel.Email,
                Password = patientInputModel.Password,
                Role = Role.Patient
            };
            var validationResult = InputValidation.ValidatePatient(patient);
            if (validationResult.result == false) return BadRequest(new { message = validationResult.error });
            await _patientRepository.Insert(patient);

            _logger.LogInformation($"New patient \"{patient.Email}\" was created.");

            return Ok(patient);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut]
        [Authorize(Roles = "Patient")]
        public async Task<ActionResult<Patient>> Put(Patient patient)
        {
            if (patient == null || 
                patient.Id != Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value)) return BadRequest();
            if (await _patientRepository.GetById(patient.Id) == null) return NotFound();
            await _patientRepository.Update(patient);

            _logger.LogInformation($"{User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value} " +
                $" \"{this.User.Identity.Name}[{User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value}]\" changed his data.");

            return Ok(patient);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Patient>> Delete(int id)
        {
            Patient patient = await _patientRepository.GetById(id);
            if (patient == null) return NotFound();
            await _patientRepository.Delete(patient);

            _logger.LogInformation($"Patient \"{patient.Email}[{patient.Id}]\" was deleted.");

            return Ok();
        }


    }
}
