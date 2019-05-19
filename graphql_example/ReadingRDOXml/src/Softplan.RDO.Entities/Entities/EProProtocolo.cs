using System;
using System.Collections.Generic;
using Softplan.RDO.Entities.DTO;

namespace Softplan.RDO.Entities.Entities
{
    public class EProProtocolo
    {
        public string CodigoProtocolo { get; set; }
        public int CodigoForoDetino { get; set; }
        public int CodigoTipoProtocolo { get; set; }
        public string NumeroProtocolo  { get; set; }
        public string CodigoProcesso  { get; set; }
        public string NumeroProcesso  { get; set; }
        public int CodigoSituacaoProtocolo  { get; set; }
        public string NumerProcessoReferencia  { get; set; }

        public ICollection<EProProcedContrDTO> ControleProcedimento {get; set;}

    }
}