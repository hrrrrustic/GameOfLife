using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.ComponentModel;
using System.Threading;

namespace GameOfLife
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>  
    public partial class MainWindow : Window
    {
        private LifeData _lifeData;
        private static Button[,] _dots;
        public MainWindow()
        {
            InitializeComponent();

        }
        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mode = (MenuItem)sender;
            int field = int.Parse(mode.Name.Remove(0, 1));
            _lifeData = new LifeData(field);
            _dots = new Button[field, field];
            map.Width = field * 10;
            map.Height = field * 10;
            for (int i = 0; i < field; i++)
            {
                for (int j = 0; j < field; j++)
                {
                    Button buttons = new Button {Width = 10, Height = 10, Uid = $"{i},{j}"};
                    buttons.Click += buttons_Click;
                    buttons.Background = Brushes.White;
                    _dots[i, j] = buttons;
                    map.Children.Add(buttons);
                }
            }
        }
        private void buttons_Click(object sender, RoutedEventArgs e)
        {
            Button thisButton = (Button)sender;
            int posY = int.Parse(thisButton.Uid.Split(',').First());
            int posX = int.Parse(thisButton.Uid.Split(',').Last());
            _lifeData.ReverseState(thisButton, posY, posX);
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += Execute;
            worker.RunWorkerAsync();
        }

        private void Execute(object sender, DoWorkEventArgs e)
        {
            do
            {
                _lifeData.MakeTurn();
                Application.Current.Dispatcher.Invoke(() => { _lifeData.PaintButtons(_dots); });
                Thread.Sleep(100);
            } while (_lifeData.LiveCount != 0);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _lifeData.SaveInBmp();           
        }
    }
}
