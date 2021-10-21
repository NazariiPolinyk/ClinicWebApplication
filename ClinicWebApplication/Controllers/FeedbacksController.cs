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
        public async Task<ActionResult<IEnumerable<Feedback>>> Get()
        {
            return await _feedbackRepository.GetAll().ToListAsync();
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
            _feedbackRepository.Insert(feedback);
            await Task.Run(() => _feedbackRepository.Save());
            return Ok(feedback);
        }
        [HttpPut]
        public async Task<ActionResult<Feedback>> Put(Feedback feedback)
        {
            if (feedback == null) return BadRequest();
            if (!_feedbackRepository.GetAll().Any(x => x.Id == feedback.Id)) return NotFound();
            _feedbackRepository.Update(feedback);
            await Task.Run(() => _feedbackRepository.Save());
            return Ok(feedback);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Feedback>> Delete(int id)
        {
            Feedback feedback = await _feedbackRepository.GetById(id);
            if (feedback == null) return NotFound();
            _feedbackRepository.Delete(id);
            await Task.Run(() => _feedbackRepository.Save());
            return Ok(feedback);
        }
    }
}
