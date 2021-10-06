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
        ClinicContext context;

        public FeedbacksController(ClinicContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Feedback>>> Get()
        {
            return await context.Feedbacks.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Feedback>> Get(int id)
        {
            Feedback feedback = await context.Feedbacks.FirstOrDefaultAsync(x => x.Id == id);
            if (feedback == null) return NotFound();
            return new ObjectResult(feedback);
        }
        [HttpPost]
        public async Task<ActionResult<Feedback>> Post(Feedback feedback)
        {
            if (feedback == null) return BadRequest();
            context.Feedbacks.Add(feedback);
            await context.SaveChangesAsync();
            return Ok(feedback);
        }
        [HttpPut]
        public async Task<ActionResult<Feedback>> Put(Feedback feedback)
        {
            if (feedback == null) return NotFound();
            context.Update(feedback);
            await context.SaveChangesAsync();
            return Ok(feedback);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Feedback>> Delete(int id)
        {
            Feedback feedback = context.Feedbacks.FirstOrDefault(x => x.Id == id);
            if (feedback == null) return NotFound(); 
            context.Feedbacks.Remove(feedback);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
