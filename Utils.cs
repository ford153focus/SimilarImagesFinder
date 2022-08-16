using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace SimilarImagesFinder;

public class Utils
{
    public static List<string> CollectLocalFilesList(string path)
    {
        List<string> folders = new() { path };

        List<string> localFilesList = new();

        while (folders.Count > 0)
        {
            var dirs = Directory.GetDirectories(folders[0]);
            var files = Directory.GetFiles(folders[0]);

            localFilesList.AddRange(files);
            folders.AddRange(dirs);

            folders.RemoveAt(0);
        }

        return localFilesList;
    }

    public static async Task CheckImage(Dictionary<string, Image?> images, int index)
    {
        string imagePath = images.ElementAt(index).Key;
        
        if (images[imagePath] != null) return;
        
        var image = await Image.LoadAsync(imagePath);
        image.Mutate(x => x.Resize(256, 256));
        image.Mutate(x => x.Grayscale());
        
        images[imagePath] = image;
    }
}