using System;
using System.Collections.Generic;
using System.Text;

using static NeuralNetwork.TurnBasedBoardGameTrainerStuff.Enums;

namespace NeuralNetwork.TurnBasedBoardGameTrainerStuff
{
    public interface IGridSquare<TState> where TState : INetInputer
    {
        TState State { get; set; }
        Players Owner { get; set; }
        Action<IGridSquare<TState>> WasActivated { get; }
    }
}
