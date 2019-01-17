using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;

namespace GameOfLife
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>  
    public partial class MainWindow : Window
    {
        private LifeData _lifeData;
        public MainWindow()
        {
            InitializeComponent();
            _lifeData = new LifeData();

        }
        private static Button[,] dots = new Button[20, 20];
        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    Button buttons = new Button();
                    buttons.Width = 10;
                    buttons.Height = 10;
                    buttons.Uid =  $"{i},{j}";
                    buttons.Click += buttons_Click;
                    buttons.Background = Brushes.White;
                    dots[i, j] = buttons;
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
                _lifeData.MakeTurn();
                _lifeData.PaintButtons(dots);

        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _lifeData.SaveInBMP();        
        }
    }
}
