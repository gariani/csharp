using System;
using Softplan.RDO.Entities.Keys;

namespace Softplan.RDO.Entities.DTO
{
  public class EProXml : EProXmlKey
  {
    public string NuControleUnico { get; set; }
    public Byte[] BlXml { get; set; }
    public string NmXml { get; set; }
    public string TipoPeticao { get; set; }
  }

}