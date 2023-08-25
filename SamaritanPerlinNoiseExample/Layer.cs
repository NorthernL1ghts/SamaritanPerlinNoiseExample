using System;

namespace SamaritanPerlinNoise
{
    public class Layer
    {
        private double amplitude;
        private double frequencyModifier;

        public double FrequencyModifier
        {
            get { return frequencyModifier; }
            set { frequencyModifier = value; }
        }

        public Layer(double frequencyModifier)
        {
            // Initialize the layer with given frequency modifier:
            this.frequencyModifier = frequencyModifier;

            // Calculate the amplitude based on frequency modifier:
            amplitude = Math.Pow(0.5, frequencyModifier - 1);
        }

        public double GetNoiseValue(double x, double y, double z)
        {
            // Calculate the frequency based on frequency modifier:
            double frequency = frequencyModifier;

            // Calculate the noise value for the given coordinates using PerlinNoise,
            // Scale it by the layer's amplitude and frequency, without the -1 to 1 mapping:
            return amplitude * PerlinNoise.Noise(x * frequency, y * frequency, z * frequency);
        }
    }
}
