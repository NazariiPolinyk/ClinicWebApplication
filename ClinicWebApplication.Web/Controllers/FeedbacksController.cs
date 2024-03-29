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
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using ClinicWebApplication.BusinessLayer.Services.AuthenticationService;
using ClinicWebApplication.Web.InputModels;

namespace ClinicWebApplication.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbacksController : ControllerBase
    {
        private readonly IRepository<Feedback> _feedbackRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<FeedbacksController> _logger;

        public FeedbacksController(IRepository<Feedback> feedbackRepository, IMapper mapper, ILogger<FeedbacksController> logger)
        {
            _feedbackRepository = feedbackRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<FeedbackViewModel>> Get()
        {
            var feedbacks = await _feedbackRepository.GetAll();
            return _mapper.Map<IEnumerable<Feedback>, IEnumerable<FeedbackViewModel>>(feedbacks);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("{id}")]
        [Authorize(Roles = "Patient, Doctor, Admin")]
        public async Task<ActionResult<IEnumerable<FeedbackViewModel>>> Get(int id)
        {
            if (User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value == Role.Doctor &&
                Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value) != id) return BadRequest();
            var feedbacks = await _feedbackRepository.FindWithSpecification(new FeedbackWithPatientSpecification(id));
            if (feedbacks == null) return NotFound();
            var feedbackViewModel = _mapper.Map<IEnumerable<Feedback>, IEnumerable<FeedbackViewModel>>(feedbacks);
            return new ObjectResult(feedbackViewModel);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost]
        [Authorize(Roles = "Patient")]
        public async Task<ActionResult<Feedback>> Post([FromForm] FeedbackInputModel feedbackInputModel)
        {
            if (feedbackInputModel == null) return BadRequest();
            Feedback feedback = new Feedback
            {
                DoctorId = feedbackInputModel.DoctorId,
                PatientId = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value),
                FeedbackText = feedbackInputModel.FeedbackText
            };
            var validationResult = InputValidation.ValidateFeedback(feedback);
            if (validationResult.result == false) return BadRequest(new { message = validationResult.error });
            await _feedbackRepository.Insert(feedback);

            _logger.LogInformation($"Patient \"{this.User.Identity.Name}[{User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value}]\" created new feedback.");

            return Ok(feedback);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut]
        [Authorize(Roles = "Patient")]
        public async Task<ActionResult<Feedback>> Put(Feedback feedback)
        {
            if (feedback == null ||
                feedback.PatientId != Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value)) return BadRequest();
            if (await _feedbackRepository.GetById(feedback.Id) == null) return NotFound();
            await _feedbackRepository.Update(feedback);

            _logger.LogInformation($"Patient \"{this.User.Identity.Name}[{User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value}]\" changed feedback.");
            return Ok(feedback);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete("{id}")]
        [Authorize(Roles = "Patient")]
        public async Task<ActionResult<Feedback>> Delete(int id)
        {
            Feedback feedback = await _feedbackRepository.GetById(id);
            if (User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value == Role.Patient &&
               Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value) != feedback.PatientId) return BadRequest();
            if (feedback == null) return NotFound();
            await _feedbackRepository.Delete(feedback);

            _logger.LogInformation($"Patient \"{this.User.Identity.Name}[{User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value}]\" deleted feedback[{id}].");

            return Ok();
        }
    }
}
