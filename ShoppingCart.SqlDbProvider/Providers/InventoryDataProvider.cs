using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Shopping.Core.DomainModels;
using Shopping.Core.Interfaces;
using Shopping.Extensions;
using Shopping.SqlDbProvider.Database;
using Shopping.SqlDbProvider.Interfaces;
using Shopping.SqlDbProvider.Models.CsvMappers;
using Shopping.SqlDbProvider.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopping.SqlDbProvider
{
    /// <summary>
    /// This class contains all the contracts needed for Inventory Data Provider
    /// </summary>
    public class InventoryDataProvider : IInventoryDataProvider
    {
        private readonly IConfiguration _configuration;
        private readonly InventoryDatabaseContext _inventoryDatabaseContext;
        private readonly ICsvDataprovider<FruitEntity,FruitEntityMap> _csvDataProvider;

        /// <summary>
        /// Constructor for Inventory Data Provider
        /// </summary>
        /// <param name="config">Configuration</param>
        /// <param name="shoppingDatabaseContext">Shopping Database Context</param>
        /// <param name="csvDataProvider">CSV Data provider</param>
        public InventoryDataProvider(IConfiguration config,
            InventoryDatabaseContext inventoryDatabaseContext,
            ICsvDataprovider<FruitEntity, FruitEntityMap> csvDataProvider)
        {
            _configuration = config ?? throw new ArgumentNullException(nameof(config));
            _inventoryDatabaseContext = inventoryDatabaseContext ?? throw new ArgumentNullException(nameof(inventoryDatabaseContext));
            _csvDataProvider = csvDataProvider ?? throw new ArgumentNullException(nameof(csvDataProvider));
        }

        /// <summary>
        /// Upload Inventory File into Database
        /// </summary>
        /// <param name="inventoryfile">Inventory File</param>
        /// <returns>Task</returns>
        public async Task Upload(InventoryFile inventoryFile)
        {
            // Validate the received File Data
            inventoryFile.Name.ThrowIfIsNullOrWhitespace(nameof(inventoryFile.Name));
            inventoryFile.Content.ThrowIfIsNullOrEmpty(nameof(inventoryFile.Content));

            // Parse the File Content into Fruits Collection
            var fruits = _csvDataProvider.Parse(inventoryFile.Content);

            // If no Fruits are returned, dont proceed
            if(fruits == null || fruits.Any())
                return;
          
            // Iterate through fruits and get add fruit to database
            foreach(var fruit in fruits)
            {
                fruit.Id = Guid.NewGuid();
                await _inventoryDatabaseContext.Fruits.AddAsync(fruit);
            }

            // Commit changes to database
            await _inventoryDatabaseContext.Save();
        }

        /// <summary>
        /// Retrieve Groceries from Inventory
        /// </summary>
        /// <returns>Collection of Groceries</returns>
        public async Task<IEnumerable<Fruit>> Retrieve()
        {
            var fruits = new List<Fruit>();

            // Retrieve Fruits from Database
            var fruitEntities = await (from fruit in _inventoryDatabaseContext.Fruits
                          orderby fruit.UpdatedDate
                          select fruit).ToListAsync();
            // If there are fruits iterate through the list and convert each entity to domain model
            if (fruitEntities != null && fruitEntities.Any())
            {
                foreach(var fruitEntity in fruitEntities)
                {
                    fruits.Add(new Fruit { Name = fruitEntity.Name, Price = fruitEntity.Price, QuantityInStock = fruitEntity.QuantityInStock, UpdatedDate = fruitEntity.UpdatedDate });
                }
            }
            return fruits;
        }
    }
}
