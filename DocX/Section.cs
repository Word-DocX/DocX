using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Xml.Linq;

namespace Novacode
{
    /// <summary>
    /// Please read the specifications at
    /// http://msdn.microsoft.com/en-us/library/documentformat.openxml.wordprocessing.sectionproperties.aspx
    /// </summary>
    public class Section : Container
    {

        public SectionBreakType SectionBreakType;

        internal Section(DocX document, XElement xml)
            : base(document, xml)
        {
        }

        // Section doesn't contain any paragraphs in the xml, so we override the base property
        public override List<Paragraph> Paragraphs
        {
            get
            {
                if (Xml.Parent == null)
                {
                    throw new ApplicationException("Sections must be either be under a paragraph or the body!");
                }

                var output = new List<Paragraph>();
                var index = 0; // Is required by the recursion
                // Based on the specifications:
                //   For all sections except the final section, the sectPr element is stored as a child element 
                //   of the last paragraph in the section. For the final section, this information is stored as 
                //   the last child element of the body element
                // This

                var elementsBeforeSection = GetElementsBeforeSection();
                // Get last previous section
                var lastSectionBefore = elementsBeforeSection.InDocumentOrder().LastOrDefault(el => el.Descendants().Any(s => s.Name.LocalName == "sectPr"));
                if (lastSectionBefore != null) // We have to exclude all the previous elements
                {
                    var elementComparer = new XElementEqualityComparer();
                    elementsBeforeSection = elementsBeforeSection.Except<XElement>(lastSectionBefore.ElementsBeforeSelf(), elementComparer);
                    // And the previous section element
                    elementsBeforeSection = elementsBeforeSection.Except<XElement>(new XElement[] { lastSectionBefore }, elementComparer);
                }
                foreach (var xElement in elementsBeforeSection)
                {
                    GetParagraphsRecursive(xElement, ref index, ref output);
                }
                return output;
            }
        }

        private IEnumerable<XElement> GetElementsBeforeSection()
        {
            IEnumerable<XElement> elementsBeforeSection = null;

            // If this is a section somewhere inside the document
            if (Xml.Parent.Name.LocalName == "pPr") // Get the nodes on the same level as the father
            {
                elementsBeforeSection = Xml.Parent.Parent.ElementsBeforeSelf();
            }
            else if (Xml.Parent.Name.LocalName == "body") // This is the final section so we get all children in same level
            {
                elementsBeforeSection = Xml.ElementsBeforeSelf();
            }
            else
            {
                throw new ApplicationException("Sections should be located inside a pPr or body");
            }
            return elementsBeforeSection;
        }

        /// <summary>
        /// Clear all paragraphs in section
        /// </summary>
        /// <param name="trackChanges">Keep track of changes</param>
        public void Clear(bool trackChanges = false)
        {
            var paragraphs = Paragraphs;
            foreach (var paragraph in paragraphs)
            {
                paragraph.Remove(trackChanges);
            }
        }


        // Sections don't have paragraphs.
        public override Paragraph InsertParagraph(string text, bool trackChanges, Formatting formatting)
        {
            XElement newParagraph = new XElement
           (
               XName.Get("p", DocX.w.NamespaceName), new XElement(XName.Get("pPr", DocX.w.NamespaceName)), HelperFunctions.FormatInput(text, formatting.Xml)
           );

            if (trackChanges)
                newParagraph = HelperFunctions.CreateEdit(EditType.ins, DateTime.Now, newParagraph);

            //So we have to add it just before the section brake
            GetElementsBeforeSection().Last().AddAfterSelf(newParagraph);

            var paragraphAdded = Paragraphs.Last();

            return paragraphAdded;
        }
    }

    public class XElementEqualityComparer : IEqualityComparer<XElement>
    {

        public bool Equals(XElement x, XElement y)
        {
            return XElement.DeepEquals(x, y);
        }

        public int GetHashCode(XElement obj)
        {
            return obj.GetHashCode();
        }
    }
}