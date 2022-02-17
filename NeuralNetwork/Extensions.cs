using NeuralNetwork.TurnBasedBoardGameTrainerStuff;
using NeuralNetwork.TurnBasedBoardGameTrainerStuff.Enums;

using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNetwork
{
    public static class Extensions
    {

        public static double NextDouble(this Random random, double min, double max)
        {
            return random.NextDouble() * (max - min) + min;
        }

        public static (int, int) RandomMove<TState, TSquare>(this IGridBoard<TState, TSquare> currentGame, Random random)
            where TState : INetInputer
            where TSquare : IGridSquare<TState>
        {
            var currentBoard = currentGame;

            int returnY = random.Next(0, currentBoard.YLength);
            int returnX = random.Next(0, currentBoard.XLength);

            while (currentBoard[returnY, returnX].Owner != Players.None)
            {
                returnX++;
                if (returnX >= currentBoard.XLength)
                {
                    returnX = 0;
                    returnY++;
                    if (returnY >= currentBoard.YLength)
                    {
                        returnY = 0;
                    }
                }
            }

            return (returnY, returnX);
        }
    }
}
