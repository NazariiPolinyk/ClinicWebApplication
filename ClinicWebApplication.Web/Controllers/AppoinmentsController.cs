using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClinicWebApplication.DataLayer.Models;
using ClinicWebApplication.Interfaces;
using ClinicWebApplication.Web.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using ClinicWebApplication.BusinessLayer.Services.InputValidationService;
using ClinicWebApplication.BusinessLayer.Specification.AppoinmentSpecification;
using ClinicWebApplication.BusinessLayer.Services.AuthenticationService;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using ClinicWebApplication.Web.InputModels;

namespace ClinicWebApplication.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppoinmentsController : ControllerBase
    {
        private readonly IRepository<Appoinment> _appoinmentRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AppoinmentsController> _logger;

        public AppoinmentsController(IRepository<Appoinment> appoinmentRepository, IMapper mapper, ILogger<AppoinmentsController> logger)
        {
            _appoinmentRepository = appoinmentRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "Doctor, Admin")]
        public async Task<IEnumerable<AppoinmentViewModel>> Get()
        {
            var appoinments = await _appoinmentRepository.GetAll();
            return _mapper.Map<IEnumerable<Appoinment>, IEnumerable<AppoinmentViewModel>>(appoinments);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("{id}")]
        [Authorize(Roles = "Doctor, Admin")]
        public async Task<ActionResult<AppoinmentViewModel>> Get(int id)
        {
            if (User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value == Role.Doctor &&
                Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value) != id) return BadRequest();
            var appoinmentsWithSpecification = await _appoinmentRepository.FindWithSpecification(new AppoinmentWithDoctorAndPatientSpecification(id));
            var appoinment = appoinmentsWithSpecification.SingleOrDefault();
            if (appoinment == null) return NotFound();
            var appoinmentViewModel = _mapper.Map<AppoinmentViewModel>(appoinment);
            return new ObjectResult(appoinmentViewModel);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost]
        [Authorize(Roles = "Patient")]
        public async Task<ActionResult<Appoinment>> Post([FromForm] AppoinmentInputModel appoinmentInputModel)
        {
            if (appoinmentInputModel == null) return BadRequest();
            Appoinment appoinment = new Appoinment
            {
                PatientId = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value),
                DoctorId = appoinmentInputModel.DoctorId,
                Description = appoinmentInputModel.Description,
                IsEnable = true
            };
            var validationResult = InputValidation.ValidateAppoinment(appoinment);
            if (validationResult.result == false) return BadRequest(new { message = validationResult.error });
            await _appoinmentRepository.Insert(appoinment);

            _logger.LogInformation($"Patient \"{this.User.Identity.Name}[{User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value}]\" created new appointment.");

            return Ok(appoinment);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut]
        [Authorize(Roles = "Patient, Doctor")]
        public async Task<ActionResult<Appoinment>> Put(Appoinment appoinment)
        {
            if (appoinment == null 
                || appoinment.PatientId != Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value)) return BadRequest();
            if (await _appoinmentRepository.GetById(appoinment.Id) == null) return NotFound();
            await _appoinmentRepository.Update(appoinment);

            _logger.LogInformation($"{User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value} \"{this.User.Identity.Name}[{User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value}]\" " +
                $"changed appointment[{appoinment.Id}].");

            return Ok(appoinment);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete("{id}")]
        [Authorize(Roles = "Patient")]
        public async Task<ActionResult<Appoinment>> Delete(int id)
        {
            Appoinment appoinment = await _appoinmentRepository.GetById(id);
            if (appoinment == null) return NotFound();
            if (User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value == Role.Patient &&
                Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value) != appoinment.PatientId) return BadRequest();
            await _appoinmentRepository.Delete(appoinment);

            _logger.LogInformation($"Patient \"{this.User.Identity.Name}[{User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value}]\" " +
                $"deleted appointment[{appoinment.Id}].");

            return Ok();
        }
    }
}
