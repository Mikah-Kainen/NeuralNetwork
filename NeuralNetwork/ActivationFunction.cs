using System;

namespace NeuralNetwork
{
    public class ActivationFunction
    {
        public Func<double, double> Function { get; private set; }
        public Func<double, double> Derivitive { get; private set; }

        public ActivationFunction(Func<double, double> function, Func<double, double> derivitive)
        {
            Function = function;
            Derivitive = derivitive;
        }
    }
}
