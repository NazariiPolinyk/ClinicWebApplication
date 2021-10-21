using Xunit;
using ClinicWebApplication.Controllers;
using ClinicWebApplication.Repository;
using ClinicWebApplication.Models;
using Moq;
using MockQueryable.Moq;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace ClinicWebApplication.Tests
{
    public class AppoinmentsControllerTests
    {
        private List<Appoinment> GetTestAppoinments()
        {
            var appoinments = new List<Appoinment>
            {
                new Appoinment{Id = 1, PatientId = 1, DoctorId = 1, Description = "complaint1", IsEnable = true },
                new Appoinment{Id = 2, PatientId = 2, DoctorId = 3, Description = "complaint2", IsEnable = true },
                new Appoinment{Id = 3, PatientId = 3, DoctorId = 2, Description = "complaint3", IsEnable = true },
                new Appoinment{Id = 4, PatientId = 4, DoctorId = 5, Description = "complaint4", IsEnable = true },
                new Appoinment{Id = 5, PatientId = 2, DoctorId = 3, Description = "complaint5", IsEnable = true }
            };
            return appoinments;
        }

        [Fact]
        public async void GetAllReturnListOfAppoinments()
        {
            var repo = new Mock<IRepository<Appoinment>>();
            var mock = GetTestAppoinments().AsQueryable().BuildMock();
            repo.Setup(x => x.GetAll()).Returns(mock.Object);
            var controller = new AppoinmentsController(repo.Object);

            ActionResult<IEnumerable<Appoinment>> result = await controller.Get();

            Assert.Equal(5, result.Value.Count());
        }

        [Fact]
        public async void GetAppoinmentReturnObjectResult()
        {
            var testAppoinment = GetTestAppoinments()[0];
            int testAppoinmentId = testAppoinment.Id;
            var repo = new Mock<IRepository<Appoinment>>();
            var mock = GetTestAppoinments().AsQueryable().BuildMock();
            repo.Setup(x => x.GetById(testAppoinmentId))
                .ReturnsAsync(GetTestAppoinments().FirstOrDefault(p => p.Id == testAppoinmentId));
            var controller = new AppoinmentsController(repo.Object);

            var actionResult = await controller.Get(testAppoinmentId);
            var result = UnitTestUtility.GetObjectResultContent(actionResult);

            Assert.Equal(testAppoinment.PatientId, result.PatientId);
            Assert.Equal(testAppoinment.DoctorId, result.DoctorId);
            Assert.Equal(testAppoinment.Description, result.Description);
            Assert.Equal(testAppoinment.IsEnable, result.IsEnable);
        }

        [Fact]
        public async void GetAppoinmentReturnNotFoundResult()
        {
            int testAppoinmentId = 0;
            var repo = new Mock<IRepository<Appoinment>>();
            var mock = GetTestAppoinments().AsQueryable().BuildMock();
            repo.Setup(x => x.GetById(testAppoinmentId))
                .ReturnsAsync(GetTestAppoinments().FirstOrDefault(p => p.Id == testAppoinmentId));
            var controller = new AppoinmentsController(repo.Object);

            var actionResult = await controller.Get(testAppoinmentId);

            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public async void AddAppoinmentReturnsOkResult()
        {
            var repo = new Mock<IRepository<Appoinment>>();
            var controller = new AppoinmentsController(repo.Object);

            var actionResult = await controller.Post(new Appoinment { Id = 6, PatientId = 1, DoctorId = 1, Description = "complaint1", IsEnable = true });

            Assert.IsType<OkObjectResult>(actionResult.Result);
        }

        [Fact]
        public async void AddAppoinmentReturnsBadRequestResult()
        {
            var repo = new Mock<IRepository<Appoinment>>();
            var controller = new AppoinmentsController(repo.Object);

            var actionResult = await controller.Post(null);

            Assert.IsType<BadRequestResult>(actionResult.Result);
        }

        [Fact]
        public async void UpdateAppoinmentReturnsOkResult()
        {
            var repo = new Mock<IRepository<Appoinment>>();
            var mock = GetTestAppoinments().AsQueryable().BuildMock();
            repo.Setup(x => x.GetAll()).Returns(mock.Object);
            var controller = new AppoinmentsController(repo.Object);

            var actionResult = await controller.Put(new Appoinment { Id = 1, PatientId = 1, DoctorId = 1, Description = "complaint12131", IsEnable = true });

            Assert.IsType<OkObjectResult>(actionResult.Result);
        }

        [Fact]
        public async void UpdateAppoinmentReturnsBadRequestResult()
        {
            var repo = new Mock<IRepository<Appoinment>>();
            var mock = GetTestAppoinments().AsQueryable().BuildMock();
            repo.Setup(x => x.GetAll()).Returns(mock.Object);
            var controller = new AppoinmentsController(repo.Object);

            var actionResult = await controller.Put(null);

            Assert.IsType<BadRequestResult>(actionResult.Result);
        }

        [Fact]
        public async void UpdateAppoinmentReturnsNotFoundResult()
        {
            var repo = new Mock<IRepository<Appoinment>>();
            var mock = GetTestAppoinments().AsQueryable().BuildMock();
            repo.Setup(x => x.GetAll()).Returns(mock.Object);
            var controller = new AppoinmentsController(repo.Object);

            var actionResult = await controller.Put(new Appoinment { Id = 6, PatientId = 1, DoctorId = 1, Description = "complaint1", IsEnable = true });

            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public async void DeleteAppoinmentReturnsOkResult()
        {
            int testAppoinmentId = 2;
            var repo = new Mock<IRepository<Appoinment>>();
            var mock = GetTestAppoinments().AsQueryable().BuildMock();
            repo.Setup(x => x.GetById(testAppoinmentId))
                .ReturnsAsync(GetTestAppoinments().FirstOrDefault(p => p.Id == testAppoinmentId));
            var controller = new AppoinmentsController(repo.Object);

            var actionResult = await controller.Delete(testAppoinmentId);

            Assert.IsType<OkObjectResult>(actionResult.Result);
        }

        [Fact]
        public async void DeleteAppoinmentReturnsNotFoundResult()
        {
            var repo = new Mock<IRepository<Appoinment>>();
            var mock = GetTestAppoinments().AsQueryable().BuildMock();
            repo.Setup(x => x.GetAll()).Returns(mock.Object);
            var controller = new AppoinmentsController(repo.Object);

            var actionResult = await controller.Delete(7);

            Assert.IsType<NotFoundResult>(actionResult.Result);
        }
    }
}
