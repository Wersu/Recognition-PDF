using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace ProjectRO
{
    internal interface IOutputCreator
    {
        OutputType GetType();

        string GetFileExtension();

        void GenerateFile(TesseractEngine engine, List<RenderPageData> pagesData, string outputPath);
    }

    public enum OutputType
    {
        PDF,
        DOCX,
    }
}
