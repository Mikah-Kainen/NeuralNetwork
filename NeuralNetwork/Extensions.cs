using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNetwork
{
    public static class Extensions
    {

        public static double NextDouble(this Random random, double min, double max)
        {
            return random.NextDouble() * (max - min) + min;
        }
    }
}
