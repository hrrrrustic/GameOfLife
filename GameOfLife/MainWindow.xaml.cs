using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;

namespace GameOfLife
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        static Button[,] dots = new Button[20, 20];
        static Brush[,] dotsNext = new Brush[20, 20];
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            const int width = 200;
            const int height = 200;
            for (int i = 0; i < height/10; i++)
            {
                for (int j = 0; j < width/10; j++)
                {
                    Button buttons = new Button();
                    buttons.Width = 10;
                    buttons.Uid =  $"{j.ToString()},{i.ToString()}";
                    buttons.Height = 10;
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
            thisButton.Background = thisButton.Background == Brushes.Black ? Brushes.White : Brushes.Black;
            int posX = int.Parse(thisButton.Uid.Split(',').First());
            int posY = int.Parse(thisButton.Uid.Split(',').Last());
            dots[posY, posX].Background = thisButton.Background;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    int[] info = GetInfo(dots[i, j]);
                    if (dots[i, j].Background == Brushes.White)
                    {
                        AddDots(info[0], info[1], info[2]);
                    }
                    else
                    {
                        DeleteDots(info[0], info[1], info[2]);
                    }
                }
            }
            Refreshing();
        }

        private int[] GetInfo(Button item)
        {
            int aliveCount = 0;
            int posX = int.Parse(item.Uid.Split(',').First());
            int posY = int.Parse(item.Uid.Split(',').Last());
            for (int i = posY - 1; i < posY + 2; i++)
            {
                for (int j = posX - 1; j < posX + 2; j++)
                {
                    int posXCopy = j == -1 ? 19 : j == 20 ? 0 : j;
                    int posYCopy = i == -1 ? 19 : i == 20 ? 0 : i;
                    if (dots[posYCopy, posXCopy].Background == Brushes.Black)
                        aliveCount++;
                }
            }
            int[] info = new int[3];
            info[0] = posX;
            info[1] = posY;
            info[2] = aliveCount;
            return info;
        }
        private void AddDots(int x, int y, int aliveCount)
        {

            if (aliveCount == 3)
            {
                dotsNext[y, x] = Brushes.Black;
            }
            else
            {
                dotsNext[y, x] = Brushes.White;
            }
        }
        private void DeleteDots(int x, int y, int aliveCount)
        {
            if (aliveCount > 3 || aliveCount < 2)
            {
                dotsNext[y, x] = Brushes.White;
            }
            else
            {
                dotsNext[y, x] = Brushes.Black;
            }
        }
        private void Refreshing()
        {
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    dots[i, j].Background = dotsNext[i, j];
                }
            }
        }

        //private void ClearField()
        //{
        //    for (int i = 0; i < 20; i++)
        //    {
        //       for (int j = 0; j < 20; j++)
        //        {
        //            dots[i, j].Background = Brushes.White;
        //        }
        //    }
        //    canChangeAfterAdd.Clear();
        //   
        //}
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
           // ClearField();
        }
    }
}
