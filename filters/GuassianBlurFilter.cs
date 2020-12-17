using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kristers_Dugels_181RDB024.filters {
    public static class GuassianBlurFilter {

        public static PixelRGB[,] GuassianBlurImpl(PixelRGB[,] src, PixelRGB[,] dest, double[,] kernel) {

            int width  = dest.GetLength(0);
            int height = dest.GetLength(1);

            int kernelLen = kernel.GetLength(0) - 1;
            int edge = kernelLen / 2;

            for(int x = edge; x < width - edge; x++) {
                for(int y = edge; y < height - edge; y++) {

                    double r = 0;
                    double g = 0;
                    double b = 0;
                    double i = 0;

                    for(int fx = 0; fx < kernelLen; fx++) {
                        for(int fy = 0; fy < kernelLen; fy++) {
                            r +=src[x + fx - edge, y + fy - edge].R * kernel[fx, fy];
                            g +=src[x + fx - edge, y + fy - edge].G * kernel[fx, fy];
                            b +=src[x + fx - edge, y + fy - edge].B * kernel[fx, fy];
                            i +=src[x + fx - edge, y + fy - edge].I * kernel[fx, fy];
                        }
                    }

                    r = Math.Max(0, Math.Min(255, r));
                    g = Math.Max(0, Math.Min(255, g));
                    b = Math.Max(0, Math.Min(255, b));
                    i = Math.Max(0, Math.Min(255, i));


                    dest[x, y].R = (byte)r;
                    dest[x, y].G = (byte)g;
                    dest[x, y].B = (byte)b;
                    dest[x, y].I = (byte)i;
                }
            }
            return dest;
        }
    }
}
