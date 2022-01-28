using System;
using System.Collections.Generic;
using System.Text;

using static NeuralNetwork.TurnBasedBoardGameTrainerStuff.Enums;

namespace NeuralNetwork.TurnBasedBoardGameTrainerStuff
{
    public interface IGridBoard
    {
        public IGridSquare[][] CurrentGame { get; }
        public int YLength => CurrentGame?.Length ?? -1;
        public int XLength => CurrentGame?[0]?.Length ?? -1;
        //public Players PreviousPlayer { get; set; }
        //public Players NextPlayer => GetNextPlayer[PreviousPlayer](this);
        public Players NextPlayer { get; }
        public bool IsTerminal { get; }
    }
}
