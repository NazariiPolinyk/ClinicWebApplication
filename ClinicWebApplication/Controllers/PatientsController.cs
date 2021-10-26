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
    public class PatientsController : ControllerBase
    {
        private readonly IRepository<Patient> _patientRepository;
        
        public PatientsController(IRepository<Patient> patientRepository)
        {
            _patientRepository = patientRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<Patient>> Get()
        {
            return await _patientRepository.GetAll();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> Get(int id)
        {
            Patient patient = await _patientRepository.GetById(id);
            if (patient == null) return NotFound();
            return new ObjectResult(patient);
        }
        [HttpPost]
        public async Task<ActionResult<Patient>> Post(Patient patient)
        {
            if (patient == null) return BadRequest();
            await _patientRepository.Insert(patient);
            return Ok(patient);
        }
        [HttpPut]
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
