using System;
using SkiaSharp;

namespace SamaritanPerlinNoise
{
    public class Program
    {
        public static void Main(string[] args)
        {
            int width = Constants.ImageWidth;
            int height = Constants.ImageHeight;
            double scale = Constants.NoiseScale;

            PerlinNoise noise = new PerlinNoise();

            // Create layers with different amplitudes and frequencies:
            Layer[] layers = new Layer[]
            {
                new Layer(1.0, 0.1),   // Base layer with full amplitude and low frequency.
                new Layer(0.5, 0.5),   // Second layer with half amplitude and moderate frequency.
                new Layer(0.25, 1.0)   // Third layer with quarter amplitude and high frequency.
            };

            using (SKBitmap bitmap = new SKBitmap(width, height))
            {
                // Loop through each pixel:
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        double totalNoiseValue = 0.0;

                        // Sum the noise values from all layers:
                        foreach (var layer in layers)
                        {
                            totalNoiseValue += layer.GetNoiseValue(x * scale, y * scale, 0);
                        }

                        // Map the total noise value to grayscale:
                        int grayscale = (int)((totalNoiseValue + layers.Length) * (255.0 / (2.0 * layers.Length)));

                        SKColor pixelColor = new SKColor((byte)grayscale, (byte)grayscale, (byte)grayscale);
                        bitmap.SetPixel(x, y, pixelColor);
                    }
                }

                using (SKImage newImage = SKImage.FromBitmap(bitmap))
                {
                    using (SKData data = newImage.Encode(SKEncodedImageFormat.Png, 100))
                    {
                        // Save the new image:
                        using (var stream = new System.IO.FileStream("layered_perlin_noise.png", System.IO.FileMode.Create))
                        {
                            data.SaveTo(stream);
                            Console.WriteLine("Layered Perlin noise image saved as layered_perlin_noise.png");
                        }
                    }
                }
            }
        }
    }
}
