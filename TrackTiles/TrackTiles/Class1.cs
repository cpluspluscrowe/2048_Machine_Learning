using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameBoard;
using Swipe;

namespace TrackTiles
{
    public class BoardConfiguration
    {
        protected List<int> Configuration;
        public BoardConfiguration(Board board)
        {
            Configuration = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Configuration.Add(board.Table[i,j]);
                }
            }
        }

        public List<int> GetBoardConfiguration()
        {
            return this.Configuration;
        }
    }
    public class TrainSwipeData
    {
        protected BoardConfiguration Configuration;
        protected int StartScore;
        protected int TurnsPastStartScore;
        protected int CurrentScore;
        protected int ScoreDelta;
        protected string ClassifierString;
        protected double Hashing;
        public TrainSwipeData(Board board)
        {
            Configuration = new BoardConfiguration(board);
            StartScore = board.Score;
            CurrentScore = board.Score;
            TurnsPastStartScore = 0;
            ClassifierString = "";
            Hashing = HashBoard();
        }

        public BoardConfiguration GetConfiguration()
        {
            return Configuration;
        }
        public double GetHash()
        {
            return this.Hashing;
        }
        protected double HashBoard()
        {
            double val = 0;
            int power = 0;
            foreach (var tile in this.Configuration.GetBoardConfiguration())
            {
                val += Math.Pow(tile, power);
                power += 1;
            }
            return val;
        }
        public void IncrementTurn(int newScore,string direction)
        {
            this.TurnsPastStartScore += 1;
            CurrentScore = newScore;
            ScoreDelta = this.CurrentScore - this.StartScore;
            ClassifierString += direction;
        }

        public string GetClassifierString()
        {
            return this.ClassifierString;
        }
        public int GetTurnCount()
        {
            return TurnsPastStartScore;
        }

        public int GetScoreDelta()
        {
            return this.ScoreDelta;
        }
    }
    public class Tracker
    {
        protected List<TrainSwipeData> TrainData;
        protected List<TrainSwipeData> FinishedData;
        public int HowFarToTrack;
        public Tracker(int numberOfTurnsToTrack)
        {
            this.HowFarToTrack = numberOfTurnsToTrack;
            TrainData = new List<TrainSwipeData>();
            FinishedData = new List<TrainSwipeData>();
        }

        public void AddData(Board board,string direction)
        {
            TrainSwipeData trainData = new TrainSwipeData(board);
            TrainData.Add(trainData);
            IncrementTurn(board.Score,direction);
        }

        public void TransferDataToFinished()
        {
            foreach (var swipeData in TrainData)
            {
                FinishedData.Add(swipeData);
            }
            TrainData = new List<TrainSwipeData>();
        }

        public List<TrainSwipeData> GetTrainData()
        {
            return FinishedData;
        }
        protected void IncrementTurn(int newScore, string direction)
        {
            List<int> RemoveAt= new List<int>();
            int cnter = 0;
            foreach (var swipeData in TrainData)
            {
                if (swipeData.GetTurnCount() < HowFarToTrack)
                {
                    swipeData.IncrementTurn(newScore,direction);
                }
                else
                {
                    FinishedData.Add(swipeData);
                    RemoveAt.Add(cnter);
                }
                cnter += 1;
            }
            foreach (var removeAt in RemoveAt)
            {
                TrainData.RemoveAt(removeAt);
            }
        }
    }
}
