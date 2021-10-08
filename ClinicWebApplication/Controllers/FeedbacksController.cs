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
    public class FeedbacksController : ControllerBase
    {
        private readonly ClinicContext _context;

        public FeedbacksController(ClinicContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Feedback>>> Get()
        {
            return await _context.Feedbacks.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Feedback>> Get(int id)
        {
            Feedback feedback = await _context.Feedbacks.FirstOrDefaultAsync(x => x.Id == id);
            if (feedback == null) return NotFound();
            return new ObjectResult(feedback);
        }
        [HttpPost]
        public async Task<ActionResult<Feedback>> Post(Feedback feedback)
        {
            if (feedback == null) return BadRequest();
            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();
            return Ok(feedback);
        }
        [HttpPut]
        public async Task<ActionResult<Feedback>> Put(Feedback feedback)
        {
            if (feedback == null) return NotFound();
            _context.Update(feedback);
            await _context.SaveChangesAsync();
            return Ok(feedback);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Feedback>> Delete(int id)
        {
            Feedback feedback = await _context.Feedbacks.FirstOrDefaultAsync(x => x.Id == id);
            if (feedback == null) return NotFound(); 
            _context.Feedbacks.Remove(feedback);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
