using SimilarImagesFinder;
using SixLabors.ImageSharp;
using System.Linq;
using Codeuctivity.ImageSharpCompare;

string targetPath = @"M:\Clouds\Google Drive\Google Фото";

List<string> imageFormats = new List<string>()
{
    ".jpg",  
    ".jpeg", 
    ".png", 
    ".gif", 
    ".bmp", 
    ".webp", 
};

Dictionary<string, Image?> images = new();
Dictionary<string, string> coincidences = new();

ParallelOptions options = new ParallelOptions()
{
    MaxDegreeOfParallelism = Environment.ProcessorCount
};

await Parallel.ForEachAsync(Utils.CollectLocalFilesList(targetPath), async (filePath, token) =>
{
    string extension = Path.GetExtension(filePath).ToLower();
    if (!imageFormats.Contains(extension)) return;
    lock (images)
    {
        images.Add(filePath, null);
    }
});

Parallel.For(0, images.Count-2, async (c1, state2) =>
{
    await Utils.CheckImage(images, c1);
    
    Parallel.For(c1+1, images.Count-1, async (c2, state2) =>
    {
        await Utils.CheckImage(images, c2);
        
        bool isEqual = ImageSharpCompare.ImagesAreEqual(images.ElementAt(c1).Value, images.ElementAt(c2).Value);
        if (isEqual)
        {
            coincidences.Add(images.ElementAt(c1).Key, images.ElementAt(c2).Key);
            Console.WriteLine($"{images.ElementAt(c1).Key} === {images.ElementAt(c2).Key}");
        }
    });
});


Console.WriteLine("Hello, World!");
