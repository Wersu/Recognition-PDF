using System.Diagnostics;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Tesseract;
using System.Reflection;
using Microsoft.Office.Interop.Word;
using System.Runtime.InteropServices;
using WordDocument = Microsoft.Office.Interop.Word.Document;
using WordApplication = Microsoft.Office.Interop.Word.Application;
using WordParagraph = Microsoft.Office.Interop.Word.Paragraph;
using Org.BouncyCastle.X509;

namespace ProjectRO
{
    public partial class Form1 : Form
    {
        private readonly IReadOnlyList<IOutputCreator> outputCreators = new List<IOutputCreator>() {
            new PDFCreator(),
            new WordCreator(),
        };

        private OutputType outputType = OutputType.PDF;

        private IOutputCreator ActualCreator => outputCreators.FirstOrDefault(x => x.GetType()==outputType);

        private string inputFilePath = string.Empty; //���� �� �������� �����, ���������� �������������
        private string outputFilePath = "NewFileh"; //���� �� ��������� �����
        private string actualOutputFilePath => outputFilePath+ActualCreator.GetFileExtension();

        private string lang = "eng";//���� ��� �������������, �� ��������� ����� ����������

        private Stopwatch recognizeSW = new Stopwatch();

        public Form1()
        {
            InitializeComponent();
        }

        private void Recognize()
        {
            //���������, ������ �� �������� ����     
            if (String.IsNullOrEmpty(inputFilePath) || String.IsNullOrWhiteSpace(inputFilePath))
                throw new Exception("No file selected");

            //���������, ������ �� ��� ��������� �����
            if (outputTypeSelector.SelectedItem == null)
                throw new Exception("No type selected");

            try
            {
                OnStartRecognize();

                if (File.Exists(actualOutputFilePath))
                    File.Delete(actualOutputFilePath);

                //������� TesseractEngine, ������� ����� ������������ �����. �������� ���� �� ����� � ���� �������������
                using (var engine = new TesseractEngine(@"traineddata", lang, EngineMode.Default))
                {
                    //������ �������� PDF-����
                    using (var inputPdf = new PdfReader(inputFilePath))
                    {
                        PdfReaderContentParser parser = new PdfReaderContentParser(inputPdf);

                        List<RenderPageData> pagesData = new List<RenderPageData>();

                        for (int pageNumber = 1; pageNumber<=inputPdf.NumberOfPages; pageNumber++)
                        {
                            BlockRenderListener listener;
                            parser.ProcessContent(pageNumber, (listener = new BlockRenderListener()));

                            pagesData.Add(new RenderPageData(pageNumber-1, listener.OrderedElements));
                        }

                        ActualCreator.GenerateFile(engine, pagesData, actualOutputFilePath);
                    }

                    //������ Open ���������� ��������, ����� ����
                    buttonOpen.BackColor = Color.LightGreen;
                    buttonOpen.Enabled = true;
                }

                OnEndRecognize();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnStartRecognize()
        {
            Cursor.Current = Cursors.WaitCursor;
            recognizeSW = new Stopwatch();
            richTextBoxOutput.Text += "������������� ��������...\n";

            recognizeSW.Start();
        }

        private void OnEndRecognize()
        {
            recognizeSW.Stop();

            richTextBoxOutput.Text += $"������������� �����������. ����� �������������: {recognizeSW.Elapsed.ToString(@"hh\:mm\:ss")}\n";
            richTextBoxOutput.Text += "�������� ����: \n";
            richTextBoxOutput.Text += actualOutputFilePath+"\n";

            Cursor.Current = Cursors.Default;
        }

        private void SelectOpenPath()
        {
            DialogResult res = openFileDialog1.ShowDialog();//������������ �������� ����

            if (res == DialogResult.OK)//���� � ����� ����� ��
            {
                inputFilePath = openFileDialog1.FileName;//���������� ��� �������� �����

                richTextBoxInput.Text = openFileDialog1.SafeFileName;//� ����� ������� (richTextBox) ���������� ��� �������� �����

                //������ Recognize ���������� �������� � ������ ���� ����
                buttonRecognize.BackColor = SystemColors.ActiveCaption;
                buttonRecognize.Enabled = true;
            }
            else
            {
                MessageBox.Show("No file selected", "You need to select a file", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void OpenRecognizedFile()
        {
            //���� ����� �� ����������, �� ������� �� ������
            if (!File.Exists(actualOutputFilePath))
                return;

            //������� �������
            var p = new Process();
            //��������� ������ �� ������ ��������� �� ���������� ����
            p.StartInfo = new ProcessStartInfo(actualOutputFilePath)
            {
                //������� ��������� ������ ����� ��������� ������ (PowerShell)
                UseShellExecute = true
            };
            //�������� ��� �������
            p.Start();
        }

        private void SelectOutputPath()
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    outputFilePath = fbd.SelectedPath + "\\NewFile";
                    MessageBox.Show(actualOutputFilePath);
                }
            }
        }

        private void OutputToolStrip_Click(object sender, EventArgs e)
        {
            SelectOutputPath();
        }

        private void OutputTypeSelector_Select(object sender, EventArgs e)
        {
            if (outputTypeSelector.SelectedIndex == 0)
                outputType = OutputType.PDF;
            if (outputTypeSelector.SelectedIndex == 1)
                outputType = OutputType.DOCX;
        }

        private void ButtonOpen_Click(object sender, EventArgs e)
        {
            OpenRecognizedFile();
        }

        private void ButtonRecognize_Click(object sender, EventArgs e)
        {
            Recognize();
        }

        private void OpenFileToolStrip_Click(object sender, EventArgs e)
        {
            SelectOpenPath();
        }
    }
}