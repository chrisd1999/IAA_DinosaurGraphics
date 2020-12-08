using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kristers_Dugels_181RDB024.helpers
{
    public static class BoxFilterHelper
    {
        static PixelRGB[,] BoxFilterImpl(PixelRGB[,] src, PixelRGB[,] dest) {

            int width = dest.GetLength(0);
            int height = dest.GetLength(1);

            for(int x = 1; x < width - 1; x++){
                for(int y = 1; y < height - 1; y++) {

                    int r = 0;
                    int g = 0;
                    int b = 0;
                    int i = 0;

                    for(int fi = 0; fi < 3; fi++) {
                        for (int fj = 0; fj < 3; fj++) {
                            r += src[x + fi - 1, y + fj - 1].R;
                            g += src[x + fi - 1, y + fj - 1].G;
                            b += src[x + fi - 1, y + fj - 1].B;
                            i += src[x + fi - 1, y + fj - 1].I;
                        }
                    }

                    dest[x, y].R = (byte)(r / 9);
                    dest[x, y].G = (byte)(g / 9);
                    dest[x, y].B = (byte)(b / 9);
                    dest[x, y].I = (byte)(i / 9);
                }
            }

            return dest;
        }

        public static PixelRGB[,] BoxFilter(PixelRGB[,] src, PixelRGB[,] dest) {
            return BoxFilterImpl(src, dest);
        }
    }
}
