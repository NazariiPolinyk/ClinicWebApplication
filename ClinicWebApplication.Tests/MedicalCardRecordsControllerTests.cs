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

namespace ClinicWebApplication.Tests
{
    public class MedicalCardRecordsControllerTests
    {
        private List<MedicalCardRecord> GetTestMedicalCardRecords()
        {
            var medicalCardRecords = new List<MedicalCardRecord>
            {
                new MedicalCardRecord{Id = 1, PatientId = 1, DoctorId = 1, Diagnosis = "MedicalCardRecord1", DateTime = new DateTime(2021, 7, 12) },
                new MedicalCardRecord{Id = 2, PatientId = 2, DoctorId = 3, Diagnosis = "MedicalCardRecord2", DateTime = new DateTime(2021, 9, 7) },
                new MedicalCardRecord{Id = 3, PatientId = 3, DoctorId = 2, Diagnosis = "MedicalCardRecord3", DateTime = new DateTime(2021, 5, 9) },
                new MedicalCardRecord{Id = 4, PatientId = 4, DoctorId = 5, Diagnosis = "MedicalCardRecord4", DateTime = new DateTime(2021, 2, 16) },
                new MedicalCardRecord{Id = 5, PatientId = 2, DoctorId = 3, Diagnosis = "MedicalCardRecord5", DateTime = new DateTime(2021, 4, 2) }
            };
            return medicalCardRecords;
        }

        [Fact]
        public async void GetAllReturnListOfMedicalCardRecords()
        {
            var repo = new Mock<IRepository<MedicalCardRecord>>();
            var mock = GetTestMedicalCardRecords().AsQueryable().BuildMock();
            repo.Setup(x => x.GetAll()).Returns(Task.FromResult(mock.Object.AsEnumerable()));
            var controller = new MedicalCardRecordsController(repo.Object);

            IEnumerable<MedicalCardRecord> result = await controller.Get();

            Assert.Equal(5, result.Count());
        }

        [Fact]
        public async void GetMedicalCardRecordReturnObjectResult()
        {
            var testMedicalCardRecord = GetTestMedicalCardRecords()[0];
            int testMedicalCardRecordId = testMedicalCardRecord.Id;
            var repo = new Mock<IRepository<MedicalCardRecord>>();
            var mock = GetTestMedicalCardRecords().AsQueryable().BuildMock();
            repo.Setup(x => x.GetById(testMedicalCardRecordId))
                .ReturnsAsync(testMedicalCardRecord);
            var controller = new MedicalCardRecordsController(repo.Object);

            var actionResult = await controller.Get(testMedicalCardRecordId);
            var result = UnitTestUtility.GetObjectResultContent(actionResult);

            Assert.Equal(testMedicalCardRecord.PatientId, result.PatientId);
            Assert.Equal(testMedicalCardRecord.DoctorId, result.DoctorId);
            Assert.Equal(testMedicalCardRecord.Diagnosis, result.Diagnosis);
            Assert.Equal(testMedicalCardRecord.DateTime, result.DateTime);
        }

        [Fact]
        public async void GetMedicalCardRecordReturnNotFoundResult()
        {
            int testMedicalCardRecordId = 0;
            var repo = new Mock<IRepository<MedicalCardRecord>>();
            var mock = GetTestMedicalCardRecords().AsQueryable().BuildMock();
            repo.Setup(x => x.GetById(testMedicalCardRecordId))
                .ReturnsAsync(GetTestMedicalCardRecords().FirstOrDefault(p => p.Id == testMedicalCardRecordId));
            var controller = new MedicalCardRecordsController(repo.Object);

            var actionResult = await controller.Get(testMedicalCardRecordId);

            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public async void AddMedicalCardRecordReturnsOkResult()
        {
            var repo = new Mock<IRepository<MedicalCardRecord>>();
            var controller = new MedicalCardRecordsController(repo.Object);

            var actionResult = await controller.Post(new MedicalCardRecord { Id = 6, PatientId = 1, DoctorId = 1, Diagnosis = "MedicalCardRecord6", DateTime = new DateTime(2021, 4, 2) });

            Assert.IsType<OkObjectResult>(actionResult.Result);
        }

        [Fact]
        public async void AddMedicalCardRecordReturnsBadRequestResult()
        {
            var repo = new Mock<IRepository<MedicalCardRecord>>();
            var controller = new MedicalCardRecordsController(repo.Object);

            var actionResult = await controller.Post(null);

            Assert.IsType<BadRequestResult>(actionResult.Result);
        }

        [Fact]
        public async void UpdateMedicalCardRecordReturnsOkResult()
        {
            int testMedicalCardRecordId = 1;
            var repo = new Mock<IRepository<MedicalCardRecord>>();
            var mock = GetTestMedicalCardRecords().AsQueryable().BuildMock();
            repo.Setup(x => x.GetById(testMedicalCardRecordId))
                .ReturnsAsync(GetTestMedicalCardRecords().FirstOrDefault(p => p.Id == testMedicalCardRecordId));
            var controller = new MedicalCardRecordsController(repo.Object);

            var actionResult = await controller.Put(new MedicalCardRecord { Id = 1, PatientId = 1, DoctorId = 1, Diagnosis = "MedicalCardRecord12131", DateTime = new DateTime(2021, 4, 2) });

            Assert.IsType<OkObjectResult>(actionResult.Result);
        }

        [Fact]
        public async void UpdateMedicalCardRecordReturnsBadRequestResult()
        {
            var repo = new Mock<IRepository<MedicalCardRecord>>();
            var mock = GetTestMedicalCardRecords().AsQueryable().BuildMock();
            var controller = new MedicalCardRecordsController(repo.Object);

            var actionResult = await controller.Put(null);

            Assert.IsType<BadRequestResult>(actionResult.Result);
        }

        [Fact]
        public async void UpdateMedicalCardRecordReturnsNotFoundResult()
        {
            int testMedicalCardRecordId = 6;
            var repo = new Mock<IRepository<MedicalCardRecord>>();
            var mock = GetTestMedicalCardRecords().AsQueryable().BuildMock();
            repo.Setup(x => x.GetById(testMedicalCardRecordId))
                .ReturnsAsync(GetTestMedicalCardRecords().FirstOrDefault(p => p.Id == testMedicalCardRecordId));
            var controller = new MedicalCardRecordsController(repo.Object);

            var actionResult = await controller.Put(new MedicalCardRecord { Id = 6, PatientId = 1, DoctorId = 1, Diagnosis = "MedicalCardRecord1", DateTime = new DateTime(2021, 4, 2) });

            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public async void DeleteMedicalCardRecordReturnsOkResult()
        {
            int testMedicalCardRecordId = 2;
            var repo = new Mock<IRepository<MedicalCardRecord>>();
            var mock = GetTestMedicalCardRecords().AsQueryable().BuildMock();
            repo.Setup(x => x.GetById(testMedicalCardRecordId))
                .ReturnsAsync(GetTestMedicalCardRecords().FirstOrDefault(p => p.Id == testMedicalCardRecordId));
            var controller = new MedicalCardRecordsController(repo.Object);

            var actionResult = await controller.Delete(testMedicalCardRecordId);

            Assert.IsType<OkResult>(actionResult.Result);
        }

        [Fact]
        public async void DeleteMedicalCardRecordReturnsNotFoundResult()
        {
            var repo = new Mock<IRepository<MedicalCardRecord>>();
            var mock = GetTestMedicalCardRecords().AsQueryable().BuildMock();
            repo.Setup(x => x.GetAll()).Returns(Task.FromResult(mock.Object.AsEnumerable()));
            var controller = new MedicalCardRecordsController(repo.Object);

            var actionResult = await controller.Delete(7);

            Assert.IsType<NotFoundResult>(actionResult.Result);
        }
    }
}
