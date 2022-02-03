using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNetwork.TurnBasedBoardGameTrainerStuff
{
    public interface INetInput
    {
        public double[] Inputs { get; }
    }
}
