using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;

namespace GameOfLife
{
    public class LifeData
    {
        private bool[,] States = new bool[20, 20];
        private bool[,] NextStates = new bool[20, 20];
        private bool[] Values = new bool[400];
        public LifeData()
        {
                
        }

        public void MakeTurn()
        {
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    int aliveCount = GetInfo(i, j);
                    GetNextPositions(i, j, aliveCount);
                }
            }
            
        }

        public void PaintButtons(Button[,] buttons)
        {
            int k = 0;
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    States[i, j] = NextStates[i, j];
                    Values[k++] = States[i, j];
                    buttons[i, j].Background = States[i, j] == true ? Brushes.Black : Brushes.White;
                }
            }
        }

        public void ReverseState(Button thisButton, int y, int x)
        {
            thisButton.Background = thisButton.Background == Brushes.Black ? Brushes.White : Brushes.Black;
            States[y, x] = thisButton.Background == Brushes.Black ? true : false;
        }
        private int GetInfo(int y, int x)
        {
            int aliveCount = 0;
            for (int i = y - 1; i < y + 2; i++)
            {
                for (int j = x - 1; j < x + 2; j++)
                {
                    if (i == y && j == x)
                        continue;
                    int posXCopy = j == -1 ? 19 : j == 20 ? 0 : j;
                    int posYCopy = i == -1 ? 19 : i == 20 ? 0 : i;
                    if (States[posYCopy, posXCopy])
                        aliveCount++;
                }
            }
            return aliveCount;
        }
        private void GetNextPositions(int y, int x, int aliveCount)
        {
            if (States[y, x])
            {
                if (aliveCount > 3 || aliveCount < 2)
                    NextStates[y, x] = false;
                else
                    NextStates[y, x] = true;
            }
            else
            {
                if (aliveCount == 3)
                    NextStates[y, x] = true;
                else
                    NextStates[y, x] = false;
            }
        }
        public void SaveInBMP()
        {
            int height = 20; //Пока что фиксированное значение
            int width = 20; //Тоже самое
            int stride = width * 4;
            byte[] bits = new byte[height * stride];
            for (int i = 0; i < bits.Length - 3; i += 4) 
            {
                if (States[(i / 4) / 20, (i / 4) % 20]) 
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