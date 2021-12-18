﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNetwork
{
    public static class Functions
    {
        public static ErrorFunction MeanSquared = new ErrorFunction((double output, double desired) => (output - desired) * (output - desired), (double output, double desired) => -2 * (output - desired));
        public static ActivationFunction Tanh = new ActivationFunction(Math.Tanh, (double input) => 1 - Math.Tanh(input) * Math.Tanh(input));
        public static ActivationFunction BinaryStep = new ActivationFunction(binaryStep, (double input) => 0);
        static double binaryStep(double input)
        {
            if(input < 0)
            {
                return 0;
            }
            return 1;
        }

        public static Dictionary<ErrorFunctions, ErrorFunction> GetErrorFunction = new Dictionary<ErrorFunctions, ErrorFunction>()
        {
            [ErrorFunctions.MeanSquared] = MeanSquared,
        };

        public static Dictionary<ActivationFunctions, ActivationFunction> GetActivationFunction = new Dictionary<ActivationFunctions, ActivationFunction>()
        {
            [ActivationFunctions.None] = null,
            [ActivationFunctions.Tanh] = Tanh,
            [ActivationFunctions.BinaryStep] = BinaryStep,
        };
    }
}
