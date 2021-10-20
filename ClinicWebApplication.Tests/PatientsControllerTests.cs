using System;
using Xunit;
using ClinicWebApplication.Controllers;
using ClinicWebApplication.Repository;
using ClinicWebApplication.Models;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ClinicWebApplication.Tests
{
    public class PatientsControllerTests
    {
        [Fact]
        private IQueryable<Patient> GetTestPatients()
        {
            IQueryable<Patient> patients = new List<Patient>
            {
                new Patient{Id = 1, Name = "Nazariy", BirthDate = new DateTime(1972, 7, 26), Phone = "0967755333" },
                new Patient{Id = 2, Name = "Ivan", BirthDate = new DateTime(1986, 2, 12), Phone = "0967755333" },
                new Patient{Id = 3, Name = "Roman", BirthDate = new DateTime(1996, 6, 9), Phone = "0967755333" },
                new Patient{Id = 4, Name = "Vasyl", BirthDate = new DateTime(1972, 8, 18), Phone = "0967755333" },
                new Patient{Id = 5, Name = "Ivan", BirthDate = new DateTime(1965, 10, 20), Phone = "0967755333" }
            }.AsQueryable();
            return patients;
        }

        [Fact]
        public async void GetAllReturnListOfPatients()
        {
            var mock = new Mock<IRepository<Patient>>();
            mock.Setup(r => r.GetAll()).Returns(GetTestPatients());
            var controller = new PatientsController(mock.Object);

            ActionResult<IEnumerable<Patient>> result = await controller.Get();

            Assert.Equal(5, result.Value.Count());
        }
        //'The source 'IQueryable' doesn't implement 'IAsyncEnumerable<ClinicWebApplication.Models.Patient>'. Only sources that implement 
        //   'IAsyncEnumerable' can be used for Entity Framework asynchronous operations.'
        [Fact]
        public async void GetPatientReturnObjectResult()
        {
            int testPatientId = 1;
            var mock = new Mock<ClinicRepository<Patient>>();
            mock.Setup(r => r.GetById(testPatientId))
                .ReturnsAsync(GetTestPatients().FirstOrDefault(p => p.Id == testPatientId));
            var controller = new PatientsController(mock.Object);

            var result = await controller.Get(testPatientId);

            Assert.Equal("Nazariy", result.Value.Name);
            Assert.Equal(Convert.ToDateTime("1972-7-26"), result.Value.BirthDate);
            Assert.Equal("0967755333", result.Value.Phone);
        }
    }
}
