using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClinicWebApplication.Models;
using Microsoft.EntityFrameworkCore;
using ClinicWebApplication.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace ClinicWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalCardRecordsController : ControllerBase
    {
        private IRepository<MedicalCardRecord> _medicalCardRecordRepository;

        [ActivatorUtilitiesConstructor]
        public MedicalCardRecordsController(ClinicContext context)
        {
            _medicalCardRecordRepository = new ClinicRepository<MedicalCardRecord>(context, context.MedicalCardRecords);
        }
        public MedicalCardRecordsController(IRepository<MedicalCardRecord> medicalCardRecordRepository)
        {
            _medicalCardRecordRepository = medicalCardRecordRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicalCardRecord>>> Get()
        {
            return await _medicalCardRecordRepository.GetAll().ToListAsync();
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
            _medicalCardRecordRepository.Insert(medicalCardRecord);
            await Task.Run(() => _medicalCardRecordRepository.Save());
            return Ok(medicalCardRecord);
        }
        [HttpPut]
        public async Task<ActionResult<MedicalCardRecord>> Put(MedicalCardRecord medicalCardRecord)
        {
            if (medicalCardRecord == null) return BadRequest();
            if (!_medicalCardRecordRepository.GetAll().Any(x => x.Id == medicalCardRecord.Id)) return NotFound();
            _medicalCardRecordRepository.Update(medicalCardRecord);
            await Task.Run(() => _medicalCardRecordRepository.Save());
            return Ok(medicalCardRecord);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<MedicalCardRecord>> Delete(int id)
        {
            MedicalCardRecord medicalCardRecord = await _medicalCardRecordRepository.GetById(id);
            if (medicalCardRecord == null) return NotFound();
            _medicalCardRecordRepository.Delete(id);
            await Task.Run(() => _medicalCardRecordRepository.Save());
            return Ok(medicalCardRecord);
        }
    }
}
