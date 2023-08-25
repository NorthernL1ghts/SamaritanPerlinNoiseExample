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
                new Layer(20.0),    // Base layer with reduced amplitude and low frequency.
                new Layer(10.0),    // Second layer with even lower amplitude and moderate frequency.
                new Layer(4.0)      // Third layer with lowest amplitude and high frequency.
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

                        // Map the total noise value to the range of -1 to 1:
                        double noiseInRange = totalNoiseValue / layers.Length;

                        // Remap the value to grayscale range 0 to 255:
                        int grayscale = (int)((noiseInRange + 1.0) * 127.5);

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

                            // Call the function to calculate and output the smallest and greatest noise values:
                            CalculateMinMaxNoiseValues(bitmap);

                            // Pause the console until the user presses a key:
                            Console.WriteLine("Press any key to exit...");
                            Console.ReadKey();
                        }
                    }
                }
            }
        }

        // Function to calculate the smallest and greatest noise values in the image:
        private static void CalculateMinMaxNoiseValues(SKBitmap bitmap)
        {
            double minNoise = double.MaxValue;
            double maxNoise = double.MinValue;

            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    SKColor pixelColor = bitmap.GetPixel(x, y);
                    double noiseValue = (pixelColor.Red + pixelColor.Green + pixelColor.Blue) / (3.0 * 127.5) - 1.0;

                    if (noiseValue < minNoise)
                    {
                        minNoise = noiseValue;
                    }
                    if (noiseValue > maxNoise)
                    {
                        maxNoise = noiseValue;
                    }
                }
            }

            Console.WriteLine($"Smallest noise value: {minNoise}");
            Console.WriteLine($"Greatest noise value: {maxNoise}");
        }
    }
}
