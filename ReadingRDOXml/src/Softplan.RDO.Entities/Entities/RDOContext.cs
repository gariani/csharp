using Microsoft.EntityFrameworkCore;

namespace ReadingRDOXml.src.Softplan.RDO.Entities.Entities
{
    public class RDOContext: DbContext
    {
        public DbSeq<EProXmlDto> EProXML {get; set;}

    }
}