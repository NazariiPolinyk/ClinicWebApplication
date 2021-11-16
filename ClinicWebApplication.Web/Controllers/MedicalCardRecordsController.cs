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

namespace ClinicWebApplication.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalCardRecordsController : ControllerBase
    {
        private readonly IRepository<MedicalCardRecord> _medicalCardRecordRepository;
        private readonly IMapper _mapper;

        public MedicalCardRecordsController(IRepository<MedicalCardRecord> medicalCardRecordRepository, IMapper mapper)
        {
            _medicalCardRecordRepository = medicalCardRecordRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<MedicalCardRecordViewModel>> Get()
        {
            var medicalCardRecords = await _medicalCardRecordRepository.GetAll();
            return _mapper.Map<IEnumerable<MedicalCardRecord>, IEnumerable<MedicalCardRecordViewModel>>(medicalCardRecords);
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<MedicalCardRecordViewModel>> Get(int id)
        {
            var medicalCardRecordsWithSpecification = await _medicalCardRecordRepository.FindWithSpecification(new MedicalCardRecordWithDoctorSpecification(id));
            var medicalCardRecord = medicalCardRecordsWithSpecification.SingleOrDefault();
            if (medicalCardRecord == null) return NotFound();
            var medicalCardRecordViewModel = _mapper.Map<MedicalCardRecordViewModel>(medicalCardRecord);
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
            return Ok(medicalCardRecord);
        }
        [HttpPut]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<MedicalCardRecord>> Put(MedicalCardRecord medicalCardRecord)
        {
            if (medicalCardRecord == null) return BadRequest();
            if (await _medicalCardRecordRepository.GetById(medicalCardRecord.Id) == null) return NotFound();
            await _medicalCardRecordRepository.Update(medicalCardRecord);
            return Ok(medicalCardRecord);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<MedicalCardRecord>> Delete(int id)
        {
            MedicalCardRecord medicalCardRecord = await _medicalCardRecordRepository.GetById(id);
            if (medicalCardRecord == null) return NotFound();
            await _medicalCardRecordRepository.Delete(medicalCardRecord);
            return Ok();
        }
    }
}
