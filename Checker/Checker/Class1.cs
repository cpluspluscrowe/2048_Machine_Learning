using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using GameBoard;
using RandomTileAdder;
using Swipe;
using Update;

namespace Checker
{
    public static class ExtensionMethods
    {
        // Deep clone
        public static T DeepClone<T>(this T a)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, a);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
        }
    }
    public class GameOverChecker
    {
        protected Board Board;
        protected Board MutableBoard;
        protected Direction Direction;
        protected IUpdater PreviousUpdate;
        public GameOverChecker(Board board,Direction direction,IUpdater lastUpdate)
        {
            this.Board = board;
            this.Direction = direction;
            this.PreviousUpdate = lastUpdate;
        }

        public bool IsGameOver(Board board, Direction direction, IUpdater lastUpdate)
        {
            this.Board = board;
            this.Direction = direction;
            this.PreviousUpdate = lastUpdate;
            if (this.PreviousUpdate.DidUpdateChangeBoard() == true)
            {
                return false;
            }
            for (int i = 0; i <= 3; i++)
            {
                for (int j = 0; j <= 3; j++)
                {
                    if (this.Board.IsTileEmpty(i, j))
                    {
                        return false;
                    }
                }   
            }
            this.MutableBoard = this.Board.DeepClone();
            List<Tuple<int,int>> swipeableDirection = new List<Tuple<int, int>>(){Direction.GetUp(),Direction.GetRight(),Direction.GetDown(),Direction.GetLeft()};
            foreach (var swipeDirection in swipeableDirection)
            {
                Direction.SetSwipe(swipeDirection);
                string directionString = Direction.GetDirectionString();
                Updater updater = new Updater(this.MutableBoard,Direction,new TileAdder(this.MutableBoard));
                updater.Update(this.MutableBoard, Direction, new TileAdder(this.MutableBoard));
                if (updater.DidUpdateChangeBoard())
                {
                    return false;
                }
            }
            return true; //No swipes change the board, and the board is full
        }
    }
}
