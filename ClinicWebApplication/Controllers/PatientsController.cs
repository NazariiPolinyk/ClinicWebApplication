using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClinicWebApplication.Models;
using Microsoft.EntityFrameworkCore;
using ClinicWebApplication.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace ClinicWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private IRepository<Patient> _patientRepository;

        [ActivatorUtilitiesConstructor]
        public PatientsController(ClinicContext context)
        {
            _patientRepository = new ClinicRepository<Patient>(context, context.Patients);
        }
        public PatientsController(IRepository<Patient> patientRepository)
        {
            _patientRepository = patientRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Patient>>> Get()
        {
            return await _patientRepository.GetAll().ToListAsync();
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
            _patientRepository.Insert(patient);
            await Task.Run(() => _patientRepository.Save());
            return Ok(patient);
        }
        [HttpPut]
        public async Task<ActionResult<Patient>> Put(Patient patient)
        {
            if (patient == null) return BadRequest();
            if (!_patientRepository.GetAll().Any(x => x.Id == patient.Id)) return NotFound();
            _patientRepository.Update(patient);
            await Task.Run(() => _patientRepository.Save());
            return Ok(patient);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Patient>> Delete(int id)
        {
            Patient patient = await _patientRepository.GetById(id);
            if (patient == null) return NotFound();
            _patientRepository.Delete(id);
            await Task.Run(() => _patientRepository.Save());
            return Ok(patient);
        }
    }
}
