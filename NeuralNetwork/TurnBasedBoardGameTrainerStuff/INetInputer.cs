using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNetwork.TurnBasedBoardGameTrainerStuff
{
    public interface INetInputer
    {
        public double[] NetInputs { get; }
    }
}
