using System;

namespace NeuralNetwork
{
    public class Neuron
    {
        double bias; 
        Dentrite[] dentrites;
        public double Output => Compute();
        public double Input { get; set; }
        public ActivationFunction ActivationFunction { get; set; }


        public Neuron(ActivationFunction activationFunction, Neuron[] previousNeurons)
        {
            dentrites = new Dentrite[previousNeurons.Length];
            for(int i = 0; i < dentrites.Length; i ++)
            {
                dentrites[i] = new Dentrite(previousNeurons[i], this);
            }

            ActivationFunction = activationFunction;
        }
        
        public void Randomize(Random random, double min, double max)
        {
            for(int i = 0; i < dentrites.Length; i ++)
            {
                dentrites[i].Weight = random.NextDouble(min, max);
            }
            bias = random.NextDouble(min, max);
        }

        public double Compute()
        {
            double total = 0;
            foreach(Dentrite dentrite in dentrites)
            {
                total += dentrite.Compute();
            }
            total += bias;

            return total;
        }
    }
}
