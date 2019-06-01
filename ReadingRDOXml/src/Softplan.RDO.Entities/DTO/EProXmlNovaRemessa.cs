using System;
using ReadingRDOXml.src.Softplan.RDO.Entities.Keys;

namespace ReadingRDOXml.src.Softplan.RDO.Entities.DTO
{
  public class EProXmlNovaRemessa: EProXmlNovaRemessaKey
  {
    public string NuControleUnico { get; set; }
    public Byte[] BlXml { get; set; }
    public string NmXml { get; set; }
    public string DtInsercao { get; set; }
    public string TipoPeticao { get; set; }
    public string NuControleUnicoAnterior { get; set; }

  }
}