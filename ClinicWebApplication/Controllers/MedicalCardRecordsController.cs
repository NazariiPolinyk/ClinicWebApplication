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
    public class MedicalCardRecordsController : ControllerBase
    {
        private readonly IRepository<MedicalCardRecord> _medicalCardRecordRepository;

        public MedicalCardRecordsController(IRepository<MedicalCardRecord> medicalCardRecordRepository)
        {
            _medicalCardRecordRepository = medicalCardRecordRepository;
        }

        [HttpGet]
        public IEnumerable<MedicalCardRecord> Get()
        {
            return _medicalCardRecordRepository.GetAll();
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
            await _medicalCardRecordRepository.Delete(id);
            return Ok();
        }
    }
}
