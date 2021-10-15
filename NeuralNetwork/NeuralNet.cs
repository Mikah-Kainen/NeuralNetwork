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

        //public NeuralNet(ErrorFunctions errorFunc, ActivationFunction activationFunction, double[] serialized)
        //{
        //    ErrorFunction = errorFunc;
        //    List<Layer> currentLayers = new List<Layer>();
        //    int currentIndex = 0;
        //    currentLayers.Add(new Layer((int)serialized[currentIndex]));
        //    currentIndex++;
        //    while(currentIndex < serialized.Length)
        //    {
        //        Layer currentLayer = new Layer(activationFunction, (int)serialized[currentIndex], currentLayers[currentLayers.Count - 1]);
        //        currentIndex++;
        //        for (int i = 0; i < currentLayer.Neurons.Length; i ++)
        //        {
        //            currentLayer.Neurons[i].Bias = serialized[currentIndex];
        //            currentIndex++;
        //            for(int x = 0; x < currentLayer.Neurons[i].Dentrites.Length; x ++)
        //            {
        //                currentLayer.Neurons[i].Dentrites[x].Weight = serialized[currentIndex];
        //                currentIndex++;
        //            }
        //        }
        //        currentLayers.Add(currentLayer);
        //    }

        //    Layers = currentLayers.ToArray();
        //}

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
    }
}
