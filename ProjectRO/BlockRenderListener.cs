using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Text;

internal class RenderPageData
{
    public readonly int Index = 0;

    public IReadOnlyList<RenderBlockData> Blocks;

    public RenderPageData(int index, List<RenderBlockData> blocks)
    {
        this.Index = index;
        this.Blocks = blocks;
    }
}

internal class BlockRenderListener : IRenderListener
{
    private List<RenderBlockData> elements = new List<RenderBlockData>();

    public List<RenderBlockData> OrderedElements => elements.OrderByDescending(x => x.Y).ToList();

    public void BeginTextBlock() { }
    public void EndTextBlock() { }
    public void RenderImage(ImageRenderInfo renderInfo)
    {
        PdfImageObject image = renderInfo.GetImage();
        PdfName filter = (PdfName)image.Get(PdfName.FILTER);

        float x = renderInfo.GetImageCTM()[Matrix.I31];
        float y = renderInfo.GetImageCTM()[Matrix.I32] - renderInfo.GetImageCTM()[Matrix.I12];

        if (filter != null)
        {
            System.Drawing.Image drawingImage = image.GetDrawingImage();

            elements.Add(new RenderBlockData(x, y, drawingImage));
        }
    }
    public void RenderText(TextRenderInfo renderInfo)
    {
        string text = renderInfo.GetText();

        float x = renderInfo.GetBaseline().GetBoundingRectange().X;
        float y = renderInfo.GetBaseline().GetBoundingRectange().Y;

        RenderBlockData findedElement = elements.FirstOrDefault(x => !x.IsImage&&MathF.Abs(x.Y-y)<5);

        if (findedElement==null)
            elements.Add(new RenderBlockData(x, y, text));
        else
            findedElement.Text.Append(text);
    }
}

internal class RenderBlockData
{
    public readonly float X, Y;

    public bool IsImage => Img!=null;

    public readonly StringBuilder Text;
    public readonly Image Img = null;

    public RenderBlockData(float x, float y, string text)
    {
        this.X = x;
        this.Y = y;

        this.Text = new StringBuilder(text);
    }

    public RenderBlockData(float x, float y, Image img)
    {
        this.X = x;
        this.Y = y;

        this.Img = img;
    }
}
