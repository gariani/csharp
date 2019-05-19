using System;
using System.Text;

namespace myConsole
{

  public class CanonicalItem
  {
    public string Data { get; set; }
    public string Auth { get; set; }
  }

  class Canonical
  {
    private const string separator = ":";
    private const string newLine = "\n";
    private const string canonical = "SP";
    private const string xSpDate = "x-sp-date";
    private const string xSpHashAlgorithm = "x-sp-hashAlgorithm";
    private const string spAccessKeyID = "SAJRDO";
    private const string spHash = "SHA256withRSA";
    private const string typeEncode = "utf-8";
    private string spDate;

    public string TypeEncode
    {
      get { return typeEncode; }
      private set { }
    }

    public string SpHash
    {
      get { return spHash; }
      private set { }
    }

    private string GetSpDate(string formatedDate)
    {
      return xSpDate + separator + formatedDate;
    }

    private string GetSpHash(string hash)
    {
      return xSpHashAlgorithm + separator + hash;
    }

    public string GerenateAuthorization(string signBase64)
    {
      return canonical + " " + spAccessKeyID + separator + signBase64;
    }

  }
}
