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
            for(int i = 0; i < Layers.Length; i ++)
            {
                Layers[i] = new Layer(,);

                ///////Dont forget to actually use the activation function in all of the computes
                //////
                //////
                //////
                ////
                /////
                /////
                ////
                /////
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
            for()
            {

            }



        }
    }
}
