using System;
using static DinosaurGraphics.helpers.GuassianKernelHelper;

namespace DinosaurGraphics.filters {
    public static class NonLocalMeansFilter {

        public static PixelRGB[,] NonLocalMeansImpl(PixelRGB[,] src, PixelRGB[,] dest, double stdev, int window_size, int sim_window_size) {

            int width = src.GetLength(0);
            int height = src.GetLength(1);

            PixelRGB[,] W1;
            PixelRGB[,] W2;

            int mid_window = (window_size - 1) / 2;
            int mid_sim_window = (sim_window_size - 1) / 2;

            for(int i = mid_window; i < width - mid_window; i++) {
                for(int j = mid_window; j < height - mid_window; j++) {

                    double sumR = 0.0d;
                    double sumG = 0.0d;
                    double sumB = 0.0d;

                    W1 = GetWindow(src, i, j, window_size);

                    int umin = Math.Max(i - mid_sim_window, mid_window);
                    int umax = Math.Min(i + mid_sim_window, width - mid_window);
                    int vmin = Math.Max(j - mid_sim_window, mid_window);
                    int vmax = Math.Min(j + mid_sim_window, height - mid_window);

                    double normalization_factorR = 0.0d;
                    double normalization_factorG = 0.0d;
                    double normalization_factorB = 0.0d;

                    for (int u = umin; u < umax; u++) {
                        for(int v = vmin; v < vmax; v++) {

                            W2 = GetWindow(src, u, v, window_size);
                            var SW = SubstractWindows(W1, W2, window_size);

                            var norm_value = Normalize(SW, window_size);
                            double similarityR = Math.Exp(-norm_value.Item1 / Math.Pow(stdev, 2.0d));
                            double similarityG = Math.Exp(-norm_value.Item2 / Math.Pow(stdev, 2.0d));
                            double similarityB = Math.Exp(-norm_value.Item3 / Math.Pow(stdev, 2.0d));

                            normalization_factorR += similarityR;
                            normalization_factorG += similarityG;
                            normalization_factorB += similarityB;

                            sumR += similarityR * src[u, v].R;
                            sumG += similarityG * src[u, v].G;
                            sumB += similarityB * src[u, v].B;
                        }
                    }

                    double r = Math.Max(0, Math.Min(255, sumR / normalization_factorR));
                    double g = Math.Max(0, Math.Min(255, sumG / normalization_factorG));
                    double b = Math.Max(0, Math.Min(255, sumB / normalization_factorB));

                    dest[i, j].R = (byte)Math.Round(r);
                    dest[i, j].G = (byte)Math.Round(g);
                    dest[i, j].B = (byte)Math.Round(b);

                }
            }

            return dest;
        }

        static Tuple<double, double, double> Normalize(Tuple<double[,], double[,], double[,]> v, int size) {

            double sumR = 0.0d;
            double sumG = 0.0d;
            double sumB = 0.0d;


            for (int i = 0; i < size; i++) {
                for(int j = 0; j < size; j++) {
                    sumR += Math.Pow(v.Item1[i, j], 2.0d);
                    sumG += Math.Pow(v.Item2[i, j], 2.0d);
                    sumB += Math.Pow(v.Item3[i, j], 2.0d);
                }
            }

            return Tuple.Create(Math.Sqrt(sumR), Math.Sqrt(sumG), Math.Sqrt(sumB));
        }

        static Tuple<double[,], double[,], double[,]> SubstractWindows(PixelRGB[,] v, PixelRGB[,] u, int size) {

            double[,] resultR = new double[size, size];
            double[,] resultG = new double[size, size];
            double[,] resultB = new double[size, size];


            for (int i = 0; i < size; i++) {
                for(int j = 0; j < size; j++) {
                    resultR[i, j] = (double)v[i, j].R - (double)u[i, j].R;
                    resultG[i, j] = (double)v[i, j].G - (double)u[i, j].G;
                    resultB[i, j] = (double)v[i, j].B - (double)u[i, j].B;
                }
            }

            return Tuple.Create(resultR, resultG, resultB);
        }

        static PixelRGB[,] GetWindow(PixelRGB[,] src, int x, int y, int size) {

            PixelRGB[,] window = new PixelRGB[size, size];
            int mid = (int)(size - 1) / 2;

            for(int m = -mid, i = 0; m < mid + 1; m++, i++) {
                for(int n = -mid, j = 0; n < mid + 1; n++, j++) {
                    window[i, j] = new PixelRGB(src[x + m, y + n].R, src[x + m, y + n].G, src[x + m, y + n].B);
                }
            }

            return window;
        }

        //static PixelRGB[,] GetWindow(PixelRGB[,] src, int x1, int x2, int y1, int y2) {

        //    int size1 = x2 - x1;
        //    int size2 = y2 - y1;
        //    PixelRGB[,] window = new PixelRGB[size1, size2];

        //    for (int x = x1, i = 0; x < x2; x++, i++) {
        //        for (int y = y1, j = 0; y < y2; y++, j++) {
        //            window[i, j] = new PixelRGB(src[x, y].R, src[x, y].G, src[x, y].B);
        //        }
        //    }

        //    return window;
        //}

    }
}
