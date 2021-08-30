using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNetwork
{
    public class ErrorFunction
    {
        public Func<double, double, double> Function { get; private set; }
        public Func<double, double, double> Derivitive { get; private set; }


        public ErrorFunction(Func<double, double, double> function, Func<double, double, double> derivitive)
        {
            Function = function;
            Derivitive = derivitive;
        }
    }
}
