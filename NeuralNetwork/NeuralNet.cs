using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNetwork
{
    public class NeuralNet
    {
        public ErrorFunction ErrorFunc { get; private set; }
        public Layer[] Layers { get; set; }


        public NeuralNet(ErrorFunction errorFunc, ActivationFunction activationFunction, int[] neuronsPerLayer)
        {
            ErrorFunc = errorFunc;
            Layers = new Layer[neuronsPerLayer.Length];
            Layers[0] = new Layer(activationFunction, neuronsPerLayer[0], null);
            for(int i = 1; i < Layers.Length; i ++)
            {
                Layers[i] = new Layer(activationFunction, neuronsPerLayer[i], Layers[i-1]);
            }
        }


        public void Randomize(Random random, double min, double max)
        {
            for(int i = 0; i < Layers.Length; i ++)
            {
                Layers[i].Randomize(random, min, max);
            }
        }

        public double[] Compute(double[] inputs)
        {
            double[] returnVals = new double[inputs.Length];
            for(int i = 0; i < inputs.Length; i ++)
            {
                Layers[0].Neurons[i].Input = inputs[i];
            }
            returnVals = Layers[Layers.Length - 1].Compute();

            return returnVals;
        }
    }
}
