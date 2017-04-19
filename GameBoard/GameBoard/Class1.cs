using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBoard
{
    [Serializable]
    public class Board
    {
        public int[,] Table;
        public int Score;
        public Board()
        {
            this.Score = 0;
            List<int> row;
            this.Table = new int[4, 4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    this.Table[i, j] = 0;
                }
            }
            this.Table[0, 0] = 2;
            this.Table[0, 1] = 4;
            this.Table[1, 0] = 2;
            this.Table[0, 3] = 8;
        }

        public bool IsTileEmpty(int i, int j)
        {
            return this.Table[i, j] == 0;
        }
        public void Print()
        {
            string rowString = "";
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    rowString +=  this.Table[i, j] + " ";
                }
                rowString += "\n";
            }
            Debug.Print(rowString);
            Console.WriteLine(rowString);
        }
    }
}