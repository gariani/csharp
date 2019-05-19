using System;
using System.Xml.Linq;
using System.Linq;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Collections.Generic;
using Newtonsoft.Json;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using Softplan.RDO.WebApi;
using myConsole;
using ConsoleApp1;

namespace ReadingRDOXml
{

  public class MultiPartItem : IEquatable<ByteArrayContent>
  {
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

  class MultiPart
  {
    private List<KeyValuePair<string, ByteArrayContent>> content = new List<KeyValuePair<string, ByteArrayContent>>();

    public void LoadXmlToMultiParte(Stream streamFile, string parse, string contentName)
    {
      var streamContent = new StreamContent(streamFile);
      var imageContent = new ByteArrayContent(streamContent.ReadAsByteArrayAsync().Result);
      imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse(parse);
      content.Add(new KeyValuePair<string, ByteArrayContent>(contentName, imageContent));
    }

    public MultipartFormDataContent GetMultipart()
    {
      MultipartFormDataContent multiPart = new MultipartFormDataContent();
      foreach (KeyValuePair<string, ByteArrayContent> c in content)
      {
        multiPart.Add(c.Value, c.Key);
      }
      content.Clear();
      return multiPart;
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

    static void Main(string[] args)
    {

      var keyAuth = RequestCanonical();

      Console.WriteLine(keyAuth.Auth);
      Console.WriteLine(keyAuth.Data);

      var unzip = new Zip();
      var sqlataBase = new SQLDataBase();
      var lista = sqlataBase.Select();

      foreach (KeyValuePair<string, byte[]> itemZip in lista)
      {
        try
        {
          Console.WriteLine(itemZip.Key);
          var streamZip = new MemoryStream(itemZip.Value);
          var streamXml = unzip.UnZipFile(streamZip);
          var xml = new Xml(streamXml);
          var indicadorUnicoProcedimento = xml.GetElement("indicadorUnicoProcedimento");
          indicadorUnicoProcedimento.Value = LongRandom(99999999999, 9999999999999999, new Random()).ToString();
          var listaDocumentoDigital = xml.GetElement("documentoDigital").Elements();

          int countPdf = 0;
          foreach (XElement listaDocumento in listaDocumentoDigital)
          {
            if (listaDocumento.Name.LocalName.Equals("nomeDocumentoDigital"))
            {
              countPdf++;
              Console.WriteLine(listaDocumento.Value);
              listaDocumento.Value = "teste.pdf";
            }
          }

          xml.Document.Save(streamXml);
          var multiPart = new MultiPart();
          multiPart.LoadXmlToMultiParte(streamXml, "application/xml", "");

          for (int i = 0; i < countPdf; i++)
          {
            Stream streamPdf = System.IO.File.OpenRead(@"D:\Arquivos Integração\Daniel\Estudo\C#\ReadingRDOXml\teste.pdf");
            multiPart.LoadXmlToMultiParte(streamPdf, "multipart/form-data", "");
          }

          Canonical canonical = new Canonical();


          var httpClient = new HttpClient();
          var request = new HttpRequestMessage(HttpMethod.Post, @"http://172.21.8.11/delegaciawsRDO/procedimento/assincrono/ajuizamento");

          httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/form-data; charset=utf-8");
          request.Headers.Add(charset, typeEncode);
          // request.Headers.Add(auth, authorization);
          request.Headers.Add(xSpDate, keyAuth.Data);
          request.Headers.Add(xSpHashAlgorithm, spHash);
          request.Content = multiPart.GetMultipart();
          var save = "data:" + keyAuth.Data + "\n\n" + "authorization:" + canonical.GerenateAuthorization(keyAuth.Auth);
          var response = httpClient.SendAsync(request).Result;


        }
        catch (System.Exception e)
        {
          Console.WriteLine(e.Message);
          continue;
        }
      }
      Console.WriteLine("Quantidade de itens lidos: " + lista.Count());
    }
  }
}
