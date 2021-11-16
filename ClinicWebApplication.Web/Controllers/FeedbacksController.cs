﻿using Microsoft.AspNetCore.Http;
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
using Microsoft.AspNetCore.Authorization;
using ClinicWebApplication.BusinessLayer.Services.InputValidationService;
using ClinicWebApplication.BusinessLayer.Specification.FeedbackSpecification;

namespace ClinicWebApplication.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbacksController : ControllerBase
    {
        private readonly IRepository<Feedback> _feedbackRepository;
        private readonly IMapper _mapper;

        public FeedbacksController(IRepository<Feedback> feedbackRepository, IMapper mapper)
        {
            _feedbackRepository = feedbackRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<FeedbackViewModel>> Get()
        {
            var feedbacks = await _feedbackRepository.GetAll();
            return _mapper.Map<IEnumerable<Feedback>, IEnumerable<FeedbackViewModel>>(feedbacks);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<FeedbackViewModel>> Get(int id)
        {
            var feedbacksWithSpecification = await _feedbackRepository.FindWithSpecification(new FeedbackWithPatientSpecification(id));
            var feedback = feedbacksWithSpecification.SingleOrDefault();
            if (feedback == null) return NotFound();
            var feedbackViewModel = _mapper.Map<FeedbackViewModel>(feedback);
            return new ObjectResult(feedbackViewModel);
        }
        [HttpPost]
        [Authorize(Roles = "Patient")]
        public async Task<ActionResult<Feedback>> Post(Feedback feedback)
        {
            if (feedback == null) return BadRequest();
            var validationResult = InputValidation.ValidateFeedback(feedback);
            if (validationResult.result == false) return BadRequest(new { message = validationResult.error });
            await _feedbackRepository.Insert(feedback);
            return Ok(feedback);
        }
        [HttpPut]
        [Authorize(Roles = "Patient")]
        public async Task<ActionResult<Feedback>> Put(Feedback feedback)
        {
            if (feedback == null) return BadRequest();
            if (await _feedbackRepository.GetById(feedback.Id) == null) return NotFound();
            await _feedbackRepository.Update(feedback);
            return Ok(feedback);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Patient")]
        public async Task<ActionResult<Feedback>> Delete(int id)
        {
            Feedback feedback = await _feedbackRepository.GetById(id);
            if (feedback == null) return NotFound();
            await _feedbackRepository.Delete(feedback);
            return Ok();
        }
    }
}
