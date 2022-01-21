using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Text;


namespace NeuralNetwork
{
    public class NeuralNet
    {
        public ErrorFunctions ErrorFunction { get; set; }
        [JsonIgnore]
        public ErrorFunction ErrorFunc { get => Functions.GetErrorFunction[ErrorFunction]; }
        public Layer[] Layers { get; set; }


        public NeuralNet() { }

        public NeuralNet(ErrorFunctions errorFunc, ActivationFunctions activationFunction, int[] neuronsPerLayer)
        {
            ErrorFunction = errorFunc;
            Layers = new Layer[neuronsPerLayer.Length];
            Layers[0] = new Layer(neuronsPerLayer[0]);
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
            for(int i = 0; i < Layers[0].Neurons.Length; i ++)
            {
                Layers[0].Neurons[i].Output = inputs[i];
            }
            double[] returnVals = Layers[Layers.Length - 1].Compute();

            return returnVals;
        }

        public void Cross(NeuralNet parent, Random random)
        {
            for(int i = 1; i < Layers.Length; i ++)
            {
                int crossIndex = random.Next(0, Layers[i].Neurons.Length);
                int sideToKeep = random.Next(0, 2);
                int left;
                int right;
                if (sideToKeep == 0)
                {
                    left = crossIndex;
                    right = Layers[i].Neurons.Length;
                }
                else
                {
                    left = 0;
                    right = crossIndex;
                }
                for (int z = left; z < right; z++)
                {
                    Layers[i].Neurons[z].Bias = parent.Layers[i].Neurons[z].Bias;
                }
            }
        }

        public void Mutate(Random random, double mutationMin, double mutationMax)
            //a percent of the current bias between mutationMin and mutationMax is randomly added or subtracted from the current bias to create the new bias
        {
            for(int i = 0; i < Layers.Length; i ++)
            {
                int mutateIndex = random.Next(0, Layers[i].Neurons.Length);
                double mutationRate = random.NextDouble(mutationMin, mutationMax);
                int negativeOrPositive = random.Next(0, 2);
                double currentBias = Layers[i].Neurons[mutateIndex].Bias;
                double mutationValue;
                if(negativeOrPositive == 0)
                {
                    mutationValue = -1 * currentBias * mutationRate;
                }
                else
                {
                    mutationValue = currentBias * mutationRate;
                }
                Layers[i].Neurons[mutateIndex].Bias += mutationValue;
            }
        }

        //number of neurons(Layer1), neuron1, all dentrites, neuron2, all dentrites... number of neurons(Layer2), neuron1, all dentrites...
        public double[] Serialize()
        {
            List<double> returnValues = new List<double>();

            returnValues.Add(Layers[0].Neurons.Length);
            for(int i = 1; i < Layers.Length; i ++)
            {
                returnValues.Add(Layers[i].Neurons.Length);
                for (int x = 0; x < Layers[i].Neurons.Length; x ++)
                {
                    returnValues.Add(Layers[i].Neurons[x].Bias);
                    for(int z = 0; z < Layers[i].Neurons[x].Dentrites.Length; z ++)
                    {
                        returnValues.Add(Layers[i].Neurons[x].Dentrites[z].Weight);
                    }
                }
            }


            return returnValues.ToArray();
        }

        public void SaveToFile(string fileName)
        {

        }
    }
}
