using System;
using System.Collections.Generic;
using System.Text;

using static NeuralNetwork.TurnBasedBoardGameTrainerStuff.Enums;

namespace NeuralNetwork.TurnBasedBoardGameTrainerStuff
{
    public interface IGridBoard<TState> where TState : INetInputer
    {
        IGridSquare<TState>[][] CurrentGame { get; }
        int YLength => CurrentGame?.Length ?? -1;
        int XLength => CurrentGame?[0]?.Length ?? -1;
        //Players PreviousPlayer { get; set; }
        //Players NextPlayer => GetNextPlayer[PreviousPlayer](this);
        Players NextPlayer { get; }
        bool IsTerminal { get; }        
        IGridSquare<TState> this[int y, int x] { get; set; }

        List<IGridBoard<TState>> GetChildren();
    }
}
