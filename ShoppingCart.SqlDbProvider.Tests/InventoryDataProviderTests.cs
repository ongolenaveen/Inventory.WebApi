using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using Shopping.Core.DomainModels;
using Shopping.SqlDbProvider.Database;
using Shopping.SqlDbProvider.Interfaces;
using Shopping.SqlDbProvider.Models.CsvMappers;
using Shopping.SqlDbProvider.Models.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Shopping.SqlDbProvider.Tests
{
    [TestFixture]
    public class InventoryDataProviderTests:TestBase
    {
        private Mock<IConfiguration> _mockConfiguration;
        private InventoryDatabaseContext _mockInventoryDatabaseContext;
        private Mock<ICsvDataprovider<FruitEntity, FruitEntityMap>> _mockCsvDataProvider;
        private InventoryDataProvider _sut;

        [SetUp]
        public void SetUp()
        {
            _mockConfiguration = _mockRepository.Create<IConfiguration>();
            _mockInventoryDatabaseContext = GetContextWithData();
            _mockCsvDataProvider = _mockRepository.Create<ICsvDataprovider<FruitEntity, FruitEntityMap>>();
            _sut = new InventoryDataProvider(_mockConfiguration.Object, _mockInventoryDatabaseContext, _mockCsvDataProvider.Object);
        }

        #region Upload
        [TestCase("")]
        [TestCase(null)]
        [TestCase(" ")]
        public void Upload_With_Null_FileName_Raises_Exception(string fileName)
        {
            var inventoryFile = _fixture.Create<InventoryFile>();
            inventoryFile.Name = fileName;

            // Act
            Func<Task> action = async () => { await _sut.Upload(inventoryFile); };

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestCase(new byte[0])]
        [TestCase(null)]
        public void Upload_With_Null_FileContent_Raises_Exception(byte[] content)
        {
            var inventoryFile = _fixture.Create<InventoryFile>();
            inventoryFile.Content = content;

            // Act
            Func<Task> action = async () => { await _sut.Upload(inventoryFile); };

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Upload_With_Error_While_Parsing_Csv_Data_Raises_Exception()
        {
            // Arrange
            var inventoryFile = _fixture.Create<InventoryFile>();

            // Mock CSV parser Response
            _mockCsvDataProvider.Setup(x => x.Parse(It.IsAny<Byte[]>())).Throws(new ArgumentNullException());

            // Act
            Func<Task> action = async () => { await _sut.Upload(inventoryFile); };

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public async Task Upload_With_Valid_Request_Returns_Success_Response()
        {
            // Arrange
            var inventoryFile = _fixture.Create<InventoryFile>();
            var fruits = _fixture.CreateMany<FruitEntity>(2);
            var firstFruit = fruits.First();

            // Mock CSV parser Response
            _mockCsvDataProvider.Setup(x => x.Parse(It.IsAny<Byte[]>())).Returns(fruits);

            // Act
            await _sut.Upload(inventoryFile);

            // Assert
            _mockInventoryDatabaseContext.Fruits.Contains(firstFruit);
        }
        #endregion Upload

        #region Retrieve
        [Test]
        public async Task Retrieve_With_Valid_Request_Returns_Groceries_Collection()
        {
            // Arrange

            // Act
            var groceries = await _sut.Retrieve();

            // Assert
            groceries.Should().NotBeNull();
            groceries.Count().Should().Be(2);
        }
        #endregion Retrieve
        private InventoryDatabaseContext GetContextWithData()
        {
            var options = new DbContextOptionsBuilder<InventoryDatabaseContext>()
                              .UseInMemoryDatabase(Guid.NewGuid().ToString())
                              .Options;

            var context = new InventoryDatabaseContext(options);
            context.Fruits.Add(new FruitEntity { Id = Guid.NewGuid(), Name = "Banana", Price = 0.29m, QuantityInStock = 20, UpdatedDate =  DateTime.Now.AddDays(-10) });
            context.Fruits.Add(new FruitEntity { Id = Guid.NewGuid(), Name = "honeydew melon", Price = 1.01m, QuantityInStock = 3, UpdatedDate = DateTime.Now.AddDays(-5) });
            context.SaveChanges();
            return context;
        }
    }
}
