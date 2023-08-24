using System;

namespace SamaritanPerlinNoise
{
    public class PerlinNoise
    {
        private static int[] permutation;
        private static int[] p = new int[512];

        static PerlinNoise()
        {
            permutation = GenerateRandomPermutation();
            for (int i = 0; i < 256; i++)
            {
                p[i] = permutation[i];
                p[i + 256] = permutation[i];
            }
        }
 
        // Generate random permutations:
        private static int[] GenerateRandomPermutation()
        {
            int[] perm = new int[256];
            Random rand = new Random();

            for (int i = 0; i < 256; i++)
            {
                perm[i] = i;
            }

            // Shuffle the permutation array:
            for (int i = 0; i < 256; i++)
            {
                int j = rand.Next(i, 256);
                int temp = perm[i];
                perm[i] = perm[j];
                perm[j] = temp;
            }

            return perm;
        }

        // Apply fade function for smoothing interpolation:
        private static double Fade(double t)
        {
            // The fade function is used for smoothing the interpolation.
            // It takes a parameter t and applies the following polynomial:
            // f(t) = 6t^5 - 15t^4 + 10t^3
            // This function ensures smoother transitions between gradients.
            return t * t * t * (t * (t * 6 - 15) + 10);
        }

        // Linear interpolation between two values:
        private static double Lerp(double t, double a, double b)
        {
            // Linearly interpolate between a and b using t as the weight
            return a + t * (b - a);
        }

        // Calculate dot product of gradient and distance vector:
        private static double Grad(int hash, double x, double y, double z)
        {
            int h = hash & 15;
            double u = h < 8 ? x : y;
            double v = h < 4 ? y : h == 12 || h == 14 ? x : z;

            // Apply dot product of gradient and distance vector:
            return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
        }

        // Generate 3D Perlin noise value at a given point (x, y, z):
        public static double Noise(double x, double y, double z)
        {
            int X = (int)Math.Floor(x) & 255;
            int Y = (int)Math.Floor(y) & 255;
            int Z = (int)Math.Floor(z) & 255;

            // Calculate the fractional part of the coordinates:
            x -= Math.Floor(x);
            y -= Math.Floor(y);
            z -= Math.Floor(z);

            // Apply the fade function to the fractional coordinates:
            double u = Fade(x);
            double v = Fade(y);
            double w = Fade(z);

            int A = p[X] + Y;
            int AA = p[A] + Z;
            int AB = p[A + 1] + Z;
            int B = p[X + 1] + Y;
            int BA = p[B] + Z;
            int BB = p[B + 1] + Z;

            // Interpolate the gradients at the 8 corners of the unit cube:
            return Lerp(w, Lerp(v, Lerp(u, Grad(p[AA], x, y, z), Grad(p[BA], x - 1, y, z)),
                                    Lerp(u, Grad(p[AB], x, y - 1, z), Grad(p[BB], x - 1, y - 1, z))),
                            Lerp(v, Lerp(u, Grad(p[AA + 1], x, y, z - 1), Grad(p[BA + 1], x - 1, y, z - 1)),
                                    Lerp(u, Grad(p[AB + 1], x, y - 1, z - 1), Grad(p[BB + 1], x - 1, y - 1, z - 1))));
        }
    }
}