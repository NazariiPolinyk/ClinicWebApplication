using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClinicWebApplication.DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using ClinicWebApplication.Interfaces;


namespace ClinicWebApplication.Infrastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalCardRecordsController : ControllerBase
    {
        private readonly IRepository<MedicalCardRecord> _medicalCardRecordRepository;

        public MedicalCardRecordsController(IRepository<MedicalCardRecord> medicalCardRecordRepository)
        {
            _medicalCardRecordRepository = medicalCardRecordRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<MedicalCardRecord>> Get()
        {
            return await _medicalCardRecordRepository.GetAll();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<MedicalCardRecord>> Get(int id)
        {
            MedicalCardRecord medicalCardRecord = await _medicalCardRecordRepository.GetById(id);
            if (medicalCardRecord == null) return NotFound();
            return new ObjectResult(medicalCardRecord);
        }
        [HttpPost]
        public async Task<ActionResult<MedicalCardRecord>> Post(MedicalCardRecord medicalCardRecord)
        {
            if (medicalCardRecord == null) return BadRequest();
            await _medicalCardRecordRepository.Insert(medicalCardRecord);
            return Ok(medicalCardRecord);
        }
        [HttpPut]
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
