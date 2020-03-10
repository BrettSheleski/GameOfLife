using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameOfLife
{
    public class Cell : Observable
    {

        Cell _topLeft, _topCenter, _topRight,
            _right, _bottomRight, _bottomCenter,
            _bottomLeft, _left;


        private bool _isAlive;

        internal bool _nextValue;

        public Cell TopLeft { get => _topLeft; set => Set(ref _topLeft, value); }
        public Cell TopCenter { get => _topCenter; set => Set(ref _topCenter, value); }
        public Cell TopRight { get => _topRight; set => Set(ref _topRight, value); }
        public Cell Right { get => _right; set => Set(ref _right, value); }
        public Cell BottomRight { get => _bottomRight; set => Set(ref _bottomRight, value); }
        public Cell BottomCenter { get => _bottomCenter; set => Set(ref _bottomCenter, value); }
        public Cell BottomLeft { get => _bottomLeft; set => Set(ref _bottomLeft, value); }
        public Cell Left { get => _left; set => Set(ref _left, value); }
        public bool IsAlive { get => _isAlive; set => Set(ref _isAlive, value); }

        IEnumerable<Cell> GetNeighbors()
        {
            if (TopLeft != null)
                yield return TopLeft;

            if (TopCenter != null)
                yield return TopCenter;

            if (TopRight != null)
                yield return TopRight;

            if (Right != null)
                yield return Right;

            if (BottomRight != null)
                yield return BottomRight;

            if (BottomCenter != null)
                yield return BottomCenter;

            if (BottomLeft != null)
                yield return BottomLeft;

            if (Left != null)
                yield return Left;
        }


        protected internal virtual void GetNextValue()
        {
            int aliveNeighborCount = GetNeighbors().Where(x => x.IsAlive).Count();

            if (IsAlive)
            {
                _nextValue = aliveNeighborCount == 2 || aliveNeighborCount == 3;
            }
            else
            {
                _nextValue = aliveNeighborCount == 3;
            }
        }

        public void Update()
        {
            IsAlive = _nextValue;
        }
    }
}
