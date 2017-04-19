using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Accord.MachineLearning.DecisionTrees;
using GameBoard;
using Swipe;
using Update;
using RandomTileAdder;
using Checker;
using Accord.MachineLearning.DecisionTrees.Learning;
using TrackTiles;
using Turns;
using DatabaseUploader;
using BasicDecisionTree;

namespace _2048_console
{
    class Main2048Console
    {
        static void Main(string[] args)
        {
            //DecisionTreeModel model = new DecisionTreeModel();
            //model.TrainModel();
            Train();
        }

        static void Train(DecisionTreeModel model = null)
        {
            for (int i = 0; i <= 10; i++)
            {
                Board board = new Board();
                Direction direction = new Direction();
                TileAdder adder = new TileAdder(board);
                IUpdater updater = new Updater(board, direction, adder);
                GameOverChecker checker = new GameOverChecker(board, direction, updater);
                Tracker tracker = new Tracker(15);
                TurnManager manager = new TurnManager(direction, board, updater, adder, checker, tracker,model);
                manager.Play();
                Uploader uploader = new Uploader();
                uploader.Upload(manager.GetTracker().GetTrainData());
            }
        }
    }
}
/*
            foreach (var f in manager.GetTracker().GetTrainData())
            {
                Console.WriteLine(f.GetScoreDelta().ToString() + "   " + f.GetTurnCount());
                Console.WriteLine(f.GetClassifierString());
                Console.WriteLine();
            }
*/