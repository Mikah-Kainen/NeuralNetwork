using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeuralNetwork
{
    public class Layer
    {
        public Neuron[] Neurons { get; set; }
        public double[] Outputs => Neurons.Select(n => n.Compute()).ToArray();


        public Layer(ActivationFunction activationFunction, int numberOfNeurons, Layer previousLayer)
        {
            Neurons = new Neuron[numberOfNeurons];
            for(int i = 0; i < Neurons.Length; i ++)
            {
                Neurons[i] = new Neuron(activationFunction, previousLayer.Neurons);
            }
        }

        public void Randomize(Random random, double min, double max)
        {
            for(int i = 0; i < Neurons.Length; i ++)
            {
                Neurons[i].Randomize(random, min, max);
            }
        }
            


        /// <summary>
        /// ///IDK if this is returning the right this maybe I should return the dentrites' computes instead
        /// </summary>
        /// <returns></returns>
        public double[] Compute()
        {
            double[] returnVal = new double[Neurons.Length];
            for(int i = 0; i < Neurons.Length; i ++)
            {
                returnVal[i] = Neurons[i].Output;
            }

            return returnVal;
        }

    }
}
