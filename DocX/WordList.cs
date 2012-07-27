using System.Xml.Linq;

namespace Novacode
{
  public class WordList : InsertBeforeOrAfter
  {
    internal WordList(DocX document, XElement xml)
      : base(document, xml)
    {
      Xml = xml;
    }
  }


}