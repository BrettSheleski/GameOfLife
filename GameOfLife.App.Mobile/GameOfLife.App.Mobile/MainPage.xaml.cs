using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GameOfLife.App.Mobile
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();

            this.Life = new GameOfLife.Life(20, 20);

            

            for (int h = 0; h < Life.Height; ++h)
            {
                TheGrid.RowDefinitions.Add(new RowDefinition());
            }
            for (int w = 0; w < Life.Width; ++w)
            {
                TheGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }


            BoxView b;

            BoolToColorConverter converter = new BoolToColorConverter()
            {
                TrueColor = Color.Black
            };


            for (int h = 0; h < Life.Height; ++h)
            {
                for (int w = 0; w < Life.Width; ++w)
                {
                    b = new BoxView();

                    Grid.SetRow(b, h);
                    Grid.SetColumn(b, w);

                    b.BindingContext = Life.Cells[h][w];
                    b.GestureRecognizers.Add(new TapGestureRecognizer()
                    {
                        Command = new Command<Cell>(ToggleCell),
                        CommandParameter = Life.Cells[h][w]
                    });


                    b.SetBinding(BoxView.BackgroundColorProperty, new Binding("IsAlive", BindingMode.OneWay, converter));

                    TheGrid.Children.Add(b);
                }
            }
        }


        private void ToggleCell(Cell cell)
        {
            cell.IsAlive = !cell.IsAlive;
        }

        public Life Life { get; }


        private bool _isRunning = false;

        public void Start()
        {
            if (!_isRunning)
            {
                _isRunning = true;

                Xamarin.Forms.Device.StartTimer(TimeSpan.FromMilliseconds(100), UpdateBoard);
            }
        }

        bool UpdateBoard()
        {
            Life.Update();

            return _isRunning;
        }

        public void Pause()
        {
            _isRunning = false;
        }

        private void StartButton_Clicked(object sender, EventArgs e)
        {
            Start();
        }

        private void PauseButton_Clicked(object sender, EventArgs e)
        {
            Pause();
        }
    }
}
