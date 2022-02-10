using System;
using System.Collections.Generic;
using System.Text;

using static NeuralNetwork.TurnBasedBoardGameTrainerStuff.Enums;

namespace NeuralNetwork.TurnBasedBoardGameTrainerStuff
{
    public interface IGridBoard<TState, TSquare> 
        where TState : INetInputer 
        where TSquare : IGridSquare<TState>
    {
        TSquare[][] CurrentGame { get; }
        //int YLength => CurrentGame?.Length ?? -1;
        //int XLength => CurrentGame?[0]?.Length ?? -1;
        Players PreviousPlayer { get; }
        Players NextPlayer => GetNextPlayer[PreviousPlayer](this);
        bool IsTerminal { get; }        
        TSquare this[int y, int x] { get; set; }
        Dictionary<Players, Func<IGridBoard<TState, TSquare>, Players>> GetNextPlayer { get; }
        List<IGridBoard<TState, TSquare>> GetChildren();
    }
}
