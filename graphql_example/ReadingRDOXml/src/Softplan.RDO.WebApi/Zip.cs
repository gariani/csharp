using System;
using System.IO;
using System.IO.Compression;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace Softplan.RDO.WebApi
{
  public class Zip
  {

    public MemoryStream UnZipFile(Stream compressed)
    {
      var ms = new MemoryStream();
      using (var zip = new ZipArchive(compressed, ZipArchiveMode.Read))
      {
        foreach (var entry in zip.Entries)
        {
          using (var stream = entry.Open())
          {
            stream.CopyTo(ms);
            ms.Position = 0;
          }
        }
      }
      return ms;
    }

    public MemoryStream Compress(MemoryStream ms)
    {
      byte[] buffer = new byte[ms.Length];
      GZipStream compressedzipStream = new GZipStream(ms, CompressionMode.Compress, true);
      compressedzipStream.Write(buffer, 0, buffer.Length);
      compressedzipStream.Close();
      MemoryStream ms1 = new MemoryStream(buffer);
      return ms1;
    }

  }

}