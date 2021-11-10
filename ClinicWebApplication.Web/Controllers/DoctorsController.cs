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

namespace ClinicWebApplication.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IRepository<Doctor> _doctorRepository;
        private readonly IMapper _mapper;
        private readonly IAuthService<Doctor> _authService;

        public DoctorsController(IRepository<Doctor> doctorRepository, IMapper mapper, IAuthService<Doctor> authService)
        {
            _doctorRepository = doctorRepository;
            _mapper = mapper;
            _authService = authService;
        }
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateModel model)
        {
            var doctor = _authService.Authenticate(model.Login, model.Password);

            if (doctor == null) return BadRequest(new { message = "Email or password is incorrect" });

            return Ok(doctor);
        }
        [HttpGet]
        [Authorize(Roles = "Patient")]
        public async Task<IEnumerable<DoctorViewModel>> Get()
        {
            var doctors = await _doctorRepository.GetAll();
            return _mapper.Map<IEnumerable<Doctor>, IEnumerable<DoctorViewModel>>(doctors);
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "Patient, Doctor")]
        public async Task<ActionResult<DoctorViewModel>> Get(int id)
        {
            Doctor doctor = await _doctorRepository.GetById(id);
            if (doctor == null) return NotFound();
            var doctorViewModel = _mapper.Map<DoctorViewModel>(doctor);
            return new ObjectResult(doctorViewModel);
        }
        [HttpPost]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<Doctor>> Post(Doctor doctor)
        {
            if (doctor == null) return BadRequest();
            await _doctorRepository.Insert(doctor);
            return Ok(doctor);
        }
        [HttpPut]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<Doctor>> Put(Doctor doctor)
        {
            if (doctor == null) return BadRequest();
            if (await _doctorRepository.GetById(doctor.Id) == null) return NotFound();
            await _doctorRepository.Update(doctor);
            return Ok(doctor);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Doctor>> Delete(int id)
        {
            Doctor doctor = await _doctorRepository.GetById(id);
            if (doctor == null) return NotFound();
            await _doctorRepository.Delete(doctor);
            return Ok();
        }
    
    }
}
