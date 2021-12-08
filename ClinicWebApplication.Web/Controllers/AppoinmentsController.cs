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
using ClinicWebApplication.BusinessLayer.Services.InputValidationService;
using ClinicWebApplication.BusinessLayer.Specification.AppoinmentSpecification;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using ClinicWebApplication.BusinessLayer.Services.AuthenticationService;

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
        [Authorize(Roles = "Doctor")]
        public async Task<IEnumerable<AppoinmentViewModel>> Get()
        {
            var appoinments = await _appoinmentRepository.GetAll();
            return _mapper.Map<IEnumerable<Appoinment>, IEnumerable<AppoinmentViewModel>>(appoinments);
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "Doctor")]
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
        [HttpPost]
        [Authorize(Roles = "Patient")]
        public async Task<ActionResult<Appoinment>> Post(Appoinment appoinment)
        {
            if (appoinment == null) return BadRequest();
            var validationResult = InputValidation.ValidateAppoinment(appoinment);
            if (validationResult.result == false) return BadRequest(new { message = validationResult.error });
            await _appoinmentRepository.Insert(appoinment);

            _logger.LogInformation($"Patient \"{this.User.Identity.Name}[{User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value}]\" created new appointment to" +
                $"{appoinment.Doctor.Email}.");

            return Ok(appoinment);
        }
        [HttpPut]
        [Authorize(Roles = "Patient, Doctor")]
        public async Task<ActionResult<Appoinment>> Put(Appoinment appoinment)
        {
            if (appoinment == null ||
                appoinment.DoctorId != Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value) ||
                appoinment.PatientId != Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value)) return BadRequest();
            if (await _appoinmentRepository.GetById(appoinment.Id) == null) return NotFound();
            await _appoinmentRepository.Update(appoinment);

            _logger.LogInformation($"{User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value} \"{this.User.Identity.Name}[{User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value}]\" " +
                $"changed appointment[{appoinment.Id}].");

            return Ok(appoinment);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Patient")]
        public async Task<ActionResult<Appoinment>> Delete(int id)
        {
            Appoinment appoinment = await _appoinmentRepository.GetById(id);
            if (appoinment == null) return NotFound();
            await _appoinmentRepository.Delete(appoinment);

            _logger.LogInformation($"Patient \"{this.User.Identity.Name}[{User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value}]\" " +
                $"deleted appointment[{appoinment.Id}].");

            return Ok();
        }
    }
}
