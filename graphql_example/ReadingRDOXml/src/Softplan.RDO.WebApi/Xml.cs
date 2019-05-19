using System;
using System.Xml.Linq;
using System.Linq;
using System.IO;
using System.IO.Compression;
using System.Xml;
using System.Text;

namespace Softplan.RDO.WebApi
{

  public class Xml
  {
    private Stream RemoveInvalidXmlChars(Stream xml)
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

    private bool IsValiXmlString(string text)
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

    public void ImprimirXml(Stream xml)
    {
      using (StreamReader reader = new StreamReader(xml))
      {
        xml.Position = 0;
        string text = reader.ReadToEnd();
        Console.WriteLine("final do xml!");
      }
    }

    public XDocument Document { get; set; }

    public void LoadXml(Stream xml)
    {
      Document = XDocument.Load(RemoveInvalidXmlChars(xml));
    }

    public Xml(Stream xml)
    {
      Document = XDocument.Load(RemoveInvalidXmlChars(xml));
    }

    public Xml(string path)
    {
      Document = XDocument.Load(path);
    }

    public XElement GetElement(string elementName)
    {
      foreach (XNode node in Document.DescendantNodes())
      {
        if (node is XElement)
        {
          XElement element = (XElement)node;
          if (element.Name.LocalName.Equals(elementName))
            return element;
        }
      }
      return null;
    }

  }

}