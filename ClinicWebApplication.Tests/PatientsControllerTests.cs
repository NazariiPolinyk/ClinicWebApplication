using System;
using Xunit;
using ClinicWebApplication.Web.Controllers;
using ClinicWebApplication.Interfaces;
using ClinicWebApplication.DataLayer.Models;
using Moq;
using MockQueryable.Moq;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AutoMapper;

namespace ClinicWebApplication.Tests
{    
    public class PatientsControllerTests
    {
        private readonly IMapper mapper = UnitTestUtility.CreateTestMapper();

        private List<Patient> GetTestPatients()
        {
            var patients = new List<Patient>
            {
                new Patient{Id = 1, Name = "Nazariy", BirthDate = new DateTime(1972, 7, 26), Phone = "0967755333" },
                new Patient{Id = 2, Name = "Ivan", BirthDate = new DateTime(1986, 2, 12), Phone = "0967755333" },
                new Patient{Id = 3, Name = "Roman", BirthDate = new DateTime(1996, 6, 9), Phone = "0967755333" },
                new Patient{Id = 4, Name = "Vasyl", BirthDate = new DateTime(1972, 8, 18), Phone = "0967755333" },
                new Patient{Id = 5, Name = "Ivan", BirthDate = new DateTime(1965, 10, 20), Phone = "0967755333" }
            };
            return patients;
        }

        [Fact]
        public async void GetAllReturnListOfPatients()
        {
            var repo = new Mock<IRepository<Patient>>();
            var mock = GetTestPatients().AsQueryable().BuildMock();
            repo.Setup(x => x.GetAll()).Returns(Task.FromResult(mock.Object.AsEnumerable()));
            var controller = new PatientsController(repo.Object, mapper);

            var result = await controller.Get();

            Assert.Equal(5, result.Count());
        }

        [Fact]
        public async void GetPatientReturnObjectResult()
        {
            var testPatient = GetTestPatients()[0];
            int testPatientId = testPatient.Id;
            var repo = new Mock<IRepository<Patient>>();
            var mock = GetTestPatients().AsQueryable().BuildMock();
            repo.Setup(x => x.GetById(testPatientId))
                .ReturnsAsync(testPatient);
            var controller = new PatientsController(repo.Object, mapper);

            var actionResult = await controller.Get(testPatientId);
            var result = UnitTestUtility.GetTestObjectResultContent(actionResult);

            Assert.Equal(testPatient.Name, result.Name);
            Assert.Equal(testPatient.BirthDate, result.BirthDate);
            Assert.Equal(testPatient.Phone, result.Phone);
        }

        [Fact]
        public async void GetPatientReturnNotFoundResult()
        {
            int testPatientId = 0;
            var repo = new Mock<IRepository<Patient>>();
            var mock = GetTestPatients().AsQueryable().BuildMock();
            repo.Setup(x => x.GetById(testPatientId))
                .ReturnsAsync(GetTestPatients().FirstOrDefault(p => p.Id == testPatientId));
            var controller = new PatientsController(repo.Object, mapper);

            var actionResult = await controller.Get(testPatientId);

            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public async void AddPatientReturnsOkResult()
        {
            var repo = new Mock<IRepository<Patient>>();
            var controller = new PatientsController(repo.Object, mapper);

            var actionResult = await controller.Post(new Patient { Id = 6, Name = "Ivan", BirthDate = new DateTime(1965, 10, 20), Phone = "0967755333" });

            Assert.IsType<OkObjectResult>(actionResult.Result);
            
        }

        [Fact]
        public async void AddPatientReturnsBadRequestResult()
        {
            var repo = new Mock<IRepository<Patient>>();
            var controller = new PatientsController(repo.Object, mapper);

            var actionResult = await controller.Post(null);

            Assert.IsType<BadRequestResult>(actionResult.Result);
        }

        [Fact]
        public async void UpdatePatientReturnsOkResult()
        {
            int testPatientId = 1;
            var repo = new Mock<IRepository<Patient>>();
            var mock = GetTestPatients().AsQueryable().BuildMock();
            repo.Setup(x => x.GetById(testPatientId))
                .ReturnsAsync(GetTestPatients().FirstOrDefault(p => p.Id == testPatientId));
            var controller = new PatientsController(repo.Object, mapper);

            var actionResult = await controller.Put(new Patient { Id = 1, Name = "Nazar", BirthDate = new DateTime(1972, 7, 26), Phone = "0967755333" });

            Assert.IsType<OkObjectResult>(actionResult.Result);
        }

        [Fact]
        public async void UpdatePatientReturnsBadRequestResult()
        {
            var repo = new Mock<IRepository<Patient>>();
            var mock = GetTestPatients().AsQueryable().BuildMock();
            var controller = new PatientsController(repo.Object, mapper);

            var actionResult = await controller.Put(null);

            Assert.IsType<BadRequestResult>(actionResult.Result);
        }

        [Fact]
        public async void UpdatePatientReturnsNotFoundResult()
        {
            int testPatientId = 6;
            var repo = new Mock<IRepository<Patient>>();
            var mock = GetTestPatients().AsQueryable().BuildMock();
            repo.Setup(x => x.GetById(testPatientId))
                .ReturnsAsync(GetTestPatients().FirstOrDefault(p => p.Id == testPatientId));
            var controller = new PatientsController(repo.Object, mapper);

            var actionResult = await controller.Put(new Patient { Id = 6, Name = "Nazar", BirthDate = new DateTime(1972, 7, 26), Phone = "0967755333" });

            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public async void DeletePatientReturnsOkResult()
        {
            int testPatientId = 2;
            var repo = new Mock<IRepository<Patient>>();
            var mock = GetTestPatients().AsQueryable().BuildMock();
            repo.Setup(x => x.GetById(testPatientId))
                .ReturnsAsync(GetTestPatients().FirstOrDefault(p => p.Id == testPatientId));
            var controller = new PatientsController(repo.Object, mapper);

            var actionResult = await controller.Delete(testPatientId);

            Assert.IsType<OkResult>(actionResult.Result);
        }

        [Fact]
        public async void DeletePatientReturnsNotFoundResult()
        {
            var repo = new Mock<IRepository<Patient>>();
            var mock = GetTestPatients().AsQueryable().BuildMock();
            repo.Setup(x => x.GetAll()).Returns(Task.FromResult(mock.Object.AsEnumerable()));
            var controller = new PatientsController(repo.Object, mapper);

            var actionResult = await controller.Delete(6);

            Assert.IsType<NotFoundResult>(actionResult.Result);
        }
    }
}
