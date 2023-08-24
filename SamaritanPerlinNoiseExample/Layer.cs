using System;

namespace SamaritanPerlinNoise
{
    public class Layer
    {
        private double amplitude;
        private double frequency;

        public Layer(double amplitude, double frequency)
        {
            // Initialize the layer with given amplitude and frequency:
            this.amplitude = amplitude;
            this.frequency = frequency;
        }

        public double GetNoiseValue(double x, double y, double z)
        {
            // Calculate the noise value for the given coordinates using PerlinNoise,
            // Scale it by the layer's amplitude and frequency:
            return amplitude * PerlinNoise.Noise(x * frequency, y * frequency, z * frequency);
        }
    }
}
