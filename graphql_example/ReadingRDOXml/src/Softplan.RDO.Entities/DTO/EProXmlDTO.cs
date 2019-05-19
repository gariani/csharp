namespace Softplan.RDO.Entities.DTO
{
  public partial class EProXmlDTO
  {

    public string NumeroControleUnico { get; set; }
    public byte[] BlobXMl { get; set; }
    public string NomeXml { get; set; }
    public string DataInsercao { get; set; }
    public string TipoPeticao { get; set; }
    public string Processando { get; set; }
    public string Processado { get; set; }

  }
}