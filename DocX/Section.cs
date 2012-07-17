using System.Collections.Generic;
using System.IO.Packaging;
using System.Xml.Linq;

namespace Novacode
{
  public class Section : Container
  {

    public SectionBreakType SectionBreakType;

    internal Section(DocX document, XElement xml) : base(document, xml)
    {
    }

    public override List<Paragraph> Paragraphs
    {
      get
      {
        List<Paragraph> l = base.Paragraphs;
     //   l.ForEach(x => x.mainPart = mainPart);
        return l;
      }
    }

    public override List<Table> Tables
    {
      get
      {
        List<Table> l = base.Tables;
       // l.ForEach(x => x.mainPart = mainPart);
        return l;
      }
    }


    public override Paragraph InsertParagraph()
    {
      Paragraph p = base.InsertParagraph();
//      p.PackagePart = mainPart;
      return p;
    }

    public override Paragraph InsertParagraph(int index, string text, bool trackChanges)
    {
      Paragraph p = base.InsertParagraph(index, text, trackChanges);
  //    p.PackagePart = mainPart;
      return p;
    }

    public override Paragraph InsertParagraph(Paragraph p)
    {
    //  p.PackagePart = mainPart;
      return base.InsertParagraph(p);
    }

    public override Paragraph InsertParagraph(int index, Paragraph p)
    {
      //p.PackagePart = mainPart;
      return base.InsertParagraph(index, p);
    }

    public override Paragraph InsertParagraph(int index, string text, bool trackChanges, Formatting formatting)
    {
      Paragraph p = base.InsertParagraph(index, text, trackChanges, formatting);
      //p.PackagePart = mainPart;
      return p;
    }

    public override Paragraph InsertParagraph(string text)
    {
      Paragraph p = base.InsertParagraph(text);
      //p.PackagePart = mainPart;
      return p;
    }

    public override Paragraph InsertParagraph(string text, bool trackChanges)
    {
      Paragraph p = base.InsertParagraph(text, trackChanges);
      //p.PackagePart = mainPart;
      return p;
    }

    public override Paragraph InsertParagraph(string text, bool trackChanges, Formatting formatting)
    {
      Paragraph p = base.InsertParagraph(text, trackChanges, formatting);
      //p.PackagePart = mainPart;

      return p;
    }
  }
}