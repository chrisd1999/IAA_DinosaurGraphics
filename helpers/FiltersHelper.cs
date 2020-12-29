using static DinosaurGraphics.filters.GuassianBlurFilter;
using static DinosaurGraphics.filters.OutlierTechniqueFilter;
using static DinosaurGraphics.filters.NonLocalMeansFilter;
using static DinosaurGraphics.helpers.GuassianKernelHelper;

namespace DinosaurGraphics.helpers {
    public static class FiltersHelper {

        public static PixelRGB[,] GuassianBlur(PixelRGB[,] src, PixelRGB[,] dest, double[,] kernel) {
            return GuassianBlurImpl(src, dest, kernel);
        }

        public static PixelRGB[,] GuassianBlur(PixelRGB[,] src, PixelRGB[,] dest, int len, double sigma) {
            return GuassianBlurImpl(src, dest, GuassianKernel(len, sigma));
        }

        public static PixelRGB[,] OutlierTechnique(PixelRGB[,] src, PixelRGB[,] dest, int t) {
            return OutlierTechniqueImpl(src, dest, t);
        }

        public static PixelRGB[,] OutlierTechnique(PixelRGB[,] src, PixelRGB[,] dest) {
            return OutlierTechniqueImpl(src, dest, 1);
        }

        public static PixelRGB[,] NonLocalMeansFilter(PixelRGB[,] src, PixelRGB[,] dest, int h, int patchSize, int windowSize) {
            return NonLocalMeansImpl(src, dest, h, patchSize, windowSize);
        }
    }
}
