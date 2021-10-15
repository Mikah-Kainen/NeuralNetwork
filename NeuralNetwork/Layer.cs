using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeuralNetwork
{
    public class Layer
    {
        public ActivationFunctions ActivationFunction { get; set; }

        [JsonIgnore]
        public Layer PreviousLayer { get; set; }
        public Neuron[] Neurons { get; set; }

        [JsonIgnore]
        public double[] Output { get; set; }

        [JsonIgnore]
        public ActivationFunction ActivationFunc => Functions.GetActivationFunction[ActivationFunction];

        public Layer() { }

        public Layer(ActivationFunctions activationFunction, int numberOfNeurons, Layer previousLayer)
        {
            Output = new double[numberOfNeurons];
            Neurons = new Neuron[numberOfNeurons];
            for(int i = 0; i < Neurons.Length; i ++)
            {
                Neurons[i] = new Neuron(previousLayer.Neurons);
            }

            ActivationFunction = activationFunction;
            PreviousLayer = previousLayer;
        }

        public Layer(int numberOfNeurons)
        {
            PreviousLayer = null;
            Output = new double[numberOfNeurons];
            Neurons = new Neuron[numberOfNeurons];
            for(int i = 0; i < numberOfNeurons; i ++)
            {
                Neurons[i] = new Neuron(new Neuron[0]);
                Neurons[i].Output = 0;
            }
            ActivationFunction = ActivationFunctions.None;
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
                Output[i] = Neurons[i].Compute(ActivationFunc);
            }

            return Output;
        }

    }
}
