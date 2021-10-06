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
    public class DoctorsController : ControllerBase
    {
        ClinicContext context;

        public DoctorsController(ClinicContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Doctor>>> Get()
        {
            return await context.Doctors.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Doctor>> Get(int id)
        {
            Doctor doctor = await context.Doctors.FirstOrDefaultAsync(x => x.Id == id);
            if (doctor == null) return NotFound();
            return new ObjectResult(doctor);
        }
        [HttpPost]
        public async Task<ActionResult<Doctor>> Post(Doctor doctor)
        {
            if (doctor == null) return BadRequest();
            context.Doctors.Add(doctor);
            await context.SaveChangesAsync();
            return Ok(doctor);
        }
        [HttpPut]
        public async Task<ActionResult<Doctor>> Put(Doctor doctor)
        {
            if (doctor == null) return NotFound();
            context.Update(doctor);
            await context.SaveChangesAsync();
            return Ok(doctor);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Doctor>> Delete(int id)
        {
            Doctor doctor = context.Doctors.FirstOrDefault(x => x.Id == id);
            if (doctor == null) return NotFound();
            context.Doctors.Remove(doctor);
            await context.SaveChangesAsync();
            return Ok(doctor);
        }
    
    }
}
