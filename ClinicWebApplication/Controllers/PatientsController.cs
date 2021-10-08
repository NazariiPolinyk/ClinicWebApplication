using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClinicWebApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly ClinicContext _context;

        public PatientsController(ClinicContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Patient>>> Get()
        {
            return await _context.Patients.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> Get(int id)
        {
            Patient patient = await _context.Patients.FirstOrDefaultAsync(x => x.Id == id);
            if (patient == null) return NotFound();
            return new ObjectResult(patient);
        }
        [HttpPost]
        public async Task<ActionResult<Patient>> Post(Patient patient)
        {
            if (patient == null) return BadRequest();
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
            return Ok(patient);
        }
        [HttpPut]
        public async Task<ActionResult<Patient>> Put(Patient patient)
        {
            if (patient == null) return NotFound();
            _context.Update(patient);
            await _context.SaveChangesAsync();
            return Ok(patient);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Patient>> Delete(int id)
        {
            Patient patient = await _context.Patients.FirstOrDefaultAsync(x => x.Id == id);
            if (patient == null) return NotFound();
            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return Ok(patient);
        }
    }
}
