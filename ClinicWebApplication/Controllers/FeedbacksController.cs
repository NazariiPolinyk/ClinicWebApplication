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
    public class FeedbacksController : ControllerBase
    {
        private readonly IRepository<Feedback> _feedbackRepository;

        public FeedbacksController(IRepository<Feedback> feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;
        }

        [HttpGet]
        public IEnumerable<Feedback> Get()
        {
            return _feedbackRepository.GetAll();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Feedback>> Get(int id)
        {
            Feedback feedback = await _feedbackRepository.GetById(id);
            if (feedback == null) return NotFound();
            return new ObjectResult(feedback);
        }
        [HttpPost]
        public async Task<ActionResult<Feedback>> Post(Feedback feedback)
        {
            if (feedback == null) return BadRequest();
            await _feedbackRepository.Insert(feedback);
            return Ok(feedback);
        }
        [HttpPut]
        public async Task<ActionResult<Feedback>> Put(Feedback feedback)
        {
            if (feedback == null) return BadRequest();
            if (await _feedbackRepository.GetById(feedback.Id) == null) return NotFound();
            await _feedbackRepository.Update(feedback);
            return Ok(feedback);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Feedback>> Delete(int id)
        {
            Feedback feedback = await _feedbackRepository.GetById(id);
            if (feedback == null) return NotFound();
            await _feedbackRepository.Delete(id);
            return Ok();
        }
    }
}
