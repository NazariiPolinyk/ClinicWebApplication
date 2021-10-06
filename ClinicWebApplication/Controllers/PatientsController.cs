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
        ClinicContext context;

        public PatientsController(ClinicContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Patient>>> Get()
        {
            return await context.Patients.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> Get(int id)
        {
            Patient patient = await context.Patients.FirstOrDefaultAsync(x => x.Id == id);
            if (patient == null) return NotFound();
            return new ObjectResult(patient);
        }
        [HttpPost]
        public async Task<ActionResult<Patient>> Post(Patient patient)
        {
            if (patient == null) return BadRequest();
            context.Patients.Add(patient);
            await context.SaveChangesAsync();
            return Ok(patient);
        }
        [HttpPut]
        public async Task<ActionResult<Patient>> Put(Patient patient)
        {
            if (patient == null) return NotFound();
            context.Update(patient);
            await context.SaveChangesAsync();
            return Ok(patient);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Patient>> Delete(int id)
        {
            Patient patient = context.Patients.FirstOrDefault(x => x.Id == id);
            if (patient == null) return NotFound();
            context.Patients.Remove(patient);
            await context.SaveChangesAsync();
            return Ok(patient);
        }
    }
}
