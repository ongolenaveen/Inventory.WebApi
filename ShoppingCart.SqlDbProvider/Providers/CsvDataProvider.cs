using CsvHelper;
using CsvHelper.Configuration;
using Shopping.SqlDbProvider.Interfaces;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Shopping.Extensions;

namespace Shopping.SqlDbProvider.Providers
{
    /// <summary>
    /// This Class has all the functions needed for CSV data provider
    /// </summary>
    /// <typeparam name="T">Type of the object to which CSV row needs to be mapped to</typeparam>
    /// <typeparam name="TMap">Type of the object which maps CSV data to type T</typeparam>
    public class CsvDataProvider<T,TMap> : ICsvDataprovider<T, TMap> where T : new() where TMap:ClassMap
    {
        /// <summary>
        /// Parse the CSV file Contents into List of objects of type T
        /// </summary>
        /// <param name="content">CSV file Content</param>
        /// <returns>List of objects of type T</returns>
        public IEnumerable<T> Parse(byte[] content)
        {
            // Validate the received File Content
            content.ThrowIfIsNullOrEmpty(nameof(content));

            IEnumerable<T> records = default;

            // Parse the Data into a Objects of Type T
            using (var stream = new MemoryStream(content))
            {
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        csv.Configuration.RegisterClassMap<TMap>();
                        records = csv.GetRecords<T>();
                    }
                }
            }
            // Send the response back
            return records;
        }
    }
}
