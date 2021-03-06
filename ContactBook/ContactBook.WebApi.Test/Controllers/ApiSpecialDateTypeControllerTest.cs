﻿using AutoMapper;
using ContactBook.Db.Data;
using ContactBook.Db.Repositories;
using ContactBook.Domain.Models;
using ContactBook.WebApi.Controllers;
using ContactBook.WebApi.Test.Fixtures;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Routing;
using Xunit;

namespace ContactBook.WebApi.Test.Controllers
{
    public class ApiSpecialDateTypeControllerTest : IClassFixture<ControllerTestFixtures>
    {
        private CancellationTokenSource cts;
        private List<SpecialDateType> specialDateTypeList = null;

        public ApiSpecialDateTypeControllerTest(ControllerTestFixtures fixture)
        {
            ControllerFixture = fixture;
            cts = new CancellationTokenSource(10000);
            specialDateTypeList = new List<SpecialDateType>()
            {
                new SpecialDateType(){ SpecialDateTpId = 1, Date_TypeName="Home", BookId=null},
                new SpecialDateType(){ SpecialDateTpId = 2, Date_TypeName="Office", BookId=null},
                new SpecialDateType(){ SpecialDateTpId = 3, Date_TypeName="abc", BookId=1}
            };
        }

        public ControllerTestFixtures ControllerFixture { get; private set; }

        [Fact]
        public void GetSpecialDateTypeShouldReturnNotFound()
        {
            //Arrange
            ControllerFixture.MockUnitOfWork.Setup(rp => rp.GetEntityByType<SpecialDateType>()).Returns(() => ControllerFixture.MockRepository<SpecialDateType>(null));

            ApiSpecialDateTypeController typeCnt = new ApiSpecialDateTypeController(ControllerFixture.MockUnitOfWork.Object, ControllerFixture.MockUnitOfWork.Object);
            typeCnt.Request = new HttpRequestMessage();
            typeCnt.Configuration = new HttpConfiguration();

            //act
            HttpResponseMessage result = ControllerFixture.GetResponseMessage(typeCnt.Get(1), cts.Token);

            //Assert
            Assert.True(result.StatusCode == HttpStatusCode.NotFound);
        }

        [Fact]
        public void GetSpecialDateTypeShouldReturnNumber()
        {
            //Arrange
            ControllerFixture.MockUnitOfWork.Setup(rp => rp.GetEntityByType<SpecialDateType>()).Returns(
                () => ControllerFixture.MockRepository<SpecialDateType>(specialDateTypeList
                    ));

            ApiSpecialDateTypeController typeCntr = new ApiSpecialDateTypeController(ControllerFixture.MockUnitOfWork.Object, ControllerFixture.MockUnitOfWork.Object);
            typeCntr.Request = new HttpRequestMessage();
            typeCntr.Configuration = new HttpConfiguration();

            //Act
            HttpResponseMessage result = ControllerFixture.GetResponseMessage(typeCntr.Get(1), cts.Token);

            List<SpecialDateTypeModel> resultType;
            result.TryGetContentValue<List<SpecialDateTypeModel>>(out resultType);

            //Assert
            Assert.True(result.StatusCode == HttpStatusCode.OK);

            Assert.NotNull(resultType);

            Assert.True(resultType.Count == 3);
        }

        [Fact]
        public void GetSpecialDateTypeShouldOnlyDefaultNumbers()
        {
            //Arrange
            ControllerFixture.MockUnitOfWork.Setup(rp => rp.GetEntityByType<SpecialDateType>()).Returns(
                () => ControllerFixture.MockRepository<SpecialDateType>(specialDateTypeList
                    ));
            var config = new HttpConfiguration();
            ControllerFixture.RouteConfig(config);
            ApiSpecialDateTypeController typeCntr = new ApiSpecialDateTypeController(ControllerFixture.MockUnitOfWork.Object, ControllerFixture.MockUnitOfWork.Object);
            typeCntr.Request = new HttpRequestMessage();
            typeCntr.Configuration = config;

            //Act
            HttpResponseMessage result = ControllerFixture.GetResponseMessage(typeCntr.Get(-1), cts.Token);

            List<SpecialDateTypeModel> resultType;
            result.TryGetContentValue<List<SpecialDateTypeModel>>(out resultType);

            //Assert
            Assert.True(result.StatusCode == HttpStatusCode.OK);

            Assert.NotNull(resultType);

            Assert.True(resultType.Count == 2);
        }

        [Fact]
        public void ShouldInsertSpecialDateType()
        {
            //Arrange
            List<SpecialDateType> typeResult = new List<SpecialDateType>();
            var specialDateType = new SpecialDateTypeModel()
            {
                SpecialDateTpId = 1,
                DateTypeName = "Home",
                BookId = 1
            };
            var cbSpecialDateType = new SpecialDateType()
            {
                SpecialDateTpId = 1,
                Date_TypeName = "Home",
                BookId = 1
            };

            Mock<IContactBookDbRepository<SpecialDateType>> mockRepo = ControllerFixture.MockRepository<SpecialDateType>();
            mockRepo.Setup(s => s.Insert(It.IsAny<SpecialDateType>())).Callback<SpecialDateType>(
                c =>
                {
                    Mapper.CreateMap<SpecialDateType, SpecialDateTypeModel>().ForMember(at => at.DateTypeName, cb => cb.MapFrom(m => m.Date_TypeName));

                    typeResult.Add(Mapper.Map<SpecialDateType>(c));
                });

            ControllerFixture.MockUnitOfWork.Setup(rp => rp.GetEntityByType<SpecialDateType>()).Returns(() =>
            {
                return mockRepo.Object;
            });

            ApiSpecialDateTypeController relationshipController = new ApiSpecialDateTypeController(ControllerFixture.MockUnitOfWork.Object, ControllerFixture.MockUnitOfWork.Object);

            //Act
            relationshipController.Post(specialDateType);

            //Assert
            Assert.NotEmpty(typeResult);
        }

        [Fact]
        public void ShouldInsertNumberAndReturnCreatedAndLocation()
        {
            //Arrange
            bool saveCalled = false;
            List<SpecialDateType> typeResult = new List<SpecialDateType>();

            var specialDateType = new SpecialDateTypeModel()
            {
                SpecialDateTpId = 1,
                DateTypeName = "Home",
                BookId = 1
            };

            var cbSpecialDateType = new SpecialDateType()
            {
                SpecialDateTpId = 1,
                Date_TypeName = "Home",
                BookId = 1
            };

            Mock<IContactBookDbRepository<SpecialDateType>> mockRepo = ControllerFixture.MockRepository<SpecialDateType>();
            mockRepo.Setup(s => s.Insert(It.IsAny<SpecialDateType>())).Callback<SpecialDateType>(
                c =>
                {
                    Mapper.CreateMap<SpecialDateType, SpecialDateTypeModel>().ForMember(at => at.DateTypeName, cb => cb.MapFrom(m => m.Date_TypeName));

                    typeResult.Add(Mapper.Map<SpecialDateType>(c));
                });

            ControllerFixture.MockUnitOfWork.Setup(rp => rp.GetEntityByType<SpecialDateType>()).Returns(() =>
            {
                return mockRepo.Object;
            });

            ControllerFixture.MockUnitOfWork.Setup(rp => rp.Save()).Callback(() =>
            {
                saveCalled = true;
            });

            Mock<UrlHelper> urlHelperMock = new Mock<UrlHelper>();
            urlHelperMock.Setup(u => u.Link(It.IsAny<string>(), It.IsAny<object>())).Returns("http://localhost");
            urlHelperMock.Setup(u => u.Link(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).Returns("http://localhost");

            var config = new HttpConfiguration();
            ControllerFixture.RouteConfig(config);

            ApiSpecialDateTypeController typeController = new ApiSpecialDateTypeController(ControllerFixture.MockUnitOfWork.Object, ControllerFixture.MockUnitOfWork.Object);
            typeController.Request = new HttpRequestMessage() { RequestUri = new Uri("http://localhost/api/testcontroller/get") };
            typeController.Configuration = config;
            typeController.Url = urlHelperMock.Object;

            //Act
            HttpResponseMessage resMsg = ControllerFixture.GetResponseMessage(typeController.Post(specialDateType), cts.Token);

            //Assert
            Assert.True(saveCalled);
            Assert.True(resMsg.StatusCode == HttpStatusCode.Created);
            Assert.NotNull(resMsg.Headers.Location);
        }

        [Fact]
        public void ShouldUpdateNumberOk()
        {
            //Arrange
            bool saveCalled = false;
            List<SpecialDateType> typeResult = new List<SpecialDateType>();

            var cbType = new SpecialDateTypeModel()
            {
                SpecialDateTpId = 3,
                DateTypeName = "Updated",
                BookId = 1
            };

            Mock<IContactBookDbRepository<SpecialDateType>> mockRepo = ControllerFixture.MockRepositoryNeedList<SpecialDateType>(specialDateTypeList);

            mockRepo.Setup(s => s.Update(It.IsAny<SpecialDateType>(), It.IsAny<SpecialDateType>())).Callback<SpecialDateType, SpecialDateType>(
                (a, c) =>
                {
                    SpecialDateType upGroup = specialDateTypeList.Where(cb => cb.SpecialDateTpId == c.SpecialDateTpId).SingleOrDefault();
                    upGroup.Date_TypeName = c.Date_TypeName;
                });

            ControllerFixture.MockUnitOfWork.Setup(rp => rp.GetEntityByType<SpecialDateType>()).Returns(() =>
            {
                return mockRepo.Object;
            });

            ControllerFixture.MockUnitOfWork.Setup(rp => rp.Save()).Callback(() =>
            {
                saveCalled = true;
            });

            var config = new HttpConfiguration();
            ControllerFixture.RouteConfig(config);

            ApiSpecialDateTypeController typeController = new ApiSpecialDateTypeController(ControllerFixture.MockUnitOfWork.Object, ControllerFixture.MockUnitOfWork.Object);
            typeController.Request = new HttpRequestMessage() { RequestUri = new Uri("http://localhost/api/testcontroller/get") };
            typeController.Configuration = config;

            //Act
            HttpResponseMessage resMsg = ControllerFixture.GetResponseMessage(typeController.Put(cbType), cts.Token);

            //Assert
            Assert.True(saveCalled);
            Assert.True(resMsg.StatusCode == HttpStatusCode.OK);
            Assert.True(specialDateTypeList.Where(cb => cb.SpecialDateTpId == 3).SingleOrDefault().Date_TypeName == "Updated");
        }

        [Fact]
        public void SpecialDateTypeUpdateShouldReturnNotFound()
        {
            //Arrange
            var cbType = new SpecialDateTypeModel()
            {
                SpecialDateTpId = -3,
                DateTypeName = "Updated",
                BookId = -1
            };

            Mock<IContactBookDbRepository<SpecialDateType>> mockRepo = ControllerFixture.MockRepositoryNeedList<SpecialDateType>(specialDateTypeList);

            ControllerFixture.MockUnitOfWork.Setup(rp => rp.GetEntityByType<SpecialDateType>()).Returns(() =>
            {
                return mockRepo.Object;
            });

            var config = new HttpConfiguration();
            ControllerFixture.RouteConfig(config);

            ApiSpecialDateTypeController typeController = new ApiSpecialDateTypeController(ControllerFixture.MockUnitOfWork.Object, ControllerFixture.MockUnitOfWork.Object);
            typeController.Request = new HttpRequestMessage() { RequestUri = new Uri("http://localhost/api/testcontroller/get") };
            typeController.Configuration = config;

            //Act
            HttpResponseMessage resMsg = ControllerFixture.GetResponseMessage(typeController.Put(cbType), cts.Token);

            //Assert
            Assert.True(resMsg.StatusCode == HttpStatusCode.NotFound);
        }

        [Fact]
        public void ShouldDeleteSpecialDateTypeAndOked()
        {
            //Arrange
            bool saveCalled = false;
            List<SpecialDateType> typeResult = new List<SpecialDateType>();

            var specialdateType = new SpecialDateTypeModel()
            {
                SpecialDateTpId = 3,
                DateTypeName = "Updated",
                BookId = 1
            };

            Mock<IContactBookDbRepository<SpecialDateType>> mockRepo = ControllerFixture.MockRepositoryNeedList<SpecialDateType>(specialDateTypeList);

            mockRepo.Setup(s => s.Delete(It.IsAny<SpecialDateType>())).Callback<SpecialDateType>(
                c =>
                {
                    SpecialDateType delAddr = specialDateTypeList.Where(cb => cb.SpecialDateTpId == c.SpecialDateTpId).SingleOrDefault();
                    specialDateTypeList.Remove(delAddr);
                });

            ControllerFixture.MockUnitOfWork.Setup(rp => rp.GetEntityByType<SpecialDateType>()).Returns(() =>
            {
                return mockRepo.Object;
            });

            ControllerFixture.MockUnitOfWork.Setup(rp => rp.Save()).Callback(() =>
            {
                saveCalled = true;
            });

            var config = new HttpConfiguration();
            ControllerFixture.RouteConfig(config);

            ApiSpecialDateTypeController typeController = new ApiSpecialDateTypeController(ControllerFixture.MockUnitOfWork.Object, ControllerFixture.MockUnitOfWork.Object);
            typeController.Request = new HttpRequestMessage();
            typeController.Configuration = config;

            //Act
            HttpResponseMessage resMsg = ControllerFixture.GetResponseMessage(typeController.Delete(specialdateType.BookId.Value, specialdateType.SpecialDateTpId), cts.Token);

            //Assert
            Assert.True(saveCalled);
            Assert.True(resMsg.StatusCode == HttpStatusCode.OK);
            Assert.Null(specialDateTypeList.Where(cb => cb.SpecialDateTpId == 3).SingleOrDefault());
        }

        [Fact]
        public void SpecialDateTypeDeleteReturnNotFound()
        {
            //Arrange
            List<SpecialDateType> typeResult = new List<SpecialDateType>();

            var cbType = new SpecialDateTypeModel()
            {
                SpecialDateTpId = 3,
                DateTypeName = "Updated",
                BookId = 1
            };

            Mock<IContactBookDbRepository<SpecialDateType>> mockRepo = ControllerFixture.MockRepositoryNeedList<SpecialDateType>(specialDateTypeList);

            ControllerFixture.MockUnitOfWork.Setup(rp => rp.GetEntityByType<SpecialDateType>()).Returns(() =>
            {
                return mockRepo.Object;
            });

            var config = new HttpConfiguration();
            ControllerFixture.RouteConfig(config);

            ApiSpecialDateTypeController typeController = new ApiSpecialDateTypeController(ControllerFixture.MockUnitOfWork.Object, ControllerFixture.MockUnitOfWork.Object);
            typeController.Request = new HttpRequestMessage();
            typeController.Configuration = config;

            //Act
            HttpResponseMessage resMsg = ControllerFixture.GetResponseMessage(typeController.Delete(2, 1), cts.Token);

            //Assert
            Assert.True(resMsg.StatusCode == HttpStatusCode.NotFound);
        }
    }
}