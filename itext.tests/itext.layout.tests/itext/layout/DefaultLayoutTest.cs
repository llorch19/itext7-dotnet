using System;
using System.IO;
using iText.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Utils;
using iText.Layout.Border;
using iText.Layout.Element;
using iText.Test;
using iText.Test.Attributes;

namespace iText.Layout {
    public class DefaultLayoutTest : ExtendedITextTest {
        public static readonly String sourceFolder = NUnit.Framework.TestContext.CurrentContext.TestDirectory + "/../../resources/itext/layout/DefaultLayoutTest/";

        public static readonly String destinationFolder = NUnit.Framework.TestContext.CurrentContext.TestDirectory
             + "/test/itext/layout/DefaultLayoutTest/";

        [NUnit.Framework.TestFixtureSetUp]
        public static void BeforeClass() {
            CreateDestinationFolder(destinationFolder);
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void MultipleAdditionsOfSameModelElementTest() {
            String outFileName = destinationFolder + "multipleAdditionsOfSameModelElementTest1.pdf";
            String cmpFileName = sourceFolder + "cmp_multipleAdditionsOfSameModelElementTest1.pdf";
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(new FileStream(outFileName, FileMode.Create)));
            Document document = new Document(pdfDocument);
            Paragraph p = new Paragraph("Hello. I am a paragraph. I want you to process me correctly");
            document.Add(p).Add(p).Add(new AreaBreak(PageSize.Default)).Add(p);
            document.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(outFileName, cmpFileName, destinationFolder
                , "diff"));
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void RendererTest01() {
            String outFileName = destinationFolder + "rendererTest01.pdf";
            String cmpFileName = sourceFolder + "cmp_rendererTest01.pdf";
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(new FileStream(outFileName, FileMode.Create)));
            Document document = new Document(pdfDocument);
            String str = "Hello. I am a fairly long paragraph. I really want you to process me correctly. You heard that? Correctly!!! Even if you will have to wrap me.";
            document.Add(new Paragraph(new Text(str).SetBackgroundColor(iText.Kernel.Color.Color.RED)).SetBackgroundColor
                (iText.Kernel.Color.Color.GREEN)).Add(new Paragraph(str)).Add(new AreaBreak(PageSize.Default)).Add(new 
                Paragraph(str));
            document.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(outFileName, cmpFileName, destinationFolder
                , "diff"));
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        [LogMessage(LogMessageConstant.RECTANGLE_HAS_NEGATIVE_OR_ZERO_SIZES)]
        public virtual void EmptyParagraphsTest01() {
            String outFileName = destinationFolder + "emptyParagraphsTest01.pdf";
            String cmpFileName = sourceFolder + "cmp_emptyParagraphsTest01.pdf";
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(new FileStream(outFileName, FileMode.Create)));
            Document document = new Document(pdfDocument);
            // the next 3 lines should not cause any effect
            document.Add(new Paragraph());
            document.Add(new Paragraph().SetBackgroundColor(iText.Kernel.Color.Color.GREEN));
            document.Add(new Paragraph().SetBorder(new SolidBorder(iText.Kernel.Color.Color.BLUE, 3)));
            document.Add(new Paragraph("Hello! I'm the first paragraph added to the document. Am i right?"));
            document.Add(new Paragraph().SetHeight(50));
            document.Add(new Paragraph("Hello! I'm the second paragraph added to the document. Am i right?"));
            document.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(outFileName, cmpFileName, destinationFolder
                , "diff"));
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void EmptyParagraphsTest02() {
            String outFileName = destinationFolder + "emptyParagraphsTest02.pdf";
            String cmpFileName = sourceFolder + "cmp_emptyParagraphsTest02.pdf";
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(new FileStream(outFileName, FileMode.Create)));
            Document document = new Document(pdfDocument);
            document.Add(new Paragraph("Hello, i'm the text of the first paragraph on the first line. Let's break me and meet on the next line!\nSee? I'm on the second line. Now let's create some empty lines,\n for example one\n\nor two\n\n\nor three\n\n\n\nNow let's do something else"
                ));
            document.Add(new Paragraph("\n\n\nLook, i'm the the text of the second paragraph. But before me and the first one there are three empty lines!"
                ));
            document.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(outFileName, cmpFileName, destinationFolder
                , "diff"));
        }
    }
}
