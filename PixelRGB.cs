﻿using System;

namespace DinosaurGraphics
{
    public class PixelRGB
    {
        public byte R;
        public byte G;
        public byte B;
        public byte I;

        /*
         *  Constructor without parametrs
         */
        public PixelRGB()
        {
            R = 0;
            G = 0;
            B = 0;
            I = 0;
        }
        /*
         *  Constructor with paramters r, g, b.
         */
        public PixelRGB(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
            I = (byte)Math.Round(0.0722f * b + 0.715f * g + 0.212f * r);
        }
    }
}
