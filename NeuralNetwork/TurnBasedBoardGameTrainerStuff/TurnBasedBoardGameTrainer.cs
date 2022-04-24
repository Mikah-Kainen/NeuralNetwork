using System;
using System.Collections.Generic;
using System.Text;

using System.Linq;
using Newtonsoft.Json;
using NeuralNetwork.TurnBasedBoardGameTrainerStuff.Enums;

namespace NeuralNetwork.TurnBasedBoardGameTrainerStuff
{
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
        public BoardNetPair(IGridBoard<TState, TSquare> board, NeuralNet net)
        {
            Board = board;
            Net = net;
            Success = 0;
        }
    }

    public class TurnBasedBoardGameTrainer<TState, TSquare>
        where TState : INetInputer
        where TSquare : IGridSquare<TState>
    {
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

        public NeuralNet GetNet(IGridBoard<TState, TSquare> rootState, Players[] allActivePlayers, Func<BoardNetPair<TState, TSquare>, Random, bool> makeMove, Func<IGridBoard<TState, TSquare>, Random, bool>[] opponentMoves, int numberOfSimulations, int numberOfGenerations, Random random, NeuralNet preTrainedNet)
        {
            Dictionary<Players, Func<IGridBoard<TState, TSquare>, Random, bool>> opponentMoveMap = new Dictionary<Players, Func<IGridBoard<TState, TSquare>, Random, bool>>();
            for(int i = 0; i < allActivePlayers.Length - 1; i ++)
            {
                opponentMoveMap.Add(allActivePlayers[i], opponentMoves[i]);
            }
            Players neuralNetPlayer = allActivePlayers[allActivePlayers.Length - 1];
            //Action<TSquare[][], Random> neuralNetAction = (squares, random) => makeMove(, random);

            int[] neuronsPerLayer = new int[]
            {
                rootState.YLength * rootState.XLength,
                6,
                4,
                5,
                rootState.YLength * rootState.XLength,
            };
            List<BoardNetPair<TState, TSquare>> pairs = new List<BoardNetPair<TState, TSquare>>();
            for (int i = 0; i < numberOfSimulations; i++)
            {
                NeuralNet pairNet = new NeuralNet(ErrorFunctions.MeanSquared, ActivationFunctions.BinaryStep, neuronsPerLayer);
                pairNet.Randomize(random, -1, 1);
                pairs.Add(new BoardNetPair<TState, TSquare>(rootState.Clone(), pairNet));
            }
            if(preTrainedNet != null)
            {
                pairs[0].Net = preTrainedNet;
            }
            NeuralNet best = null;
            for (int i = 0; i < numberOfGenerations; i++)
            {
                for (int z = 0; z < numberOfSimulations; z++)
                {
                    pairs[z].Board.SetCurrentGame(rootState.CurrentBoard, Players.None);
                }
                best = Train(pairs, opponentMoveMap, neuralNetPlayer, makeMove, currentGeneration: i, random, preservePercent: 10, randomizePercent: 10, mutationMin: 0.5f, mutationMax: 1.5f, randomizeMin: -1, randomizeMax: 1);
            }
            return best;
        }


        public event EventHandler<(IGridBoard<TState, TSquare>, int)> BoardDied;

        private NeuralNet Train(List<BoardNetPair<TState, TSquare>> pairs, Dictionary<Players, Func<IGridBoard<TState, TSquare>, Random, bool>> opponentMoveMap, Players neuralNetPlayer, Func<BoardNetPair<TState, TSquare>, Random, bool> makeMove/*, Action<TSquare[][], Random>[] makeOpponentMove*/, int currentGeneration, Random random, double preservePercent, double randomizePercent, double mutationMin, double mutationMax, double randomizeMin, double randomizeMax)
        //preservePercent => percent of population to save, randomizePercent => percent of population to randomize, mutationRange => multiply mutations by a random value between positive and negative mutationRange
        //Train only changes the nets of the pairs it doesn't reset the boards. Boards must be reset before Train function is called
        {
            for (int i = 0; i < pairs.Count; i++)
            {
                int movesDone = 0;
                bool shouldContinue = true;
                while (shouldContinue)
                {
                    if (pairs[i].Board.NextPlayer == neuralNetPlayer)
                    {
                        shouldContinue = makeMove(pairs[i], random);
                    }
                    else
                    {
                        shouldContinue = opponentMoveMap[pairs[i].Board.NextPlayer](pairs[i].Board, random);
                    }
                    movesDone++;
                }
                BoardDied?.Invoke(this, (pairs[i].Board, currentGeneration));
            }

            pairs = pairs.OrderByDescending((BoardNetPair<TState, TSquare> current) => current.Success).ToList();

            int preserveCutoff = (int)(pairs.Count * preservePercent / 100);
            int randomizeCutoff = (int)(pairs.Count * (100 - randomizePercent) / 100);
            //for (int i = 0; i < preserveCutoff; i++)
            //{
            //}
            for (int i = preserveCutoff; i < randomizeCutoff; i++)
            {
                int parent = random.Next(0, preserveCutoff);
                pairs[i].Net.Cross(pairs[parent].Net, random);
                pairs[i].Net.Mutate(random, mutationMin, mutationMax);
            }
            for (int i = randomizeCutoff; i < pairs.Count; i++)
            {
                pairs[i].Net.Randomize(random, randomizeMin, randomizeMax);
            }
            return pairs[0].Net;
        }
    }
}
