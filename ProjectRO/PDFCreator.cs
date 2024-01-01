using iTextSharp.text.pdf.parser;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Tesseract;

namespace ProjectRO
{
    internal class PDFCreator : IOutputCreator
    {
        OutputType IOutputCreator.GetType()
        {
            return OutputType.PDF;
        }

        string IOutputCreator.GetFileExtension()
        {
            return ".pdf";
        }

        public void GenerateFile(TesseractEngine engine, List<RenderPageData> pages, string outputPath)
        {
            using (var outputPdf = new Document(PageSize.A4))
            {
                FileStream fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None);

                // создаем PdfWriter для записи в выходной PDF
                using (var outputStream = PdfWriter.GetInstance(outputPdf, fs))
                {
                    outputPdf.Open();//открываем выходной файл
                    outputStream.CloseStream = false;//говорим не закрывать поток

                    //проходимся по каждой странице входного файла
                    foreach (var page in pages)
                    {
                        // Проходимся по каждому элементу на странице
                        foreach (var blockData in page.Blocks)
                        {
                            // Если элемент - изображение, сохраняем его во временный файл. Распознаем текст и записываем его в выходной файл
                            if (blockData.IsImage)
                            {
                                var tempImagePath = System.IO.Path.GetTempFileName();

                                using (var imageStream = new FileStream(tempImagePath, FileMode.Create))
                                {
                                    blockData.Img.Save(imageStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                                }

                                foreach (var data in Extractor.Detect(tempImagePath, engine))
                                {
                                    if (data.Table!=null&&data.Table.Columns.Count()!=0)
                                    {
                                        PdfPTable pdfTable = new PdfPTable(data.Table.Columns.Count());

                                        foreach (var row in data.Table.Rows)
                                            foreach (var cell in row)
                                                pdfTable.AddCell(cell.RecognizedText);

                                        outputPdf.Add(new iTextSharp.text.Paragraph("\n"));

                                        outputPdf.Add(pdfTable);

                                        outputPdf.Add(new iTextSharp.text.Paragraph("\n"));
                                    }

                                    if (!string.IsNullOrEmpty(data.RecognizedText))
                                    {
                                        var paragraph = new iTextSharp.text.Paragraph(data.RecognizedText);

                                        outputPdf.Add(paragraph);
                                    }
                                }

                                File.Delete(tempImagePath);
                            }
                            // Если элемент - текст, записываем его в выходной файл
                            if (!blockData.IsImage)
                            {
                                var textChunk = new iTextSharp.text.Chunk(blockData.Text.ToString());
                                var textPhrase = new Phrase(textChunk);
                                var paragraph = new iTextSharp.text.Paragraph(textPhrase);

                                outputPdf.Add(paragraph);
                            }
                        }

                        outputPdf.NewPage();
                    }

                    outputPdf.Close();
                    outputStream.Close();
                }

                fs.Dispose();
            }
        }
    }
}
