using System;
using static DinosaurGraphics.helpers.GuassianKernelHelper;

namespace DinosaurGraphics.filters {
    public static class NonLocalMeansFilter {

        public static PixelRGB[,] NonLocalMeansImpl(PixelRGB[,] src, PixelRGB[,] dest, double h, int mid_patch, int mid_sim) {

            int width = src.GetLength(0);
            int height = src.GetLength(1);


            int patch_size = 2*mid_patch + 1;
            double[,] kernel = GuassianKernel(patch_size-1, 1);

            double h2 = h * h;

            for(int i = mid_patch; i < width - mid_patch; i++) {
                for(int j = mid_patch; j < height - mid_patch; j++) {

                    byte[,] W1 = GetWindow(src, i - mid_patch, i + mid_patch, j - mid_patch, j + mid_patch);

                    int umin = Math.Max(i - mid_sim, mid_patch + 1);
                    int umax = Math.Min(i + mid_sim, width - mid_patch);
                    int vmin = Math.Max(j - mid_sim, mid_patch + 1);
                    int vmax = Math.Min(j + mid_sim, height - mid_patch);

                    double Z = 0.0d;
                    double NLr = 0.0d;
                    double NLg = 0.0d;
                    double NLb = 0.0d;


                    for (int u = umin; u < umax; u++) {
                        for(int v = vmin; v < vmax; v++) {

                            byte[,] W2 = GetWindow(src, u - mid_patch, u + mid_patch, v - mid_patch, v + mid_patch);

                            double distance2 = WeightedDistance(W1, W2, kernel, patch_size-1);

                            double weight = Math.Exp(-distance2 / (3* h2));
                            
                            NLr += (weight * src[u, v].R);
                            NLg += (weight * src[u, v].G);
                            NLb += (weight * src[u, v].B);


                            Z += weight;
                        }
                    }

                    dest[i, j].R = (byte)Math.Max(0, Math.Min(255, NLr / Z));
                    dest[i, j].G = (byte)Math.Max(0, Math.Min(255, NLg / Z));
                    dest[i, j].B = (byte)Math.Max(0, Math.Min(255, NLb / Z));

                }
            }

            return dest;
        }

        static double WeightedDistance(byte[,] W1, byte[,] W2, double[,] kernel, int size) {

            double[,] distance = new double[size, size];
            int[,] result      = new int[size, size];

            double sum = 0;

            /* Convulation Kernel */
            for (int i = 0; i < size; i++) {
                for (int j = 0; j < size; j++) {
                    result[i, j] = W1[i, j] - W2[i, j];
                    result[i, j] *= result[i, j];

                    distance[i, j] = kernel[i, j] * result[i, j];

                    sum += distance[i, j];
                }
            }

            return sum;
        }

        static byte[,] GetWindow(PixelRGB[,] src, int x1, int x2, int y1, int y2) {

            int size1 = x2 - x1;
            int size2 = y2 - y1;
            byte[,] window = new byte[size1, size2];

            for (int x = x1, i = 0; x < x2; x++, i++) {
                for (int y = y1, j = 0; y < y2; y++, j++) {
                    window[i, j] = src[x, y].I;
                }
            }

            return window;
        }

    }
}
