using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Softplan.RDO.Entities.DTO;

namespace ReadingRDOXml.src.Softplan.RDO.Entities.Maps
{
  public class EProXmlMap : IEntityTypeConfiguration<EProXml>
  {
    public void Configure(EntityTypeBuilder<EProXml> builder)
    {
      builder.HasKey(e => new
      {
        NuXml = e.NuXml
      });

      builder.ToTable("EPROXML");
    }

  }
}