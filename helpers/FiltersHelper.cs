using static Kristers_Dugels_181RDB024.filters.GuassianBlurFilter;
using static Kristers_Dugels_181RDB024.filters.OutlierTechniqueFilter;
using static Kristers_Dugels_181RDB024.helpers.GuassianKernelHelper;

namespace Kristers_Dugels_181RDB024.helpers {
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
    }
}
