using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kristers_Dugels_181RDB024.helpers {
    public static class GuassianBlur {

        static PixelRGB[,] GuassianBlurImpl(PixelRGB[,] src, PixelRGB[,] dest) {

            int width = dest.GetLength(0);
            int height = dest.GetLength(1);

            for (int x = 1; x < width - 1; x++) {
                for (int y = 1; y < height - 1; y++) {

                }
            }

            return src;
        }
    }
}
