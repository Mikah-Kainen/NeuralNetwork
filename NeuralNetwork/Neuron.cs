using System;

namespace NeuralNetwork
{
    public class Neuron
    {
        double bias; 
        Dentrite[] dentrites;
        public double Output => ActivationFunction.Function(Input);
        private double input;
        public double Input {
            get 
            { 
                if(dentrites.Length > 0)
                {
                    return Compute();
                }
                else
                {
                    return input;
                }
            }
            set { input = value; }
        }
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

            Input = total;
            return Output;
        }
    }
}
