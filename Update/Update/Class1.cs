using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameBoard;
using RandomTileAdder;
using Swipe;

namespace Update
{
    public interface IUpdater
    {
        void Update(Board board, Direction direction, TileAdder adder);
        bool DidUpdateChangeBoard();
    }
    public enum Plane
    {
        Vertical,
        Horizontal
    }
    public class Updater : IUpdater
    {
        protected Board Board;
        protected int UpIncrement;
        protected int RightIncrement;
        protected int StartRow;
        protected int StartColumn;
        protected Plane MovementPlane;
        protected TileAdder TileAdder;
        protected bool BoardChanged = false;
        public Updater(Board board, Direction direction, TileAdder adder)
        {
            SetConfiguration(board,direction,adder);
        }
        protected void SetConfiguration(Board board, Direction direction, TileAdder adder)
        {
            this.Board = board;
            this.TileAdder = adder;
            this.BoardChanged = false;
            Tuple<int, int> swipe = direction.GetSwipe();
            if (swipe.Equals(direction.GetUp()))
            {
                this.StartRow = 0;
                this.StartColumn = 0;
                this.UpIncrement = swipe.Item1;
                this.RightIncrement = 1;
                this.MovementPlane = Plane.Vertical;
            }
            else if (swipe.Equals(direction.GetRight()))
            {
                this.StartRow = 0;
                this.StartColumn = 3;
                this.UpIncrement = 1;
                this.RightIncrement = swipe.Item2;
                this.MovementPlane = Plane.Horizontal;
            }
            else if (swipe.Equals(direction.GetDown()))
            {
                this.StartRow = 3;
                this.StartColumn = 0;
                this.UpIncrement = swipe.Item1;
                this.RightIncrement = 1;
                this.MovementPlane = Plane.Vertical;
            }
            else if (swipe.Equals(direction.GetLeft()))
            {
                this.StartRow = 0;
                this.StartColumn = 0;
                this.UpIncrement = 1;
                this.RightIncrement = swipe.Item2;
                this.MovementPlane = Plane.Horizontal;
            }
        }
        public void Update(Board board, Direction direction, TileAdder adder)
        {
            SetConfiguration(board, direction, adder);
            this.BoardChanged = false;
            foreach (Tuple<int,int> tile in GetNextElementToUpdate())
            {
                TryToMoveTile(tile);
            }
            this.TileAdder.AddTile();
        }

        protected void TryToMoveTile(Tuple<int, int> position)
        {
            bool moving = true;
            while (moving)
            {
                Tuple<int, int> nextPosition = GetNextPosition(position);
                if (IsNextPositionValid(nextPosition))
                {
                    bool isFilled = IsNextPositionFilled(nextPosition);
                    if (!isFilled)
                    {
                        position = Move(position, nextPosition);
                        this.BoardChanged = true;
                    }
                    else
                    {
                        if (CanCombine(position, nextPosition))
                        {
                            position = Combine(position, nextPosition);
                            this.Board.Score += this.Board.Table[position.Item1, position.Item2];
                            this.BoardChanged = true;
                        }
                        else
                        {
                            moving = false;
                        }
                    }
                }
                else
                {
                    moving = false;
                }  
            }
        }

        public bool DidUpdateChangeBoard()
        {
            return this.BoardChanged;
        }
        protected System.Collections.Generic.IEnumerable<Tuple<int, int>> GetNextElementToUpdate()
        {
            for (int i = this.StartRow; i > -1 && i < 4; i += this.UpIncrement)
            {
                for (int j = this.StartColumn; j > -1 && j < 4; j += this.RightIncrement)
                {
                    yield return new Tuple<int, int>(i, j);
                }
            }
        }

        protected Tuple<int,int> Combine(Tuple<int, int> position, Tuple<int, int> nextPosition)
        {
            this.Board.Table[nextPosition.Item1, nextPosition.Item2] *= 2;
            this.Board.Table[position.Item1, position.Item2] = 0;
            return nextPosition;
        }
        protected bool CanCombine(Tuple<int, int> position, Tuple<int, int> nextPosition)
        {
            return this.Board.Table[position.Item1, position.Item2] == this.Board.Table[nextPosition.Item1, nextPosition.Item2];
        }
        protected bool IsNextPositionFilled(Tuple<int, int> nextPosition)
        {
            return this.Board.Table[nextPosition.Item1, nextPosition.Item2] != 0;
        }
        protected Tuple<int,int> GetNextPosition(Tuple<int,int> position)
        {
            int nextRow;
            int nextColumn;
            if (this.MovementPlane == Plane.Vertical)
            {
                nextRow = position.Item1 - this.UpIncrement;
                nextColumn = position.Item2;
            }
            else
            {
                nextColumn = position.Item2 - this.RightIncrement;
                nextRow = position.Item1;
            }
            return new Tuple<int,int>(nextRow,nextColumn);
        }

        protected bool IsNextPositionValid(Tuple<int,int> nextPosition)
        {
            int nextRow = nextPosition.Item1;
            int nextColumn = nextPosition.Item2;
            if (!(nextRow > -1 && nextRow < 4))
            {
                return false;
            }
            if (!(nextColumn > -1 && nextColumn < 4))
            {
                return false;
            }
            return true;
        }
        protected Tuple<int,int> Move(Tuple<int,int> from,Tuple<int,int> to)
        {
            this.Board.Table[to.Item1, to.Item2] = this.Board.Table[from.Item1, from.Item2];
            this.Board.Table[from.Item1, from.Item2] = 0;
            return to;
        }
    }
}
