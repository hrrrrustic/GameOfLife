using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.ComponentModel;
using System.Threading;

namespace GameOfLife
{
    public class LifeData
    {
        private bool[,] _states;
        private bool[,] _nextStates;
        readonly int _field;
        public bool isSomeoneAlive;
        public LifeData(int field)
        {
            _states = new bool[field, field];
            _nextStates = new bool[field, field];
            _field = field;
            isSomeoneAlive = true;
        }

        public void MakeTurn()
        {
            isSomeoneAlive = false;
            for (int i = 0; i < _field; i++)
            {
                for (int j = 0; j < _field; j++)
                {
                    int aliveCount = GetInfo(i, j);
                    GetNextPositions(i, j, aliveCount);
                }
            }
        }

        public void PaintButtons(Button[,] buttons)
        {
            for (int i = 0; i < _field; i++)
            {
                for (int j = 0; j < _field; j++)
                {
                    _states[i, j] = _nextStates[i, j];
                    buttons[i, j].Background = _states[i, j] == true ? Brushes.Black : Brushes.White;
                }
            }
        }

        public void ReverseState(Button thisButton, int y, int x)
        {
            thisButton.Background = thisButton.Background == Brushes.Black ? Brushes.White : Brushes.Black;
            _states[y, x] = thisButton.Background == Brushes.Black ? true : false;
        }
        private int GetInfo(int y, int x)
        {
            int xBorder = _field;
            int yBorder = _field;
            int aliveCount = 0;
            for (int i = y - 1; i < y + 2; i++)
            {
                for (int j = x - 1; j < x + 2; j++)
                {
                    if (i == y && j == x)
                        continue;
                    int posXCopy = j == -1 ? xBorder - 1 : j == xBorder ? 0 : j;
                    int posYCopy = i == -1 ? yBorder - 1 : i == yBorder ? 0 : i;
                    if (_states[posYCopy, posXCopy])
                        aliveCount++;
                }
            }
            return aliveCount;
        }
        private void GetNextPositions(int y, int x, int aliveCount)
        {
            if (_states[y, x])
            {
                if (aliveCount > 3 || aliveCount < 2)
                    _nextStates[y, x] = false;
                else
                {
                    _nextStates[y, x] = true;
                    isSomeoneAlive = true;
                }
            }
            else
            {
                if (aliveCount == 3)
                {
                    _nextStates[y, x] = true;
                    isSomeoneAlive = true;
                }
                else
                    _nextStates[y, x] = false;
            }
        }
        public void SaveInBMP()
        {
            int height = _field; 
            int width = _field; 
            int stride = width * 4;
            byte[] bits = new byte[height * stride];
            for (int i = 0; i < bits.Length - 3; i += 4) 
            {
                if (_states[(i / 4) / _field , (i / 4) % _field]) 
                {
                    bits[i] = 0;
                    bits[i + 1] = 0;
                    bits[i + 2] = 0;
                }
                else
                {
                    bits[i] = 255;
                    bits[i + 1] = 255;
                    bits[i + 2] = 255;
                }
                bits[i + 3] = 255;
            }
            PixelFormat format =  PixelFormats.Pbgra32;
            int bytesPerPixel = (format.BitsPerPixel + 7) / 8;
            BitmapSource bitmap = BitmapSource.Create(width, height, 96, 96, format, null, bits, stride);
            BmpBitmapEncoder encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));
            using (var filestream = new FileStream("output.bmp", FileMode.Create))
            {
                encoder.Save(filestream);
            }
        }
    }
}