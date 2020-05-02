using System.ComponentModel.DataAnnotations;

namespace Shopping.Core.DomainModels
{
    /// <summary>
    /// Inventory File Domain Model
    /// </summary>
    public class InventoryFile
    {
        /// <summary>
        /// Name of the File
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        /// <summary>
        /// Content of the File
        /// </summary>
        public byte[] Content { get; set; }
    }
}
