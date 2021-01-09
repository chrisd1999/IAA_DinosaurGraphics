using System;

namespace DinosaurGraphics.helpers {
    public static class GuassianKernelHelper {
        public static double[,] GuassianKernel(int len, double sigma) {
            double[,] gaussianKernel = new double[len, len];
            double sum = 0;
            int r = (len - 1)/2;

            double euler = 1f / (2f * Math.PI * Math.Pow(sigma, 2));

            for (int y = -r; y <= r; y++) {
                for (int x = -r; x <= r; x++) {
                    double distance = ((x * x) + (y * y) / (2 * sigma * sigma));

                    gaussianKernel[y + r, x + r] = euler * Math.Exp(-distance);
                    sum += gaussianKernel[y + r, x + r];
                }
            }

            for (int y = 0; y < len; y++) {
                for (int x = 0; x < len; x++) {
                    gaussianKernel[y, x] *= 1f / sum;
                }
            }

            return gaussianKernel;
        }
    }

}
