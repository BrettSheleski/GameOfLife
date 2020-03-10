using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace GameOfLife
{
    public class Life
    {
        public Life(int width, int height)
        {
            Width = width;
            Height = height;

            Cells = new Cell[width][];

            for (int r = 0; r < height; ++r)
            {
                Cells[r] = new Cell[height];

                for (int c = 0; c < width; ++c)
                {
                    Cells[r][c] = new Cell();
                }
            }

            int lastH = height - 1;
            int lastW = width - 1;

            for (int r = 0; r < height; ++r)
            {
                for (int c = 0; c < width; ++c)
                {
                    if (r > 0)
                    {
                        if (c > 0)
                            Cells[r][c].TopLeft = Cells[r - 1][c - 1];

                        Cells[r][c].TopCenter = Cells[r - 1][c];

                        if (c < lastW)
                            Cells[r][c].TopRight = Cells[r - 1][c + 1];
                    }


                    if (c < lastW)
                        Cells[r][c].Right = Cells[r][c + 1];

                    if (c > 0)
                        Cells[r][c].Left = Cells[r][c - 1];

                    if (r < lastH)
                    {
                        if (c > 0)
                            Cells[r][c].BottomRight = Cells[r + 1][c - 1];

                        Cells[r][c].BottomCenter = Cells[r + 1][c];

                        if (c < lastW)
                            Cells[r][c].BottomLeft = Cells[r + 1][c + 1];
                    }
                }
            }
        }

        public int Width { get; }
        public int Height { get; }

        public Cell[][] Cells { get; }

        public void Update()
        {
            foreach (var cell in AllCells)
            {
                cell.GetNextValue();
            }

            foreach (var cell in AllCells)
            {
                cell.Update();
            }
        }

        IEnumerable<Cell> AllCells
        {
            get
            {
                return Cells.SelectMany(c => c);
            }
        }

        public async Task UpdateAsync()
        {
            await Task.WhenAll(AllCells.Select(c => Task.Factory.StartNew(c.GetNextValue)));

            foreach (var cell in AllCells)
            {
                cell.IsAlive = cell._nextValue;
            }
        }
    }

    public class Observable : INotifyPropertyChanged, INotifyPropertyChanging
    {
        public event PropertyChangingEventHandler PropertyChanging;
        public event PropertyChangedEventHandler PropertyChanged;

        public delegate void PropertyChangedCallback<T>(T oldValue, T newValue);

        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        void OnPropertyChanging([CallerMemberName] string propertyName = null)
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }

        protected void Set<T>(ref T field, T value, [CallerMemberName]string propertyName = null)
        {
            PropertyChangedCallback<T> callback = null;

            Set(ref field, value, callback, propertyName);
        }

        protected void Set<T>(ref T field, T value, Action callback, [CallerMemberName]string propertyName = null)
        {
            Set(ref field, value, (oldValue, newValue) => callback(), propertyName);
        }

        protected void Set<T>(ref T field, T value, Action<T> callback, [CallerMemberName]string propertyName = null)
        {
            Set(ref field, value, (oldValue, newValue) => callback(newValue), propertyName);
        }

        protected void Set<T>(ref T field, T value, PropertyChangedCallback<T> callback, [CallerMemberName]string propertyName = null)
        {
            if (!System.Collections.Generic.EqualityComparer<T>.Default.Equals(field, value))
            {
                OnPropertyChanging(propertyName);

                T oldValue = field;

                field = value;

                OnPropertyChanged(propertyName);

                callback?.Invoke(oldValue, value);
            }
        }
    }
}