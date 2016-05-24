using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH5_autoexec
{
    class PixelColor
    {
        public const uint SpecialColorForDetectingChange = 0x01234567;
        public const uint SpecialColorForDetectingUnchange = 0xFEDCBA98;
        public const uint SpecialColorForPredicator = 0x13579BDF;

        public int X { get; private set; }

        public int Y { get; private set; }

        public uint Color { get; private set; }

        public PixelColor(int x, int y, uint color)
        {
            X = x;
            Y = y;
            Color = color;
        }
    }
}
