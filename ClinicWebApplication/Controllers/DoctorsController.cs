using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClinicWebApplication.Models;
using Microsoft.EntityFrameworkCore;
using ClinicWebApplication.Repository;


namespace ClinicWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IRepository<Doctor> _doctorRepository;

        public DoctorsController(IRepository<Doctor> doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        [HttpGet]
        public IEnumerable<Doctor> Get()
        {
            return _doctorRepository.GetAll();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Doctor>> Get(int id)
        {
            Doctor doctor = await _doctorRepository.GetById(id);
            if (doctor == null) return NotFound();
            return new ObjectResult(doctor);
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
            await _doctorRepository.Delete(id);
            return Ok();
        }
    
    }
}
