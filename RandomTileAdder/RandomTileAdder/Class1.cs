using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameBoard;

namespace RandomTileAdder
{
    public class TileAdder
    {
        protected Board Board;
        public TileAdder(Board board)
        {
            this.Board = board;
        }

        public void AddTile()
        {
            if (!this.IsBoardFull())
            {
                bool addedSuccessfully = false;
                Random r = new Random();
                while (!addedSuccessfully)
                {
                    int row = r.Next(0, 4);
                    int column = r.Next(0, 4);
                    if (this.Board.IsTileEmpty(row, column))
                    {
                        this.Board.Table[row, column] = 2;
                        addedSuccessfully = true;
                    }
                }
            }
        }

        protected bool IsBoardFull()
        {
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
            return true;
        }
    }
}
