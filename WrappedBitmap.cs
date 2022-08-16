using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace SimilarImagesFinder;

public class WrappedBitmap
{
    private string filePath;
    private Image image;

    public WrappedBitmap(string filePath)
    {
        this.filePath = filePath;
        this.image = Image.Load(filePath);
        image.Mutate(x => x.Resize(256, 256));
        image.Mutate(x => x.Grayscale());
    }
}