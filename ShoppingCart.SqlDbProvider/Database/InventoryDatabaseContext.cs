using Microsoft.EntityFrameworkCore;
using Shopping.SqlDbProvider.Models.Entities;
using System.Threading.Tasks;

namespace Shopping.SqlDbProvider.Database
{
    /// <summary>
    /// Shopping Database Context
    /// </summary>
    public class InventoryDatabaseContext: DbContext
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryDatabaseContext"/> class.
        /// </summary>
        public InventoryDatabaseContext(DbContextOptions<InventoryDatabaseContext> options) : base(options)
        {
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        /// <summary>
        /// Gets or Sets the Fruits Entity to be stored in the database.
        /// </summary>
        /// <value>
        /// Fruits.
        /// </value>
        public virtual DbSet<FruitEntity> Fruits { get; set; }

        /// <summary>
        /// Saves the Updates into Database.
        /// </summary>
        /// <returns></returns>
        public async Task Save()
        {
            await SaveChangesAsync();
        }

        /// <summary>
        /// On Model Creating 
        /// </summary>
        /// <param name="modelBuilder">Model Builder</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            const string SchemaName = "Inventory";

            modelBuilder.HasDefaultSchema(SchemaName);

            modelBuilder.Entity<FruitEntity>()
               .ToTable("Fruit", SchemaName)
               .HasKey(k => k.Id);
        }

    }
}
