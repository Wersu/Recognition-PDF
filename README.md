# Recognition-PDF
Recognition-PDF - система для распознавания текста и таблиц в файле формата PDF. 

Инструменты: Tesseract, iTextSharp, Microsoft.Office.Interop.Word и Emgu.CV.

Входные данные – файл формата PDF с текстом и изображениями
Выходные данные - файл формата PDF, либо DOCX, содержащий исходный текст и распознанные с изображений текст и таблицы.

-----
##  Алгоритм работы ПО
В программу загружается файл pdf формата, который может содержать изображение с текстом и сам текст.
Далее обрабатываются элементы каждой страницы и опряделяется их тип (текст, изображение) 
Исходный текст записывается в выходной файл, а изображения отправляются на распознавание.
Пример исходного изображения

![startImg](https://github.com/Wersu/Recognition-PDF/blob/master/starting.png)

Бинаризация

![binaryImg](https://github.com/Wersu/Recognition-PDF/blob/master/binary.png)

Сегментация

![binaryImg](https://github.com/Wersu/Recognition-PDF/blob/master/segmentation.png)

