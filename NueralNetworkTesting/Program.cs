using NeuralNetwork;
using System;

namespace NueralNetworkTesting
{

    class Program
    {
        static void Main(string[] args)
        {\
            Console.WriteLine("Hello World!");
            //testingtestintestinsg 
            int[] layers = new int[]
            {
                2,
                3,
                3,
                2,
            };

            NeuralNet Net = new NeuralNet(ErrorFunctions.MeanSquared, ActivationFunctions.Tanh, layers);
            Random random = new Random();
            Net.Randomize(random, -1, 1);

            var result = Net.Compute(new double[] { 1, 2 });
        }
    }
}
