
using Newtonsoft.Json;

namespace NeuralNetwork
{
    public class Dentrite
    {
        public double Weight { get; set; }
        
        [JsonIgnore]
        public Neuron Previous { get; set; }

        public Dentrite() { }

        public Dentrite(Neuron previous)
        {
            Previous = previous;
        }
        public Dentrite(Neuron previous, double weight)
            : this(previous)
        {
            Weight = weight;
        }

        public double Compute()
        {
            return Weight * Previous.Output;
        }
    }
}
