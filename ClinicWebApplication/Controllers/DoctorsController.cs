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
        public async Task<ActionResult<IEnumerable<Doctor>>> Get()
        {
            return await _doctorRepository.GetAll().ToListAsync();
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
            _doctorRepository.Insert(doctor);
            await Task.Run(() => _doctorRepository.Save());
            return Ok(doctor);
        }
        [HttpPut]
        public async Task<ActionResult<Doctor>> Put(Doctor doctor)
        {
            if (doctor == null) return BadRequest();
            if (!_doctorRepository.GetAll().Any(x => x.Id == doctor.Id)) return NotFound();
            _doctorRepository.Update(doctor);
            await Task.Run(() => _doctorRepository.Save());
            return Ok(doctor);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Doctor>> Delete(int id)
        {
            Doctor doctor = await _doctorRepository.GetById(id);
            if (doctor == null) return NotFound();
            _doctorRepository.Delete(id);
            await Task.Run(() => _doctorRepository.Save());
            return Ok(doctor);
        }
    
    }
}
