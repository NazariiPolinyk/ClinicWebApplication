using Microsoft.AspNetCore.Http;
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
using ClinicWebApplication.BusinessLayer.Specification.MedicalCardRecordSpecification;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using ClinicWebApplication.BusinessLayer.Services.AuthenticationService;

namespace ClinicWebApplication.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalCardRecordsController : ControllerBase
    {
        private readonly IRepository<MedicalCardRecord> _medicalCardRecordRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<MedicalCardRecordsController> _logger;

        public MedicalCardRecordsController(IRepository<MedicalCardRecord> medicalCardRecordRepository, IMapper mapper, ILogger<MedicalCardRecordsController> logger)
        {
            _medicalCardRecordRepository = medicalCardRecordRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "Doctor")]
        public async Task<IEnumerable<MedicalCardRecordViewModel>> Get()
        {
            var medicalCardRecords = await _medicalCardRecordRepository.GetAll();
            return _mapper.Map<IEnumerable<MedicalCardRecord>, IEnumerable<MedicalCardRecordViewModel>>(medicalCardRecords);
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "Doctor, Patient")]
        public async Task<ActionResult<IEnumerable<MedicalCardRecordViewModel>>> Get(int id)
        {
            if (User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value == Role.Patient &&
                Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value) != id) return BadRequest();
            var medicalCardRecords = await _medicalCardRecordRepository.FindWithSpecification(new MedicalCardRecordWithDoctorSpecification(id));
            if (medicalCardRecords == null) return NotFound();
            var medicalCardRecordViewModel = _mapper.Map<IEnumerable<MedicalCardRecord>, IEnumerable<MedicalCardRecordViewModel>>(medicalCardRecords);
            return new ObjectResult(medicalCardRecordViewModel);
        }
        [HttpPost]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<MedicalCardRecord>> Post(MedicalCardRecord medicalCardRecord)
        {
            if (medicalCardRecord == null) return BadRequest();
            var validationResult = InputValidation.ValidateMedicalCardRecord(medicalCardRecord);
            if (validationResult.result == false) return BadRequest(new { message = validationResult.error });
            await _medicalCardRecordRepository.Insert(medicalCardRecord);

            _logger.LogInformation($"Doctor \"{this.User.Identity.Name}[{User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value}]\" created new record in medical card of" +
                $"{medicalCardRecord.Patient.Email}.");

            return Ok(medicalCardRecord);
        }
        [HttpPut]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<MedicalCardRecord>> Put(MedicalCardRecord medicalCardRecord)
        {
            if (medicalCardRecord == null ||
                medicalCardRecord.DoctorId != Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value)) return BadRequest();
            if (await _medicalCardRecordRepository.GetById(medicalCardRecord.Id) == null) return NotFound();
            await _medicalCardRecordRepository.Update(medicalCardRecord);

            _logger.LogInformation($"Doctor \"{this.User.Identity.Name}[{User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value}]\" changed record[{medicalCardRecord.Id}] in medical card of" +
                $"{medicalCardRecord.Patient.Email}.");

            return Ok(medicalCardRecord);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<MedicalCardRecord>> Delete(int id)
        {
            MedicalCardRecord medicalCardRecord = await _medicalCardRecordRepository.GetById(id);
            if (User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value == Role.Doctor &&
                Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value) != medicalCardRecord.DoctorId) return BadRequest();
            if (medicalCardRecord == null) return NotFound();
            await _medicalCardRecordRepository.Delete(medicalCardRecord);

            _logger.LogInformation($"Doctor \"{this.User.Identity.Name}[{User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value}]\" deleted record[{id}] in medical card of" +
                $"{medicalCardRecord.Patient.Email}.");


            return Ok();
        }
    }
}
