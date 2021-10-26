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
    public class AppoinmentsController : ControllerBase
    {
        private readonly IRepository<Appoinment> _appoinmentRepository;

        public AppoinmentsController(IRepository<Appoinment> appoinmentRepository)
        {
            _appoinmentRepository = appoinmentRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<Appoinment>> Get()
        {
            return await _appoinmentRepository.GetAll();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Appoinment>> Get(int id)
        {
            Appoinment appoinment = await _appoinmentRepository.GetById(id);
            if (appoinment == null) return NotFound();
            return new ObjectResult(appoinment);
        }
        [HttpPost]
        public async Task<ActionResult<Appoinment>> Post(Appoinment appoinment)
        {
            if (appoinment == null) return BadRequest();
            await _appoinmentRepository.Insert(appoinment);
            return Ok(appoinment);
        }
        [HttpPut]
        public async Task<ActionResult<Appoinment>> Put(Appoinment appoinment)
        {
            if (appoinment == null) return BadRequest();
            if (await _appoinmentRepository.GetById(appoinment.Id) == null) return NotFound();
            await _appoinmentRepository.Update(appoinment);
            return Ok(appoinment);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Appoinment>> Delete(int id)
        {
            Appoinment appoinment = await _appoinmentRepository.GetById(id);
            if (appoinment == null) return NotFound();
            await _appoinmentRepository.Delete(appoinment);
            return Ok();
        }
    }
}
