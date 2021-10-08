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
        private readonly ClinicContext _context;

        public AppoinmentsController(ClinicContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Appoinment>>> Get()
        {
            return await _context.Appoinments.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Appoinment>> Get(int id)
        {
            Appoinment appoinment = await _context.Appoinments.FirstOrDefaultAsync(x => x.Id == id);
            if (appoinment == null) return NotFound();
            return new ObjectResult(appoinment);
        }
        [HttpPost]
        public async Task<ActionResult<Appoinment>> Post(Appoinment appoinment)
        {
            if (appoinment == null) return BadRequest();
            _context.Appoinments.Add(appoinment);
            await _context.SaveChangesAsync();
            return Ok(appoinment);
        }
        [HttpPut]
        public async Task<ActionResult<Appoinment>> Put(Appoinment appoinment)
        {
            if (appoinment == null) return NotFound();
            _context.Update(appoinment);
            await _context.SaveChangesAsync();
            return Ok(appoinment);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Appoinment>> Delete(int id)
        {
            Appoinment appoinment = await _context.Appoinments.FirstOrDefaultAsync(x => x.Id == id);
            if (appoinment == null) NotFound();
            _context.Appoinments.Remove(appoinment);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
