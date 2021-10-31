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

namespace ClinicWebApplication.Tests
{
    public class FeedbacksControllerTests
    {
        private List<Feedback> GetTestFeedbacks()
        {
            var feedbacks = new List<Feedback>
            {
                new Feedback{Id = 1, PatientId = 1, DoctorId = 1, FeedbackText = "feedback1" },
                new Feedback{Id = 2, PatientId = 2, DoctorId = 3, FeedbackText = "feedback2" },
                new Feedback{Id = 3, PatientId = 3, DoctorId = 2, FeedbackText = "feedback3" },
                new Feedback{Id = 4, PatientId = 4, DoctorId = 5, FeedbackText = "feedback4" },
                new Feedback{Id = 5, PatientId = 2, DoctorId = 3, FeedbackText = "feedback5" }
            };
            return feedbacks;
        }

        [Fact]
        public async void GetAllReturnListOfFeedbacks()
        {
            var repo = new Mock<IRepository<Feedback>>();
            var mock = GetTestFeedbacks().AsQueryable().BuildMock();
            repo.Setup(x => x.GetAll()).Returns(Task.FromResult(mock.Object.AsEnumerable()));
            var controller = new FeedbacksController(repo.Object);

            IEnumerable<Feedback> result = await controller.Get();

            Assert.Equal(5, result.Count());
        }

        [Fact]
        public async void GetFeedbackReturnObjectResult()
        {
            var testFeedback = GetTestFeedbacks()[0];
            int testFeedbackId = testFeedback.Id;
            var repo = new Mock<IRepository<Feedback>>();
            var mock = GetTestFeedbacks().AsQueryable().BuildMock();
            repo.Setup(x => x.GetById(testFeedbackId))
                .ReturnsAsync(testFeedback);
            var controller = new FeedbacksController(repo.Object);

            var actionResult = await controller.Get(testFeedbackId);
            var result = UnitTestUtility.GetObjectResultContent(actionResult);

            Assert.Equal(testFeedback.PatientId, result.PatientId);
            Assert.Equal(testFeedback.DoctorId, result.DoctorId);
            Assert.Equal(testFeedback.FeedbackText, result.FeedbackText);
        }

        [Fact]
        public async void GetFeedbackReturnNotFoundResult()
        {
            int testFeedbackId = 0;
            var repo = new Mock<IRepository<Feedback>>();
            var mock = GetTestFeedbacks().AsQueryable().BuildMock();
            repo.Setup(x => x.GetById(testFeedbackId))
                .ReturnsAsync(GetTestFeedbacks().FirstOrDefault(p => p.Id == testFeedbackId));
            var controller = new FeedbacksController(repo.Object);

            var actionResult = await controller.Get(testFeedbackId);

            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public async void AddFeedbackReturnsOkResult()
        {
            var repo = new Mock<IRepository<Feedback>>();
            var controller = new FeedbacksController(repo.Object);

            var actionResult = await controller.Post(new Feedback { Id = 6, PatientId = 1, DoctorId = 1, FeedbackText = "complaint1" });

            Assert.IsType<OkObjectResult>(actionResult.Result);
        }

        [Fact]
        public async void AddFeedbackReturnsBadRequestResult()
        {
            var repo = new Mock<IRepository<Feedback>>();
            var controller = new FeedbacksController(repo.Object);

            var actionResult = await controller.Post(null);

            Assert.IsType<BadRequestResult>(actionResult.Result);
        }

        [Fact]
        public async void UpdateFeedbackReturnsOkResult()
        {
            int testFeedbackId = 1;
            var repo = new Mock<IRepository<Feedback>>();
            var mock = GetTestFeedbacks().AsQueryable().BuildMock();
            repo.Setup(x => x.GetById(testFeedbackId))
                .ReturnsAsync(GetTestFeedbacks().FirstOrDefault(p => p.Id == testFeedbackId));
            var controller = new FeedbacksController(repo.Object);

            var actionResult = await controller.Put(new Feedback { Id = 1, PatientId = 1, DoctorId = 1, FeedbackText = "feedback12131" });

            Assert.IsType<OkObjectResult>(actionResult.Result);
        }

        [Fact]
        public async void UpdateFeedbackReturnsBadRequestResult()
        {
            var repo = new Mock<IRepository<Feedback>>();
            var mock = GetTestFeedbacks().AsQueryable().BuildMock();
            var controller = new FeedbacksController(repo.Object);

            var actionResult = await controller.Put(null);

            Assert.IsType<BadRequestResult>(actionResult.Result);
        }

        [Fact]
        public async void UpdateFeedbackReturnsNotFoundResult()
        {
            int testFeedbackId = 6;
            var repo = new Mock<IRepository<Feedback>>();
            var mock = GetTestFeedbacks().AsQueryable().BuildMock();
            repo.Setup(x => x.GetById(testFeedbackId))
                .ReturnsAsync(GetTestFeedbacks().FirstOrDefault(p => p.Id == testFeedbackId));
            var controller = new FeedbacksController(repo.Object);

            var actionResult = await controller.Put(new Feedback { Id = 6, PatientId = 1, DoctorId = 1, FeedbackText = "feedback1" });

            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public async void DeleteFeedbackReturnsOkResult()
        {
            int testFeedbackId = 2;
            var repo = new Mock<IRepository<Feedback>>();
            var mock = GetTestFeedbacks().AsQueryable().BuildMock();
            repo.Setup(x => x.GetById(testFeedbackId))
                .ReturnsAsync(GetTestFeedbacks().FirstOrDefault(p => p.Id == testFeedbackId));
            var controller = new FeedbacksController(repo.Object);

            var actionResult = await controller.Delete(testFeedbackId);

            Assert.IsType<OkResult>(actionResult.Result);
        }

        [Fact]
        public async void DeleteFeedbackReturnsNotFoundResult()
        {
            var repo = new Mock<IRepository<Feedback>>();
            var mock = GetTestFeedbacks().AsQueryable().BuildMock();
            repo.Setup(x => x.GetAll()).Returns(Task.FromResult(mock.Object.AsEnumerable()));
            var controller = new FeedbacksController(repo.Object);

            var actionResult = await controller.Delete(7);

            Assert.IsType<NotFoundResult>(actionResult.Result);
        }
    }
}
