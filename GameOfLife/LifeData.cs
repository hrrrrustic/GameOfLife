using System.Windows.Controls;

namespace GameOfLife
{
    public class LifeData
    {
        public bool[,] States = new bool[20, 20];
        public LifeData()
        {
            
        }

        public void MakeTurn()
        {
            //TODO: implement logic
        }

        public void PaintButtons(Button[,] buttons)
        {
            //TODO: implement logic
        }

        public void ReverseState(int x, int y)
        {
            //TODO: implement logic
        }
    }
}