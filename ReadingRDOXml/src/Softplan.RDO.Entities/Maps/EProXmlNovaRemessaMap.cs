using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReadingRDOXml.src.Softplan.RDO.Entities.DTO;

namespace ReadingRDOXml.src.Softplan.RDO.Entities.Maps
{
  public class EProXmlNovaRemessaMap : IEntityTypeConfiguration<EProXmlNovaRemessa>
  {
    public void Configure(EntityTypeBuilder<EProXmlNovaRemessa> builder)
    {
      builder.HasKey(e => new
      {
        NuXml = e.NuXml
      });

      builder.HasIndex(e => new
      {
        NuControleUnico = e.NuControleUnico
      });

      builder.ToTable("EPROXMLNOVAREMESSA");
    }
  }
}