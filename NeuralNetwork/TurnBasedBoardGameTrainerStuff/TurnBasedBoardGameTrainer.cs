using System;
using System.Collections.Generic;
using System.Text;

using System.Linq;
using Newtonsoft.Json;

namespace NeuralNetwork.TurnBasedBoardGameTrainerStuff
{
    public class Pair<TState, TSquare>
        where TState : INetInputer
        where TSquare : IGridSquare<TState> 
    {
        public IGridBoard<TState, TSquare> Board { get; set; }
        public NeuralNet Net { get; set; }
        public int Success { get; set; }
        public bool IsAlive { get; set; }
        public Pair(IGridBoard<TState, TSquare> board, NeuralNet net)
        {
            Board = board;
            Net = net;
            Success = 0;
            IsAlive = true;
        }
    }

    public static class TurnBasedBoardGameTrainer<TState, TSquare> 
        where TState : INetInputer
        where TSquare : IGridSquare<TState>
    {
        
        public static NeuralNet LoadNet(string filePath)
        //this function loads and builds a NeuralNetwork saved in json format. The Inputs are not set in this function
        {
            string json = System.IO.File.ReadAllText(filePath);
            NeuralNet result = JsonConvert.DeserializeObject<NeuralNet>(json);

            for (int i = result.Layers.Length - 1; i > 0; i--)
            {
                result.Layers[i].PreviousLayer = result.Layers[i - 1];
                result.Layers[i].Output = new double[result.Layers[i].Neurons.Length];
                for (int x = 0; x < result.Layers[i].Neurons.Length; x++)
                {
                    for (int z = 0; z < result.Layers[i].PreviousLayer.Neurons.Length; z++)
                    {
                        result.Layers[i].Neurons[x].Dentrites[z].Previous = result.Layers[i].PreviousLayer.Neurons[z];
                    }
                }
            }

            return result;
        }

        public static NeuralNet GetNet(IGridBoard<TState, TSquare> completeGame, int numberOfSimulations, int numberOfGenerations, Random random)
        {
            int[] neuronsPerLayer = new int[]
            {
                completeGame.YLength * completeGame.XLength,
                4,
                3,
                4,
                completeGame.YLength * completeGame.XLength,
            };
            List<Pair<TState, TSquare>> pairs = new List<Pair<TState, TSquare>>();
            for (int i = 0; i < numberOfSimulations; i++)
            {
                NeuralNet pairNet = new NeuralNet(ErrorFunctions.MeanSquared, ActivationFunctions.BinaryStep, neuronsPerLayer);
                pairNet.Randomize(random, -1, 1);
                pairs.Add(new Pair<TState, TSquare>(completeGame, pairNet));
            }
            NeuralNet best = null;
            for (int i = 0; i < numberOfGenerations; i++)
            {
                best = Train(pairs, random, 10, 10, 0.5f, 1.5f, -1, 1);
            }
            return best;
        }
        //int correctCount = 0;

        private static NeuralNet Train(List<Pair<TState, TSquare>> pairs, Random random, double preservePercent, double randomizePercent, double mutationMin, double mutationMax, double randomizeMin, double randomizeMax)
        //preservePercent => percent of population to save, randomizePercent => percent of population to randomize, mutationRange => multiply mutations by a random value between positive and negative mutationRange
        {
            bool IsThereBoardAlive = true;
            while (IsThereBoardAlive)
            {
                for (int i = 0; i < pairs.Count; i++)
                {
                    IsThereBoardAlive = MakeMove(pairs[i]);
                }
            }
            pairs = pairs.OrderByDescending<Pair<TState, TSquare>, int>((Pair<TState, TSquare> current) => current.Success).ToList();

            int preserveCutoff = (int)(pairs.Count * preservePercent / 100);
            int randomizeCutoff = (int)(pairs.Count * (100 - randomizePercent) / 100);
            for (int i = 0; i < preserveCutoff; i++)
            {
                pairs[i].IsAlive = true;
                if (pairs[i].Net.Layers[3].Neurons[0].Bias != 0)
                {

                }
            }
            for (int i = preserveCutoff; i < randomizeCutoff; i++)
            {
                int parent = random.Next(0, preserveCutoff);
                pairs[i].Net.Cross(pairs[parent].Net, random);
                pairs[i].Net.Mutate(random, mutationMin, mutationMax);
                pairs[i].IsAlive = true;
                if (pairs[i].Net.Layers[3].Neurons[0].Bias != 0)
                {

                }
            }
            for (int i = randomizeCutoff; i < pairs.Count; i++)
            {
                pairs[i].Net.Randomize(random, randomizeMin, randomizeMax);
                pairs[i].IsAlive = true;
                if (pairs[i].Net.Layers[3].Neurons[0].Bias != 0)
                {

                }
            }

            return pairs[0].Net;
        }

        private static bool MakeMove(Pair<TState, TSquare> currentPair)
        {
            bool returnValue = false;
            if (currentPair.IsAlive)
            {
                //currentPair.Success++;
                double[] inputs = new double[currentPair.Board.YLength * currentPair.Board.XLength];
                for (int y = 0; y < currentPair.Board.YLength; y++)
                {
                    for (int x = 0; x < currentPair.Board.XLength; x++)
                    {
                        switch (currentPair.Board[y, x])
                        {
                            //case Players.None:
                            //    inputs[y * currentPair.Board.YLength + x] = 0;
                            //    break;

                            //case Players.Player1:
                            //    inputs[y * currentPair.Board.YLength + x] = 1;
                            //    break;

                            //case Players.Player2:
                            //    inputs[y * currentPair.Board.YLength + x] = 2;
                            //    break;

                            //case Players.Player3:
                            //    inputs[y * currentPair.Board.YLength + x] = 3;
                            //    break;
                        }
                    }
                }
                double[] computedValues = currentPair.Net.Compute(inputs);
                int target = -1;
                for (int a = 0; a < computedValues.Length; a++)
                {
                    if (computedValues[a] == 1)
                    {
                        if (target != -1)
                        {
                            currentPair.IsAlive = false;
                            goto deathZone;
                        }
                        target = a;
                    }
                }
                if (target == -1)
                {
                    currentPair.IsAlive = false;
                    goto deathZone;
                }
                int yVal = target / currentPair.Board.YLength;
                int xVal = target % currentPair.Board.XLength;
                //if (currentPair.Board[yVal, xVal] != Players.None)
                //{
                //    currentPair.IsAlive = false;
                //    goto deathZone;
                //}
                List<IGridBoard<TState, TSquare>> children = currentPair.Board.GetChildren();
                for (int z = 0; z < children.Count; z++)
                {
                    //if (children[z][yVal, xVal] == currentPair.Board.NextPlayer)
                    //{
                    //    currentPair.Board = children[z];
                    //    currentPair.Success++;
                    //}
                }
                if (currentPair.Board.IsTerminal == true)
                {
                    currentPair.Success = 10000;
                    currentPair.IsAlive = false;
                }
                else
                {
                    returnValue = true;
                }
                currentPair.Success++;
            //                correctCount++;
            deathZone:;
            }
            return returnValue;
        }
    }
}
