using System;

namespace Kristers_Dugels_181RDB024.helpers {
    public static class OutlierTechniqueHelper {

        static PixelRGB[,] OutlierTechniqueImpl(PixelRGB[,] src, PixelRGB[,] dest, int t) {

            int width = dest.GetLength(0);
            int height = dest.GetLength(1);

            for (int x = 1; x < width - 1; x++) {
                for (int y = 1; y < height - 1; y++) {

                    int r = 0;
                    int g = 0;
                    int b = 0;
                    int i = 0;

                    for (int fi = 0; fi < 3; fi++) {
                        for (int fj = 0; fj < 3; fj++) {
                            r += src[x + fi - 1, y + fj - 1].R;
                            g += src[x + fi - 1, y + fj - 1].G;
                            b += src[x + fi - 1, y + fj - 1].B;
                            i += src[x + fi - 1, y + fj - 1].I;
                        }
                    }

                    Func<byte, int, byte> PixelValue = (byte pixel, int sum) => 
                        (pixel - sum / 8) > t ? (byte)Math.Max(0, Math.Min(255, sum /= 8)) : pixel;

                    dest[x, y].R = PixelValue(src[x, y].R, r);
                    dest[x, y].G = PixelValue(src[x, y].G, g);
                    dest[x, y].B = PixelValue(src[x, y].B, b);
                    dest[x, y].I = PixelValue(src[x, y].I, i);
                }
            }

            return dest;
        }

        public static PixelRGB[,] OutlierTechnique(PixelRGB[,] src, PixelRGB[,] dest) {
            return OutlierTechniqueImpl(src, dest, 1);
        }

        public static PixelRGB[,] OutlierTechnique(PixelRGB[,] src, PixelRGB[,] dest, int t) {
            return OutlierTechniqueImpl(src, dest, t);
        }


    }
}
