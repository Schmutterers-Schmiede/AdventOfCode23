using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode23.Day16
{
    public class Beam
    {
        public int PosX { get; set; }
        public int PosY { get; set; }
        public int XDirection { get; set; }
        public int YDirection { get; set; }

        public Beam(int posX, int posY, int xDirection, int yDirection)
        {
            PosX = posX;
            PosY = posY;
            XDirection = xDirection;
            YDirection = yDirection;
        }
    }
}
