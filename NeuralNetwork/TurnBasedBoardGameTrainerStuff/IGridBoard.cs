using System;
using System.Collections.Generic;
using System.Text;

using NeuralNetwork.TurnBasedBoardGameTrainerStuff.Enums;

namespace NeuralNetwork.TurnBasedBoardGameTrainerStuff
{
    public interface IGridBoard<TState, TSquare> 
        where TState : INetInputer 
        where TSquare : IGridSquare<TState>
    {
        TSquare[][] CurrentBoard { get; }
        int YLength { get; }
        int XLength { get; }
        //Players PreviousPlayer { get; }
        Players NextPlayer { get; }
        bool IsTerminal { get; }
        Players GetWinner();
        TSquare this[int y, int x] { get; set; }
        //Dictionary<Players, Func<IGridBoard<TState, TSquare>, Players>> GetNextPlayer { get; }
        List<IGridBoard<TState, TSquare>> GetChildren();
        void SetCurrentGame(TSquare[][] targetBoard, Players previousPlayer);
        IGridBoard<TState, TSquare> Clone();
    }
}
