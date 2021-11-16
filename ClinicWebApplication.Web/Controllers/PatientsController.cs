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
using ClinicWebApplication.BusinessLayer.Services.AuthenticationService;
using ClinicWebApplication.BusinessLayer.Services.InputValidationService;
using ClinicWebApplication.BusinessLayer.Specification.PatientSpecification;

namespace ClinicWebApplication.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IRepository<Patient> _patientRepository;
        private readonly IMapper _mapper;
        private readonly IAuthService<Patient> _authService;

        public PatientsController(IRepository<Patient> patientRepository, IMapper mapper, IAuthService<Patient> authService)
        {
            _patientRepository = patientRepository;
            _mapper = mapper;
            _authService = authService;
        }
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]AuthenticateModel model)
        {
            var patient = _authService.Authenticate(model.Login, model.Password);

            if (patient == null) return BadRequest(new { message = "Email or password is incorrect" });

            return Ok(patient);
        }
        [HttpGet]
        [Authorize(Roles = "Doctor")]
        public async Task<IEnumerable<PatientViewModel>> Get()
        {
            var patients = await _patientRepository.GetAll();
            return _mapper.Map<IEnumerable<Patient>, IEnumerable<PatientViewModel>>(patients);
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "Doctor, Patient")]
        public async Task<ActionResult<PatientViewModel>> Get(int id)
        {
            var patientsWithSpecification = await _patientRepository.FindWithSpecification(new PatientWithMedicalCardRecordsSpecification(id));
            var patient = patientsWithSpecification.SingleOrDefault();
            if (patient == null) return NotFound();
            var patientViewModel = _mapper.Map<PatientViewModel>(patient);
            return new ObjectResult(patientViewModel);
        }
        [HttpPost]
        public async Task<ActionResult<Patient>> Post(Patient patient)
        {
            if (patient == null) return BadRequest();
            var validationResult = InputValidation.ValidatePatient(patient);
            if (validationResult.result == false) return BadRequest(new { message = validationResult.error });
            await _patientRepository.Insert(patient);
            return Ok(patient);
        }
        [HttpPut]
        [Authorize(Roles = "Patient")]
        public async Task<ActionResult<Patient>> Put(Patient patient)
        {
            if (patient == null) return BadRequest();
            if (await _patientRepository.GetById(patient.Id) == null) return NotFound();
            await _patientRepository.Update(patient);
            return Ok(patient);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Patient>> Delete(int id)
        {
            Patient patient = await _patientRepository.GetById(id);
            if (patient == null) return NotFound();
            await _patientRepository.Delete(patient);
            return Ok();
        }


    }
}
