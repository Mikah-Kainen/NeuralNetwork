using System;
using System.Collections.Generic;
using System.Text;

using System.Linq;
using Newtonsoft.Json;
using NeuralNetwork.TurnBasedBoardGameTrainerStuff.Enums;

namespace NeuralNetwork.TurnBasedBoardGameTrainerStuff
{
    public delegate (bool isThereAnotherMove, TReturnStats moveStats) MyFunc<TState, TSquare, TReturnStats>(BoardNetPair<TState, TSquare> pair, Random random)
        where TState : INetInputer
        where TSquare : IGridSquare<TState>;

    //class ExampleDemoThingie
    //{
    //    bool Test<TState, TSquare>(MyFunc<TState, TSquare> myFunc)
    //        where TState : INetInputer
    //        where TSquare : IGridSquare<TState>
    //    {
    //        int a = 3;
    //        return myFunc(null, ref a);
    //    }


    //    void CallTest<TState, TSquare>()
    //        where TState : INetInputer
    //        where TSquare : IGridSquare<TState>
    //    {
    //        var result = Test<TState, TSquare>((pair, ref a) =>
    //        {
    //            a++;
    //            return a > 3;
    //        });
    //    }
    //}

    public class BoardNetPair<TState, TSquare>
        where TState : INetInputer
        where TSquare : IGridSquare<TState>
    {
        public IGridBoard<TState, TSquare> Board { get; set; }
        public NeuralNet Net { get; set; }
        public int Success { get; set; }
        public bool IsAlive { get; set; }
        public BoardNetPair(IGridBoard<TState, TSquare> board, NeuralNet net)
        {
            Board = board;
            Net = net;
            Success = 0;
            IsAlive = true;
        }
    }

    public class TurnBasedBoardGameTrainer<TState, TSquare, TMoveStats>
        where TState : INetInputer
        where TSquare : IGridSquare<TState>
    {
        public List<TMoveStats>[][] TrainingStats;

        public TurnBasedBoardGameTrainer()
        {
        }

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

        public NeuralNet GetNet(IGridBoard<TState, TSquare> rootState, MyFunc<TState, TSquare, TMoveStats> makeMove, int numberOfSimulations, int numberOfGenerations, Random random)
        {
            TrainingStats = new List<TMoveStats>[numberOfGenerations][];
            for(int i = 0; i < numberOfGenerations; i ++)
            {
                TrainingStats[i] = new List<TMoveStats>[numberOfSimulations];
                for(int r = 0; r < numberOfSimulations; r ++)
                {
                    TrainingStats[i][r] = new List<TMoveStats>();
                }
            }

            int[] neuronsPerLayer = new int[]
            {
                rootState.YLength * rootState.XLength,
                4,
                3,
                4,
                rootState.YLength * rootState.XLength,
            };
            List<BoardNetPair<TState, TSquare>> pairs = new List<BoardNetPair<TState, TSquare>>();
            for (int i = 0; i < numberOfSimulations; i++)
            {
                NeuralNet pairNet = new NeuralNet(ErrorFunctions.MeanSquared, ActivationFunctions.BinaryStep, neuronsPerLayer);
                pairNet.Randomize(random, -1, 1);
                pairs.Add(new BoardNetPair<TState, TSquare>(rootState, pairNet));
            }
            NeuralNet best = null;
            for (int i = 0; i < numberOfGenerations; i++)
            {
                best = Train(pairs, makeMove, i, random, 10, 10, 0.5f, 1.5f, -1, 1);
            }
            return best;
        }


        private NeuralNet Train(List<BoardNetPair<TState, TSquare>> pairs, MyFunc<TState, TSquare, TMoveStats> makeMove, int currentGeneration, Random random, double preservePercent, double randomizePercent, double mutationMin, double mutationMax, double randomizeMin, double randomizeMax)
        //preservePercent => percent of population to save, randomizePercent => percent of population to randomize, mutationRange => multiply mutations by a random value between positive and negative mutationRange
        {

            for (int i = 0; i < pairs.Count; i++)
            {
                bool isTherePossibleMove = !pairs[i].Board.IsTerminal;
                while (isTherePossibleMove)
                {
                    (bool isTherePossibleMove, TMoveStats currentStats) moveInfo = makeMove(pairs[i], random);
                    isTherePossibleMove = moveInfo.isTherePossibleMove;
                    TrainingStats[currentGeneration][i].Add(moveInfo.currentStats);
                }
            }
            pairs = pairs.OrderByDescending<BoardNetPair<TState, TSquare>, int>((BoardNetPair<TState, TSquare> current) => current.Success).ToList();

            int preserveCutoff = (int)(pairs.Count * preservePercent / 100);
            int randomizeCutoff = (int)(pairs.Count * (100 - randomizePercent) / 100);
            for (int i = 0; i < preserveCutoff; i++)
            {
                pairs[i].IsAlive = true;
                if (pairs[i].Net.Layers[3].Neurons[0].Bias == 0)
                {

                }
            }
            for (int i = preserveCutoff; i < randomizeCutoff; i++)
            {
                int parent = random.Next(0, preserveCutoff);
                pairs[i].Net.Cross(pairs[parent].Net, random);
                pairs[i].Net.Mutate(random, mutationMin, mutationMax);
                pairs[i].IsAlive = true;
                if (pairs[i].Net.Layers[3].Neurons[0].Bias == 0)
                {

                }
            }
            for (int i = randomizeCutoff; i < pairs.Count; i++)
            {
                pairs[i].Net.Randomize(random, randomizeMin, randomizeMax);
                pairs[i].IsAlive = true;
                if (pairs[i].Net.Layers[3].Neurons[0].Bias == 0)
                {

                }
            }
            return pairs[0].Net;
        }
    }
}
