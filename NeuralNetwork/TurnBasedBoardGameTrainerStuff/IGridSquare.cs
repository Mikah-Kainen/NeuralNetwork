using System;
using System.Collections.Generic;
using System.Text;

using static NeuralNetwork.TurnBasedBoardGameTrainerStuff.Enums;

namespace NeuralNetwork.TurnBasedBoardGameTrainerStuff
{
    public interface IGridSquare<TState> where TState : INetInput
    {
        TState State { get; set; }
        Players Owner { get; set; }
        Func<IGridSquare<TState>, TState> WasActivated { get; }
    }
}
