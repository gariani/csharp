using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Softplan.RDO.Entities.DTO;

namespace ReadingRDOXml.src.Softplan.RDO.Entities.Entities
{
  public partial class RdoDbContext : DbContext
  {
    public virtual DbSet<EproProtocoloDTO> EproProtocolo { get; set; }
    public virtual DbSet<EProProcedContrDTO> EProProcedContr { get; set; }
    public virtual DbSet<EProXmlDTO> EProXml { get; set; }
  }
}