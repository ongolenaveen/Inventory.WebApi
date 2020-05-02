using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Shopping.Core.DomainModels;
using Shopping.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shopping.Api.Tests
{
    [TestFixture]
    public class GroceriesControllerTests:TestBase
    {
        private Mock<IInventoryDataProvider> _mockInventoryDataProvider;
        private GroceriesController _sut;

        [SetUp]
        public void Setup()
        {
            _mockInventoryDataProvider = _mockRepository.Create<IInventoryDataProvider>();
            _sut = new GroceriesController(_mockInventoryDataProvider.Object);
        }

        #region Upload
        [Test]
        public void GroceriesController_With_Null_InventoryDataProvider_Raises_Exception()
        {
            Action action = ()=> { _sut = new GroceriesController(null); };

            action.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public async Task Upload_With_InValid_InventoryFile_Returns_BadRequest_Response()
        {
            var inventoryFile = _fixture.Create<InventoryFile>();
            inventoryFile.Name = default;
            _sut.ModelState.AddModelError("Name", "Name is Required");
            var response = await _sut.Upload(inventoryFile);
            response.Should().BeOfType<BadRequestObjectResult>();
        }

        [Test]
        public void Upload_With_Error_While_Uploading_Raises_Exception()
        {
            var inventoryFile = _fixture.Create<InventoryFile>();
            _mockInventoryDataProvider.Setup(x => x.Upload(It.IsAny<InventoryFile>())).Throws(new SystemException());
            Func<Task> action = async () => await _sut.Upload(inventoryFile);
            action.Should().Throw<SystemException>();
        }

        [Test]
        public async Task Upload_With_Valid_Request_Returns_Valid_Response()
        {
            var inventoryFile = _fixture.Create<InventoryFile>();
            _mockInventoryDataProvider.Setup(x => x.Upload(It.IsAny<InventoryFile>())).Returns(Task.FromResult<object>(null));
            var response = await _sut.Upload(inventoryFile);
            response.Should().BeOfType<OkResult>();
        }
        #endregion upload

        #region GetGroceries
        [Test]
        public void Get_With_Error_While_Retrieving_Groceries_Raises_Exception()
        {
            _mockInventoryDataProvider.Setup(x => x.Retrieve()).Throws(new SystemException());
            Func<Task> action = async () => await _sut.Get();
            action.Should().Throw<SystemException>();
        }

        [Test]
        public async Task Get_With_Valid_Request_Returns_Valid_Response()
        {
            var groceries = _fixture.CreateMany<Fruit>(2);
            _mockInventoryDataProvider.Setup(x => x.Retrieve()).Returns(Task.FromResult<IEnumerable<Fruit>>(groceries));
            var response = await _sut.Get();
            response.Result.Should().BeOfType(typeof(OkObjectResult));
        }
        #endregion GetGroceries
    }
}
