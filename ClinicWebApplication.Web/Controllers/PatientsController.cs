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
    public class PatientsController : ControllerBase
    {
        private readonly IRepository<Patient> _patientRepository;
        private readonly IMapper _mapper;

        public PatientsController(IRepository<Patient> patientRepository, IMapper mapper)
        {
            _patientRepository = patientRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<PatientViewModel>> Get()
        {
            var patients = await _patientRepository.GetAll();
            return _mapper.Map<IEnumerable<Patient>, IEnumerable<PatientViewModel>>(patients);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<PatientViewModel>> Get(int id)
        {
            Patient patient = await _patientRepository.GetById(id);
            if (patient == null) return NotFound();
            var patientViewModel = _mapper.Map<PatientViewModel>(patient);
            return new ObjectResult(patientViewModel);
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
