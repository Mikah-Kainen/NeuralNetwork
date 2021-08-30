namespace NeuralNetwork
{
    public class Dentrite
    {
        public double Weight { get; set; }
        public Neuron Previous { get; }
        public Neuron Next { get; }


        public Dentrite(Neuron previous, Neuron next)
        {
            Previous = previous;
            Next = next;
        }
        public Dentrite(Neuron previous, Neuron next, double weight)
            : this(previous, next)
        {
            Weight = weight;
        }


        public double Compute()
        {
            return Weight * Previous.Output;
        }
    }
}
