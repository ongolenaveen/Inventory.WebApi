using CsvHelper.Configuration;
using System.Collections.Generic;

namespace Inventory.SqlDbProvider.Interfaces
{
    /// <summary>
    /// This Interface has all the functions needed For CSV data provider
    /// </summary>
    /// <typeparam name="T">Type of the object to which CSV row needs to be mapped to</typeparam>
    /// <typeparam name="TMap">Type of the object which maps CSV data to Destination object</typeparam>
    public interface ICsvDataprovider<T, TMap> where T:new() where TMap : ClassMap
    {
        /// <summary>
        /// Parse the CSV file Contents into List of objects of type T
        /// </summary>
        /// <param name="content">CSV file Content</param>
        /// <returns>List of objects of type T</returns>
        IEnumerable<T> Parse(byte[] content);
    }
}
