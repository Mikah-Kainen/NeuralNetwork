using Newtonsoft.Json;

using System;

namespace NeuralNetwork
{
    public class Neuron
    {
        public double Bias { get; set; }
        public Dentrite[] Dentrites { get; set; }

        [JsonIgnore]
        public double Output { get; set; }
        [JsonIgnore]
        public double Input { get; private set; }

        public Neuron() { }

        public Neuron(Neuron[] previousNeurons)
        {

            Dentrites = new Dentrite[previousNeurons.Length];
            for (int i = 0; i < Dentrites.Length; i++)
            {
                Dentrites[i] = new Dentrite(previousNeurons[i]);
            }
        }

        public Neuron(Neuron neuron, Dentrite[] dentrites, Layer previousLayer)
        {
            this.Bias = neuron.Bias;
            Dentrites = dentrites;
            for (int i = 0; previousLayer != null && i < previousLayer.Neurons.Length; i++)
            {
                Dentrites[i].Previous = previousLayer.Neurons[i];
            }
            this.Input = neuron.Input;
        }

        public void Randomize(Random random, double min, double max)
        {
            for (int i = 0; i < Dentrites.Length; i++)
            {
                Dentrites[i].Weight = random.NextDouble(min, max);
            }
            Bias = random.NextDouble(min, max);
        }

        public double Compute(ActivationFunction activationFunction)
        {
            double total = 0;
            foreach (Dentrite dentrite in Dentrites)
            {
                total += dentrite.Compute();
            }
            total += Bias;

            Input = total;
            Output = activationFunction.Function(total);
            return Output;
        }
    }
}
