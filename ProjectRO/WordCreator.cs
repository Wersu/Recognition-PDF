using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Application = Microsoft.Office.Interop.Word.Application;

namespace ProjectRO
{
    internal class WordCreator : IOutputCreator
    {
        OutputType IOutputCreator.GetType()
        {
            return OutputType.DOCX;
        }

        string IOutputCreator.GetFileExtension()
        {
            return ".docx";
        }

        void IOutputCreator.GenerateFile(Tesseract.TesseractEngine engine, System.Collections.Generic.List<RenderPageData> pagesData, string outputPath)
        {
            _Application wordApp = new Application();// Создаем экземпляр приложения Word
            Document doc = wordApp.Documents.Add(Missing.Value, Missing.Value, Missing.Value, Missing.Value);// Добавляем страницу в документ Word
            wordApp.Visible = false;// Скрываем приложение Word


            //проходимся по каждой странице входного файла
            foreach (var pageData in pagesData)
            {
                Paragraph para = doc.Paragraphs.Add(Missing.Value);// создаем общий контейнер, куда будем записывать весь текст

                //проходимся по каждому элементу на странице
                foreach (var blockData in pageData.Blocks)
                {
                    // Если элемент - изображение, сохраняем его во временный файл. Распознаем текст и записываем его в документ Word
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
                                //Microsoft.Office.Interop.Word.Range wordRange = wordApp.ActiveDocument.Shapes[wordApp.ActiveDocument.Shapes.Count-1].TextFrame.ContainingRange;

                                para.Range.InsertParagraphAfter();

                                var wordTable = wordApp.ActiveDocument.Tables.Add(para.Range, data.Table.Rows.Count(), data.Table.Columns.Count());

                                wordTable.Range.Font.Size = 12;
                                wordTable.Borders.InsideLineStyle = WdLineStyle.wdLineStyleSingle;
                                wordTable.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleSingle;

                                wordTable.BottomPadding = 5;
                                wordTable.TopPadding = 5;
                                wordTable.LeftPadding = 5;
                                wordTable.RightPadding = 5;

                                int rowInd = 0; int colInd = 0;

                                foreach (var row in data.Table.Rows)
                                {
                                    foreach (var cell in row)
                                        wordTable.Cell(rowInd+1, ++colInd).Range.Text = (cell.RecognizedText);

                                    colInd = 0;
                                    rowInd++;
                                }
                            }

                            if (!string.IsNullOrEmpty(data.RecognizedText))
                            {
                                para.Range.Text = data.RecognizedText;
                                para.Range.InsertParagraphAfter();
                            }
                        }

                        File.Delete(tempImagePath);
                    }
                    // Если элемент - текст, записываем его в документ Word
                    if (!blockData.IsImage)
                    {
                        para.Range.Text = blockData.Text.ToString();
                        para.Range.InsertParagraphAfter();

                    }
                }
                //если текущая страница не последняя, то после последнего элемента на странице создаем новую страницу
                if (!pagesData.TakeLast(2).Contains(pageData))
                    doc.Words.Last.InsertBreak(Microsoft.Office.Interop.Word.WdBreakType.wdPageBreak);
            }
            // Сохраняем документ Word как файл
            object filename = $"{System.Windows.Forms.Application.StartupPath}//{outputPath}";
            doc.SaveAs(ref filename, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                       Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

            object save_changes = false;

            // Закрываем документ Word и приложение Word
            doc.Close(ref save_changes, Missing.Value, Missing.Value);
            wordApp.Quit(ref save_changes, Missing.Value, Missing.Value);


            // Очищаем ресурсы
            Marshal.FinalReleaseComObject(wordApp);
            Marshal.FinalReleaseComObject(doc);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}
