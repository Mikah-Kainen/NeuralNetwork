using System;

namespace NeuralNetwork
{

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            ErrorFunction meanSquared = new ErrorFunction((double output, double desired) => (output-desired) * (output-desired), (double output, double desired) => -2 * (output - desired));
            ActivationFunction tanh = new ActivationFunction(Math.Tanh, (double input) => 1 - Math.Tanh(input) * Math.Tanh(input));

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

            var result = Net.Compute(new double[] { 1, 2});
        }
    }
}
