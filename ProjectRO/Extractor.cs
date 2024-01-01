using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;
using Tesseract;

namespace ProjectRO
{
    public static class Extractor
    {
        //номера для сохранения ячейки таблицы
        private static int save_Col = 0;
        private static int save_Table = 0;
        private static int save_Text = 0;

        public static string GetTextFromImage(this TesseractEngine engine, string imagePath)
        {
            //Загружаем изображение из файла по указанному пути в объект img
            using (var img = Pix.LoadFromFile(imagePath))
            {
                //Обрабатываем изображение с помощью движка распознавания текста
                using (var page = engine.Process(img))
                {
                    //Возвращаем распознанный текст
                    return page.GetText();
                }
            }
        }

        public static IEnumerable<ExtractData> Detect(string path, TesseractEngine engine)
        {
            //загрузка изображения по указанному пути
            var image = new Image<Bgr, byte>(path);
            //конвертируем в черно-белое изображение
            var gray = image.Convert<Gray, byte>();
            //создаем двоичное изображение из серого изображения с использованием пороговых значений 100 и 155
            var binary = gray.ThresholdBinary(new Gray(215), new Gray(255));
            //список обнаруженных ячеек
            var cells = new List<Rectangle>();
            //список контуров
            var contours = new VectorOfVectorOfPoint();
            //находим контуры в картинке
            CvInvoke.FindContours(binary, contours, null, RetrType.List, ChainApproxMethod.ChainApproxSimple);

            //CvInvoke.Imshow("Binary", binary);

            for (int i = 0; i < contours.Size; i++)
            {
                //MessageBox.Show(contours.Size.ToString());
                //находим зону контура и проверяем является ли квадратом или прямоугольниом
                var area = CvInvoke.ContourArea(contours[i]);

                //если в указанных рамках
                if (area > 2000 && area < 400000)
                {
                    //MessageBox.Show("area: "+area.ToString());
                    var rect = CvInvoke.BoundingRectangle(contours[i]);
                    var aspectRatio = (double)rect.Width / rect.Height;
                    //проверяем соотношение сторон (если принадлежит указанным диапозонам, то является квадратом или прямоугольником)
                    //if (aspectRatio > 0.1 && aspectRatio <= 10)
                    //{
                    //    //добавляем в список ячеек
                    //image.Draw(rect, new Bgr(Color.Aqua), 3);
                    cells.Add(rect);
                    //}
                }
            }
            //список обнаруженных элементов на картинке
            List<ExtractData> data = new List<ExtractData>();

            if (!Directory.Exists("ImgTemp"))
                Directory.CreateDirectory("ImgTemp");

            while (true)
            {
                //среди всех распознанных прямоугольников ищем таблицу(находим сумму всех ячеек в высоту. если текущий элемент больше или равен сумме, то это таблица)
                var table = cells.FirstOrDefault(x => x.Width>cells.GroupBy(z => z.Y).First().Sum(z => z.Width)-25);

                if (table==Rectangle.Empty)
                    break;
                //из общей картинки вырезаем фрагмент с таблицей
                var tableImage = binary.GetSubRect(table);
                //список зон, которые входят в зону таблицы
                List<Rectangle> tableCellsRect = cells.Where(cell => table.IntersectsWith(cell)&&table!=cell).ToList();
                //формируем список об ячейках таблицы (изображение и зона)
                List<TableImageData> tableCellsImage = tableCellsRect.Select(rect =>
                {
                    var subImg = binary.GetSubRect(rect);

                    return new TableImageData(subImg, rect);
                }).ToList();
                //формируем информацию о таблице(передаем таблицу и список ячеек)
                TableData createdTable = new TableData(new TableImageData(tableImage, table), tableCellsImage);
                //сохраняем картинку таблицы
                createdTable.Table.Image.Save($"ImgTemp\\table_{save_Table}_full.jpg");
                //перечисляем все ячейки
                foreach (var row in createdTable.Rows)
                    foreach (var cell in row)
                    {
                        //сохраняем картинку текущей ячейки
                        cell.Image.Save($"ImgTemp\\table_{save_Table}_{save_Col}.jpg");
                        //распознаем текст на ячейке
                        cell.RecognizedText = engine.GetTextFromImage($"ImgTemp\\table_{save_Table}_{save_Col++}.jpg");
                    }

                save_Col = 0;
                save_Table++;

                cells.Remove(table);
                tableCellsRect.ForEach(x => cells.Remove(x));
                //добавляем новую созданную информацию о таблице
                data.Add(new ExtractData(createdTable));

                if (cells.Count==0)
                    break;
            }
            //находим все таблицы и сортируем по высоте
            List<TableData> tables = data.Where(x => x.Table!=null).Select(x => x.Table).OrderBy(x => x.Table.Rect.Y).ToList();

            try
            {
                for (int i = 0; i<=tables.Count; i++)
                {
                    //находим границы предыдущей и следующей таблицы
                    Rectangle prevRect = tables.ElementAtOrDefault(i-1)?.Table.Rect??new Rectangle(0, 0, 0, 0);
                    Rectangle nextRect = tables.ElementAtOrDefault(i)?.Table.Rect??new Rectangle(0, 0, 0, 0);
                    //рассчитываем текущую позицию и размер текста на картинке
                    Point location = new Point(0, prevRect.Bottom);
                    Size size = new Size(image.Size.Width, nextRect.Height!=0 ? nextRect.Top-location.Y : image.Size.Height-location.Y);
                    //получаем зону
                    Rectangle textRect = new Rectangle(location, size);
                    //вырезаем по этой зоне картинку
                    var textImage = image.GetSubRect(textRect);

                    string recognizedText = " ";

                    textImage.Save($"ImgTemp\\Text_{save_Text}.jpg");
                    //распознаем текст
                    recognizedText = engine.GetTextFromImage($"ImgTemp\\Text_{save_Text++}.jpg");
                    //записываем в выходные данные
                    data.Add(new ExtractData(recognizedText, textRect));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //рисуем области на картинке
            foreach (var item in data)
                image.Draw(item.Position, new Bgr(item.Table!=null ? Color.Brown : Color.Green), 3);

            //CvInvoke.Imshow("Image", image);


            //сортируем все данные по высоте  возвращаем их
            foreach (var item in data.OrderBy(x => x.Position.Y))
                yield return item;
        }

        public class ExtractData
        {
            public TableData Table { get; private set; } = null;
            public string RecognizedText { get; private set; } = " ";
            public Rectangle Position { get; private set; }

            public ExtractData(TableData table)
            {
                this.Table = table;
                Position = table.Table.Rect;
            }

            public ExtractData(string text, Rectangle pos)
            {
                this.RecognizedText = text;
                this.Position = pos;
            }
        }

        public class TableData
        {
            public TableImageData Table { get; private set; }
            public List<TableImageData> Cells { get; private set; }

            public IEnumerable<IGrouping<int, TableImageData>> Rows => Cells.OrderBy(x => x.Rect.X).GroupBy(x => x.Rect.Y).OrderBy(x => x.Key);
            public IEnumerable<IGrouping<int, TableImageData>> Columns => Cells.OrderBy(x => x.Rect.Y).GroupBy(x => x.Rect.X).OrderBy(x => x.Key);

            public TableData(TableImageData table, List<TableImageData> cells)
            {
                Table=table;
                Cells=cells;
            }
        }

        public class TableImageData
        {
            public Image<Gray, byte> Image { get; private set; }
            public Rectangle Rect { get; private set; }
            public string RecognizedText = " ";

            public TableImageData(Image<Gray, byte> image, Rectangle rect)
            {
                Image=image;
                Rect=rect;
            }
        }
    }
}
