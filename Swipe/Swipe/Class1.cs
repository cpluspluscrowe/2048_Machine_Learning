using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swipe
{
    public class Direction
    {
        protected Tuple<int, int> Up;
        protected Tuple<int, int> Right;
        protected Tuple<int, int> Down;
        protected Tuple<int, int> Left;
        protected Tuple<int, int> SwipeDirection;

        public Direction()
        {
            Up = new Tuple<int, int>(1, 0);
            Right = new Tuple<int, int>(0, -1);
            Down = new Tuple<int, int>(-1, 0);
            Left = new Tuple<int, int>(0, 1);
            this.SetSwipe(this.Up);//filler
        }

        public string GetDirectionString()
        {
            if (this.SwipeDirection == this.GetUp())
            {
                return "Up";
            }
            else if (this.SwipeDirection == this.GetRight())
            {
                return "Right";
            }
            else if (this.SwipeDirection == this.GetDown())
            {
                return "Down";
            }
            else if (this.SwipeDirection == this.GetLeft())
            {
                return "Left";
            }
            else
            {
                return "No Direction";
            }
        }

        public void SetSwipe(Tuple<int, int> direction)
        {
            this.SwipeDirection = direction;
        }
        public void SetSwipe(Direction direction)
        {
            this.SwipeDirection = direction.SwipeDirection;
        }
        public void SetSwipe(string direction)
        {
            if (direction == "Up")
            {
                SetSwipe(this.GetUp());
            }
            else if (direction == "Right")
            {
                SetSwipe(this.GetRight());
            }
            else if (direction == "Down")
            {
                SetSwipe(this.GetDown());
            }
            else if (direction == "Left")
            {
                SetSwipe(this.GetLeft());
            }
        }
        public Tuple<int, int> GetSwipe()
        {
            return this.SwipeDirection;
        }

        public Tuple<int, int> GetRight()
        {
            return this.Right;
        }

        public Tuple<int, int> GetUp()
        {
            return this.Up;
        }

        public Tuple<int, int> GetDown()
        {
            return this.Down;
        }

        public Tuple<int, int> GetLeft()
        {
            return this.Left;
        }
        public Direction GetNextSwipe()
        {
            Direction direction = new Direction();
            Random r = new Random();
            int randomInt = r.Next(0, 4);
            if (randomInt == 0)
            {
                direction.SetSwipe(this.GetUp());
            }
            else if (randomInt == 1)
            {
                direction.SetSwipe(this.GetRight());
            }
            else if (randomInt == 2)
            {
                direction.SetSwipe(this.GetDown());
            }
            else if (randomInt == 3)
            {
                direction.SetSwipe(this.GetLeft());
            }
            return direction;
        }
    }
}
