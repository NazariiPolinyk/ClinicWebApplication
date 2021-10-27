using Xunit;
using ClinicWebApplication.Infrastructure.Controllers;
using ClinicWebApplication.Interfaces;
using ClinicWebApplication.DataLayer.Models;
using Moq;
using MockQueryable.Moq;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ClinicWebApplication.Tests
{
    public class DoctorsControllerTests
    {
        private List<Doctor> GetTestDoctors()
        {
            var doctors = new List<Doctor>
            {
                new Doctor{Id = 1, Name = "Nazariy", Experience = 14, Category = "Cardiology", Description = "Good doctor" },
                new Doctor{Id = 2, Name = "Vladyslav", Experience = 2, Category = "Surgery", Description = "Good doctor" },
                new Doctor{Id = 3, Name = "Andriy", Experience = 27, Category = "Traumatology", Description = "Good doctor" },
                new Doctor{Id = 4, Name = "Natalia", Experience = 4, Category = "Pediatrics", Description = "Good doctor" },
                new Doctor{Id = 5, Name = "Mykola", Experience = 15, Category = "Cardiology", Description = "Good doctor" }
            };
            return doctors;
        }

        [Fact]
        public async void GetAllReturnListOfPatients()
        {
            var repo = new Mock<IRepository<Doctor>>();
            var mock = GetTestDoctors().AsQueryable().BuildMock();
            repo.Setup(x => x.GetAll()).Returns(Task.FromResult(mock.Object.AsEnumerable()));
            var controller = new DoctorsController(repo.Object);

            IEnumerable<Doctor> result = await controller.Get();

            Assert.Equal(5, result.Count());
        }

        [Fact]
        public async void GetPatientReturnObjectResult()
        {
            var testDoctor = GetTestDoctors()[0];
            int testDoctorId = testDoctor.Id;
            var repo = new Mock<IRepository<Doctor>>();
            var mock = GetTestDoctors().AsQueryable().BuildMock();
            repo.Setup(x => x.GetById(testDoctorId))
                .ReturnsAsync(testDoctor);
            var controller = new DoctorsController(repo.Object);

            var actionResult = await controller.Get(testDoctorId);
            var result = UnitTestUtility.GetObjectResultContent(actionResult);

            Assert.Equal(testDoctor.Name, result.Name);
            Assert.Equal(testDoctor.Experience, result.Experience);
            Assert.Equal(testDoctor.Category, result.Category);
            Assert.Equal(testDoctor.Description, result.Description);
        }

        [Fact]
        public async void GetPatientReturnNotFoundResult()
        {
            int testDoctorId = 0;
            var repo = new Mock<IRepository<Doctor>>();
            var mock = GetTestDoctors().AsQueryable().BuildMock();
            repo.Setup(x => x.GetById(testDoctorId))
                .ReturnsAsync(GetTestDoctors().FirstOrDefault(p => p.Id == testDoctorId));
            var controller = new DoctorsController(repo.Object);

            var actionResult = await controller.Get(testDoctorId);

            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public async void AddPatientReturnsOkResult()
        {
            var repo = new Mock<IRepository<Doctor>>();
            var controller = new DoctorsController(repo.Object);

            var actionResult = await controller.Post(new Doctor { Id = 6, Name = "Mykola", Experience = 15, Category = "Cardiology", Description = "Good doctor" });

            Assert.IsType<OkObjectResult>(actionResult.Result);
        }

        [Fact]
        public async void AddPatientReturnsBadRequestResult()
        {
            var repo = new Mock<IRepository<Doctor>>();
            var controller = new DoctorsController(repo.Object);

            var actionResult = await controller.Post(null);

            Assert.IsType<BadRequestResult>(actionResult.Result);
        }

        [Fact]
        public async void UpdatePatientReturnsOkResult()
        {
            int testDoctorId = 1;
            var repo = new Mock<IRepository<Doctor>>();
            var mock = GetTestDoctors().AsQueryable().BuildMock();
            repo.Setup(x => x.GetById(testDoctorId))
                .ReturnsAsync(GetTestDoctors().FirstOrDefault(p => p.Id == testDoctorId));
            var controller = new DoctorsController(repo.Object);

            var actionResult = await controller.Put(new Doctor { Id = 1, Name = "Nazar", Experience = 14, Category = "Cardiology", Description = "Bad doctor" });

            Assert.IsType<OkObjectResult>(actionResult.Result);
        }

        [Fact]
        public async void UpdatePatientReturnsBadRequestResult()
        {
            var repo = new Mock<IRepository<Doctor>>();
            var mock = GetTestDoctors().AsQueryable().BuildMock();
            var controller = new DoctorsController(repo.Object);

            var actionResult = await controller.Put(null);

            Assert.IsType<BadRequestResult>(actionResult.Result);
        }

        [Fact]
        public async void UpdatePatientReturnsNotFoundResult()
        {
            int testDoctorId = 8;
            var repo = new Mock<IRepository<Doctor>>();
            var mock = GetTestDoctors().AsQueryable().BuildMock();
            repo.Setup(x => x.GetById(testDoctorId))
                .ReturnsAsync(GetTestDoctors().FirstOrDefault(p => p.Id == testDoctorId));
            var controller = new DoctorsController(repo.Object);

            var actionResult = await controller.Put(new Doctor { Id = 8, Name = "Nazar", Experience = 14, Category = "Cardiology", Description = "Bad doctor" });

            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public async void DeletePatientReturnsOkResult()
        {
            int testDoctortId = 2;
            var repo = new Mock<IRepository<Doctor>>();
            var mock = GetTestDoctors().AsQueryable().BuildMock();
            repo.Setup(x => x.GetById(testDoctortId))
                .ReturnsAsync(GetTestDoctors().FirstOrDefault(p => p.Id == testDoctortId));
            var controller = new DoctorsController(repo.Object);

            var actionResult = await controller.Delete(testDoctortId);

            Assert.IsType<OkResult>(actionResult.Result);
        }

        [Fact]
        public async void DeletePatientReturnsNotFoundResult()
        {
            var repo = new Mock<IRepository<Doctor>>();
            var mock = GetTestDoctors().AsQueryable().BuildMock();
            repo.Setup(x => x.GetAll()).Returns(Task.FromResult(mock.Object.AsEnumerable()));
            var controller = new DoctorsController(repo.Object);

            var actionResult = await controller.Delete(6);

            Assert.IsType<NotFoundResult>(actionResult.Result);
        }
    }
}
