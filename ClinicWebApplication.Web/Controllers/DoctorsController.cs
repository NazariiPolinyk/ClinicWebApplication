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

namespace ClinicWebApplication.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IRepository<Doctor> _doctorRepository;
        private readonly IMapper _mapper;

        public DoctorsController(IRepository<Doctor> doctorRepository, IMapper mapper)
        {
            _doctorRepository = doctorRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<DoctorViewModel>> Get()
        {
            var doctors = await _doctorRepository.GetAll();
            return _mapper.Map<IEnumerable<Doctor>, IEnumerable<DoctorViewModel>>(doctors);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<DoctorViewModel>> Get(int id)
        {
            Doctor doctor = await _doctorRepository.GetById(id);
            if (doctor == null) return NotFound();
            var doctorViewModel = _mapper.Map<DoctorViewModel>(doctor);
            return new ObjectResult(doctorViewModel);
        }
        [HttpPost]
        public async Task<ActionResult<Doctor>> Post(Doctor doctor)
        {
            if (doctor == null) return BadRequest();
            await _doctorRepository.Insert(doctor);
            return Ok(doctor);
        }
        [HttpPut]
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
