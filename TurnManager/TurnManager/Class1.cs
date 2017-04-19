using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Checker;
using GameBoard;
using RandomTileAdder;
using Swipe;
using Update;
using TrackTiles;
using BasicDecisionTree;

namespace Turns
{
    public class TurnManager
    {
        protected Board GameBoard;
        protected Direction Swiper;
        protected IUpdater Updater;
        protected TileAdder Adder;
        protected GameOverChecker GameOverChecker;
        protected Tracker Tracker;
        protected DecisionTreeModel Model;
        public TurnManager(Direction direction,Board board,IUpdater updater,TileAdder adder,GameOverChecker gameOverChecker,Tracker tracker,DecisionTreeModel model)
        {
            this.GameBoard = board;
            this.Swiper = direction;
            this.Updater = updater;
            this.Adder = adder;
            this.GameOverChecker = gameOverChecker;
            this.Tracker = tracker;
            this.Model = model;
        }
        public void Play()
        {
            while (NextTurn())
            {
                
            }
            Console.WriteLine(GameBoard.Score);
        }

        public Tracker GetTracker()
        {
            return Tracker;
        }
        protected bool NextTurn()
        {
            if (Model != null)
            {
                string prediction = Model.Predict(GameBoard);
                Swiper.SetSwipe(prediction);
            }
            else
            {
                Swiper.SetSwipe(Swiper.GetNextSwipe());
            }
            Updater.Update(GameBoard, Swiper, Adder);
            Tracker.AddData(GameBoard,Swiper.GetDirectionString());
            if (GameOverChecker.IsGameOver(GameBoard, Swiper, Updater) == true)
            {
                Tracker.TransferDataToFinished();
                return false;
            }
            return true;
        }
    }
}
