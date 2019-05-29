using Microsoft.EntityFrameworkCore;
using Softplan.RDO.Entities.DTO;
using ReadingRDOXml.src.Softplan.RDO.Entities.Maps;

namespace ReadingRDOXml.src.Softplan.RDO.Entities.Entities
{
  public class RDOContext : DbContext
  {

    // public RDOContext(DbContextOptions<RDOContext> options) : base(options) { }

    public DbSet<EProXml> EProXMLContext { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlServer(@"Server=172.21.8.81\isaj01;Database=NETTINT;User ID=saj;Password=agesune1");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.ApplyConfiguration(new EProXmlMap());
    }

  }
}