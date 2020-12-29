using System;
using static DinosaurGraphics.helpers.GuassianKernelHelper;

namespace DinosaurGraphics.filters {
    public static class NonLocalMeansFilter {

        public static PixelRGB[,] NonLocalMeansImpl(PixelRGB[,] src, PixelRGB[,] dest, double h, int window_size, int sim_window_size) {

            int width = src.GetLength(0);
            int height = src.GetLength(1);

            double[,] kernel = GuassianKernel(window_size, 1);
            int mid_window = (window_size - 1) / 2;
            int mid_sim_window = (sim_window_size - 1) / 2;

            double h2 = h * h;

            for(int i = mid_window; i < width - mid_window; i++) {
                for(int j = mid_window; j < height - mid_window; j++) {

                    PixelRGB[,] W1 = GetWindow(src, i - mid_window, i + mid_window, j - mid_window, j + mid_window);

                    int umin = Math.Max(i - mid_sim_window, mid_window+1);
                    int umax = Math.Min(i + mid_sim_window, width - mid_window);
                    int vmin = Math.Max(j - mid_sim_window, mid_window+1);
                    int vmax = Math.Min(j + mid_sim_window, height - mid_window);

                    double Zr = 0.0d;
                    double Zg = 0.0d;
                    double Zb = 0.0d;

                    double NLr = 0.0d;
                    double NLg = 0.0d;
                    double NLb = 0.0d;

                    for (int u = umin; u < umax; u++) {
                        for (int v = vmin; v < vmax; v++) {

                            PixelRGB[,] W2 = GetWindow(src, u - mid_window, u + mid_window, v - mid_window, v + mid_window);

                            var result_window = SubstractWindows(W1, W2, window_size - 1);

                            var norm_value = Normalize(result_window, window_size - 1);

                            double sijR = Math.Exp(-norm_value.Item1 / h2);
                            double sijG = Math.Exp(-norm_value.Item2 / h2);
                            double sijB = Math.Exp(-norm_value.Item3 / h2);

                            //var D2 = WeightedDistance(W1, W2, kernel, window_size - 1);

                            //var sumDistance = SumDistance(D2, window_size - 1);
                            //double sijR = Math.Exp(-sumDistance.Item1 / h2);
                            //double sijG = Math.Exp(-sumDistance.Item2 / h2);
                            //double sijB = Math.Exp(-sumDistance.Item3 / h2);

                            Zr += sijR;
                            Zg += sijG;
                            Zb += sijB;

                            NLr += (sijR * src[i, j].R);
                            NLg += (sijG * src[i, j].G);
                            NLb += (sijB * src[i, j].B);
                        }
                    }

                    dest[i, j].R = (byte)Math.Max(0, Math.Min(255, NLr / Zr));
                    dest[i, j].G = (byte)Math.Max(0, Math.Min(255, NLg / Zg));
                    dest[i, j].B = (byte)Math.Max(0, Math.Min(255, NLb / Zb));
                }
            }

            return dest;
        }

        static Tuple<double, double, double> Normalize(Tuple<int[,], int[,], int[,]> values, int size) {

            double sumR = 0.0d;
            double sumG = 0.0d;
            double sumB = 0.0d;

            for(int i = 0; i < size; i++) {
                for(int j = 0; j < size; j++) {
                    sumR += Math.Pow(values.Item1[i, j], 2.0);
                    sumG += Math.Pow(values.Item2[i, j], 2.0);
                    sumB += Math.Pow(values.Item3[i, j], 2.0);
                }
            }

            return Tuple.Create(sumR, sumG, sumB);
        }

        static Tuple<double[,], double[,], double[,]> WeightedDistance(PixelRGB[,] W1, PixelRGB[,] W2, double[,] kernel, int size) {

            double[,] distanceR = new double[size, size];
            double[,] distanceG = new double[size, size];
            double[,] distanceB = new double[size, size];

            var substracted = SubstractWindows(W1, W2, size);
            var multiplied = MultiplyWindows(substracted, size);

            for(int i = 0; i < size; i++) {
                for(int j = 0; j < size; j++) {
                    distanceR[i, j] = multiplied.Item1[i, j] * kernel[i, j];
                    distanceG[i, j] = multiplied.Item2[i, j] * kernel[i, j];
                    distanceB[i, j] = multiplied.Item3[i, j] * kernel[i, j];
                }
            }

            return Tuple.Create(distanceR, distanceG, distanceB);
        }

        static Tuple<double, double, double> SumDistance(Tuple<double[,], double[,], double[,]> distance, int size) {
            double sumR = 0.0d;
            double sumG = 0.0d;
            double sumB = 0.0d;

            for(int i = 0; i < size; i++) {
                for(int j = 0; j < size; j++) {
                    sumR += distance.Item1[i, j];
                    sumG += distance.Item2[i, j];
                    sumB += distance.Item3[i, j];
                }
            }

            return Tuple.Create(sumR, sumG, sumB);
        }

        static Tuple<int[,], int[,], int[,]> SubstractWindows(PixelRGB[,] v, PixelRGB[,] u, int size) {

            int[,] substractedR = new int[size, size];
            int[,] substractedG = new int[size, size];
            int[,] substractedB = new int[size, size];


            for (int i = 0; i < size; i++) {
                for(int j = 0; j < size; j++) {
                    substractedR[i, j] = v[i, j].R - u[i, j].R;
                    substractedG[i, j] = v[i, j].R - u[i, j].G;
                    substractedB[i, j] = v[i, j].R - u[i, j].B;
                }
            }

            return Tuple.Create(substractedR, substractedG, substractedB);
        }

        static Tuple<int[,], int[,], int[,]> MultiplyWindows(Tuple<int[,], int[,], int[,]> items, int size) {

            int[,] multipliedR = new int[size, size];
            int[,] multipliedG = new int[size, size];
            int[,] multipliedB = new int[size, size];

            for (int i = 0; i < size; i++) {
                for (int j = 0; j < size; j++) {
                    multipliedR[i, j] = items.Item1[i, j] * items.Item1[i, j];
                    multipliedG[i, j] = items.Item2[i, j] * items.Item2[i, j];
                    multipliedB[i, j] = items.Item3[i, j] * items.Item3[i, j];
                }
            }
            return Tuple.Create(multipliedR, multipliedG, multipliedB);
        }
        static PixelRGB[,] GetWindow(PixelRGB[,] src, int x1, int x2, int y1, int y2) {

            int size1 = x2 - x1;
            int size2 = y2 - y1;
            PixelRGB[,] window = new PixelRGB[size1, size2];

            for (int x = x1, i = 0; x < x2; x++, i++) {
                for (int y = y1, j = 0; y < y2; y++, j++) {
                    window[i, j] = new PixelRGB(src[x, y].R, src[x, y].G, src[x, y].B);
                }
            }

            return window;
        }

    }
}
