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
    public class AppoinmentsController : ControllerBase
    {
        ClinicContext context;

        public AppoinmentsController(ClinicContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Appoinment>>> Get()
        {
            return await context.Appoinments.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Appoinment>> Get(int id)
        {
            Appoinment appoinment = await context.Appoinments.FirstOrDefaultAsync(x => x.Id == id);
            if (appoinment == null) return NotFound();
            return new ObjectResult(appoinment);
        }
        [HttpPost]
        public async Task<ActionResult<Appoinment>> Post(Appoinment appoinment)
        {
            if (appoinment == null) return BadRequest();
            context.Appoinments.Add(appoinment);
            await context.SaveChangesAsync();
            return Ok(appoinment);
        }
        [HttpPut]
        public async Task<ActionResult<Appoinment>> Put(Appoinment appoinment)
        {
            if (appoinment == null) return NotFound();
            context.Update(appoinment);
            await context.SaveChangesAsync();
            return Ok(appoinment);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Appoinment>> Delete(int id)
        {
            Appoinment appoinment = context.Appoinments.FirstOrDefault(x => x.Id == id);
            if (appoinment == null) NotFound();
            context.Appoinments.Remove(appoinment);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
