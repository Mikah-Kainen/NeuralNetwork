using System;
using System.Collections.Generic;
using System.Text;

using NeuralNetwork.TurnBasedBoardGameTrainerStuff.Enums;

namespace NeuralNetwork.TurnBasedBoardGameTrainerStuff
{
    public interface IGridSquare<TState> where TState : INetInputer
    {
        TState State { get; set; }
        Action<IGridSquare<TState>> WasActivated { get; }
    }
}
