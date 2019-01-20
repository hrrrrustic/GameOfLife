using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.ComponentModel;
using System;
using System.Windows.Threading;
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
                    Button buttons = new Button();
                    buttons.Width = 10;
                    buttons.Height = 10;
                    buttons.Uid = $"{i},{j}";
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
            //Если после смерти всех добавить новых и нажать старт - работать это не будет
            //TODO: добавь проверку, что поле изменяется за шаг
            while (_lifeData.isSomeoneAlive)
            {
                _lifeData.MakeTurn();
                _lifeData.PaintButtons(_dots);
                map.Refresh();
                Thread.Sleep(15);
            }
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _lifeData.SaveInBMP();           
        }
    }
    public static class ExtensionMethods
    {
        private static Action EmptyDelegate = delegate () { };


        public static void Refresh(this UIElement uiElement)

        {
            uiElement.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
        }
    }
}
