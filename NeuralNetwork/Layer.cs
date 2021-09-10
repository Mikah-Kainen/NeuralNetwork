using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeuralNetwork
{
    public class Layer
    {
        public Layer PreviousLayer { get; set; }
        public Neuron[] Neurons { get; set; }
        public double[] Output { get; }


        public Layer(ActivationFunction activationFunction, int numberOfNeurons, Layer previousLayer)
        {
            Output = new double[numberOfNeurons];
            Neurons = new Neuron[numberOfNeurons];
            for(int i = 0; i < Neurons.Length; i ++)
            {
                Neurons[i] = new Neuron(activationFunction, previousLayer.Neurons);
            }

            PreviousLayer = previousLayer;
        }

        public Layer(int numberOfNeurons)
        {
            PreviousLayer = null;
            Output = new double[numberOfNeurons];
            Neurons = new Neuron[numberOfNeurons];
            for(int i = 0; i < numberOfNeurons; i ++)
            {
                Neurons[i] = new Neuron(null, new Neuron[0]);
                Neurons[i].Output = 0;
            }
        }

        public void Randomize(Random random, double min, double max)
        {
            for(int i = 0; i < Neurons.Length; i ++)
            {
                Neurons[i].Randomize(random, min, max);
            }
        }
            
        public double[] Compute()
        {
            if(PreviousLayer == null)
            {
                return Output;
            }

            PreviousLayer.Compute();
            for(int i = 0; i < Neurons.Length; i ++)
            {
                Output[i] = Neurons[i].Compute();
            }

            return Output;
        }

    }
}
