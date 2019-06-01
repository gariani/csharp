using System;
using System.Xml.Linq;
using System.Xml;
using System.Linq;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using Softplan.RDO.WebApi;
using myConsole;
using ConsoleApp1;
using System.ComponentModel;
using ReadingRDOXml.src.Softplan.RDO.Entities.Entities;
using Softplan.RDO.Entities.DTO;

namespace ReadingRDOXml
{

  public class MultiPartItem : IEquatable<ByteArrayContent>
  {
    public MultiPartItem(int indice, string xml, string pDF1, string pDF2)
    {
      this.Indice = indice;
      this.Xml = xml;
      this.PDF1 = pDF1;
      this.PDF2 = pDF2;

    }
    public int Indice { get; set; }
    public string Xml { get; set; }
    public string PDF1 { get; set; }
    public string PDF2 { get; set; }

    public bool Equals(ByteArrayContent other)
    {
      if (other == null) return false;
      ByteArrayContent objAsSeqFile = other as ByteArrayContent;
      if (objAsSeqFile == null) return false;
      else return Equals(objAsSeqFile);
    }
  }

  class MultiPart : IDisposable
  {
    private bool disposed = false;
    private IntPtr handle;
    private Component component = new Component();
    private MultipartFormDataContent multiPart = new MultipartFormDataContent();

    public void CopyStream(Stream input, string filePath)
    {
      using (System.IO.FileStream output = new System.IO.FileStream(filePath, FileMode.Create))
      {
        input.Position = 0;
        output.Position = 0;
        input.CopyTo(output);
      }
    }

    public void LoadXmlToMultiParte(string parse, string contentName, string filePath)
    {
      using (FileStream newFile = File.OpenRead(filePath))
      {
        LoadToMultiParte(parse, contentName, filePath);
      }
    }

    public void LoadToMultiParte(string parse, string contentName, string filePath, int loop)
    {
      for (int i = 1; i <= loop; i++)
      {
        var newFile = i.ToString() + filePath;
        if (!File.Exists(newFile)){
          File.Copy(filePath, newFile);
          LoadToMultiParte(parse, contentName, newFile);
        }
        else {          
          LoadToMultiParte(parse, contentName, newFile);
        }
      }
    }

    public void LoadToMultiParte(string parse, string contentName, string filePath)
    {
      using (FileStream fsXML = File.OpenRead(filePath))
      using (var streamContentXML = new StreamContent(fsXML))
      {
        ByteArrayContent imageContent = new ByteArrayContent(streamContentXML.ReadAsByteArrayAsync().Result);
        imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse(parse);
        multiPart.Add(imageContent, contentName, filePath);
      }
    }

    public MultipartFormDataContent GetMultipart()
    {
      return multiPart;
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!disposed)
      {
        if (disposing)
        {
          component.Dispose();
        }

        CloseHandle(handle);
        handle = IntPtr.Zero;

        disposed = true;

      }
    }

    [System.Runtime.InteropServices.DllImport("Kernel32")]
    private extern static Boolean CloseHandle(IntPtr handle);

    ~MultiPart()
    {
      Dispose(false);
    }

  }


  class Program
  {
    private const string separator = ":";
    private const string newLine = "\n";
    private const string canonical = "SP";
    private const string xSpDate = "x-sp-date";
    private const string xSpHashAlgorithm = "x-sp-hashAlgorithm";
    private const string spAccessKeyID = "SAJRDO";
    private const string spHash = "SHA256withRSA";
    private const string typeEncode = "utf-8";
    private const string charset = "charset";
    private const string auth = "Authorization";

    public static long LongRandom(long min, long max, Random rand)
    {
      long result = rand.Next((Int32)(min >> 32), (Int32)(max >> 32));
      result = (result << 32);
      result = result | (long)rand.Next((Int32)min, (Int32)max);
      return result;
    }


    public static CanonicalItem RequestCanonical()
    {
      CanonicalItem canonicalItem;
      WebRequest request = WebRequest.Create("http://172.21.8.11:5000/api/canonical");
      request.Credentials = CredentialCache.DefaultCredentials;
      WebResponse response = request.GetResponse();
      Console.WriteLine(((HttpWebResponse)response).StatusDescription);

      using (Stream dataStream = response.GetResponseStream())
      {
        StreamReader reader = new StreamReader(dataStream);
        string responseFromServer = reader.ReadToEnd();
        Console.WriteLine(responseFromServer);
        canonicalItem = JsonConvert.DeserializeObject<CanonicalItem>(responseFromServer);

      }
      response.Close();
      return canonicalItem;
    }
    public static byte[] ConverteStreamToByteArray(Stream stream)
    {
      byte[] byteArray = new byte[16 * 1024];
      using (MemoryStream mStream = new MemoryStream())
      {
        int bit;
        while ((bit = stream.Read(byteArray, 0, byteArray.Length)) > 0)
        {
          mStream.Write(byteArray, 0, bit);
        }
        return mStream.ToArray();
      }
    }

    private static bool IsValiXmlString(string text)
    {
      try
      {
        XmlConvert.VerifyXmlChars(text);
        return true;
      }
      catch
      {
        return false;
      }
    }
    private static Stream RemoveInvalidXmlChars(Stream xml)
    {
      StreamReader reader = new StreamReader(xml);
      string text = reader.ReadToEnd();
      if (IsValiXmlString(text))
      {
        Console.WriteLine("XML sem erros");
        byte[] byteArray = Encoding.ASCII.GetBytes(text);
        return new MemoryStream(byteArray);
      }
      else
      {
        Console.WriteLine("XML com erros");
        var validXMLChars = text.Where(ch => XmlConvert.IsXmlChar(ch)).ToArray();
        var newtext = new string(validXMLChars);
        byte[] byteArray = Encoding.ASCII.GetBytes(newtext);
        return new MemoryStream(byteArray);
      }

    }

    static void Main(string[] args)
    {

      using (var db = new RDOContext())
      {
        var registros = db.Set<EProXml>().Where(x => x.TipoPeticao == "ajuizamento");

        foreach (EProXml registro in registros)
        {

          var unzip = new Zip();

          try
          {

            using (var streamZip = new MemoryStream(registro.BlXml))
            using (var streamXml = unzip.UnZipFile(streamZip))
            {
              XDocument Document = XDocument.Load(RemoveInvalidXmlChars(streamXml));
              var child = Document.Descendants("documentoDigital").Select(x => x.Value).ToList();

              foreach (string i in child)
              {
                Console.WriteLine(i);
              }
            }

            using (var streamZip = new MemoryStream(registro.BlXml))
            using (var streamXml = unzip.UnZipFile(streamZip))
            using (Xml xml = new Xml(streamXml))
            {


              Console.WriteLine(registro.NmXml);

              string[] selectableInts = new string[2] { "66", "37", };
              Random rand = new Random();
              string randomValue = selectableInts[rand.Next(0, selectableInts.Length)];

              xml.SetElementValue("indicadorUnicoProcedimento", LongRandom(99999999999, 9999999999999999, new Random()).ToString());
              xml.SetElementValue("codigoForo", randomValue);
              xml.SetElementValue("numeroDocumentoOrigem", LongRandom(99999999, 99999999999, new Random()).ToString());
              xml.SetElementValue("codigoCompetencia", 9.ToString());
              xml.SetElementValue("codigoClasse", 279.ToString());
              xml.SetElementValue("nomeDocumentoDigital", "teste.pdf", true);
              xml.SetElementValue("codigoTipoDocumentoOrigem", 1.ToString());

              using (MemoryStream xmlStream = new MemoryStream())
              using (var multiPart = new MultiPart())
              using (var httpClient = new HttpClient())
              using (var request = new HttpRequestMessage(HttpMethod.Post, @"http://172.21.8.11/delegaciawsRDO/procedimento/assincrono/ajuizamento"))
              {

                xml.Document.Save(xmlStream);
                xml.Document.Save(@"envio.xml");

                multiPart.LoadXmlToMultiParte("application/xml", "procedimento", @"xml\envio.xml");

                XmlDocument doc = new XmlDocument();
                doc.Load(@"xml\envio.xml");
                XmlNodeList xmlNodeList = doc.GetElementsByTagName("documentoDigital");
                
                multiPart.LoadToMultiParte("multipart/form-data", "documentos", @"pdf\teste.pdf", xmlNodeList.Count);
    

                var keyAuth = RequestCanonical();

                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/form-data; charset=utf-8");
                request.Headers.Add(charset, typeEncode);
                request.Headers.Add(auth, keyAuth.Auth);
                request.Headers.Add(xSpDate, keyAuth.Data);
                request.Headers.Add(xSpHashAlgorithm, spHash);
                request.Content = multiPart.GetMultipart();
                var response = httpClient.SendAsync(request).Result;
                Thread.Sleep(3000);
              }
            }
            // File.Delete("envio.xml");
          }
          catch (System.Exception e)
          {
            Console.WriteLine(e.Message);
            continue;
          }
        }
        Console.WriteLine("Quantidade de itens lidos: " + registros.Count());
      }
    }
  }
}
