using Microsoft.AspNetCore.Mvc;
using Inventory.Core.DomainModels;
using Inventory.Core.Interfaces;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System;

namespace Inventory.Api
{
    /// <summary>
    /// Groceries Controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public  class GroceriesController : ControllerBase {

        private readonly IInventoryDataProvider _inventoryDataProvider;

        /// <summary>
        /// Constructor for Groceries Controller
        /// </summary>
        /// <param name="inventoryDataProvider">Inventory Data Provider</param>
        public GroceriesController(IInventoryDataProvider inventoryDataProvider)
        {
            _inventoryDataProvider = inventoryDataProvider??throw new ArgumentNullException(nameof(inventoryDataProvider));
        }

        /// <summary>
        /// Upload Inventory File into Database
        /// </summary>
        /// <param name="inventoryFile">Inventory File </param>
        /// <returns>Action Result</returns>
        [HttpPost("Upload")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Upload(InventoryFile inventoryFile)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _inventoryDataProvider.Upload(inventoryFile);
            return Ok();
        }

        /// <summary>
        /// Retrieve Groceries from Inventory
        /// </summary>
        /// <returns>Groceries from Inventory</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Fruit>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IEnumerable<Fruit>>> Get()
        {
            var response = await _inventoryDataProvider.Retrieve();
            return Ok(response);
        }
    }
}
