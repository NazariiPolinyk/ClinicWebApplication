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
    public class MedicalCardRecordsController : ControllerBase
    {
        private readonly ClinicContext _context;

        public MedicalCardRecordsController(ClinicContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicalCardRecord>>> Get()
        {
            return await _context.MedicalCardRecords.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<MedicalCardRecord>> Get(int id)
        {
            MedicalCardRecord medicalCardRecord = await _context.MedicalCardRecords.FirstOrDefaultAsync(x => x.Id == id);
            if (medicalCardRecord == null) return NotFound();
            return new ObjectResult(medicalCardRecord);
        }
        [HttpPost]
        public async Task<ActionResult<MedicalCardRecord>> Post(MedicalCardRecord medicalCardRecord)
        {
            if (medicalCardRecord == null) return BadRequest();
            _context.MedicalCardRecords.Add(medicalCardRecord);
            await _context.SaveChangesAsync();
            return Ok(medicalCardRecord);
        }
        [HttpPut]
        public async Task<ActionResult<MedicalCardRecord>> Put(MedicalCardRecord medicalCardRecord)
        {
            if (medicalCardRecord == null) return NotFound();
            _context.Update(medicalCardRecord);
            await _context.SaveChangesAsync();
            return Ok(medicalCardRecord);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<MedicalCardRecord>> Delete(int id)
        {
            MedicalCardRecord medicalCardRecord = await _context.MedicalCardRecords.FirstOrDefaultAsync(x => x.Id == id);
            if (medicalCardRecord == null) return NotFound();
            _context.MedicalCardRecords.Remove(medicalCardRecord);
            await _context.SaveChangesAsync();
            return Ok(medicalCardRecord);
        }
    }
}
