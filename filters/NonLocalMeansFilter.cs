using System;
using static DinosaurGraphics.helpers.GuassianKernelHelper;

namespace DinosaurGraphics.filters {
    public static class NonLocalMeansFilter {

        public static PixelRGB[,] NonLocalMeansImpl(PixelRGB[,] src, PixelRGB[,] dest, float h, int window_size, int sim_window_size) {

            int width = dest.GetLength(0);
            int height = dest.GetLength(1);

            PixelRGB[,] W1;
            PixelRGB[,] W2;
            PixelRGB[,] SubstractWindow;
            
            int mid_window = (window_size - 1) / 2;
            int mid_sim_window = (sim_window_size - 1) / 2;

            for(int i = mid_window; i < width - mid_window; i++) {
                for(int j = mid_window; j < height - mid_window; j++) {

                    double sum = 0.0f;
                    W1 = GetWindow(src, i, j, window_size);

                    int umin = Math.Max(i - mid_sim_window, mid_window);
                    int umax = Math.Min(i + mid_sim_window, width - mid_window);
                    int vmin = Math.Max(j - mid_sim_window, mid_window);
                    int vmax = Math.Min(j + mid_sim_window, height - mid_window);

                    double norm = 0.0d;

                    for(int u = umin; u < umax; u++) {
                        for(int v = vmin; v < vmax; v++) {

                            W2 = GetWindow(src, u, v, window_size);
                            SubstractWindow = SubstractWindows(W1, W2, window_size);
                        }
                    }

                }
            }


            return dest;
        }

        static PixelRGB[,] SubstractWindows(PixelRGB[,] v, PixelRGB[,] u, int size) {

            PixelRGB[,] result = new PixelRGB[size, size];

            for(int i = 0; i < size; i++) {
                for(int j = 0; j < size; j++) {
                    byte r = (byte)(v[i, j].R - u[i, j].R);
                    byte g = (byte)(v[i, j].G - u[i, j].G);
                    byte b = (byte)(v[i, j].B - u[i, j].B);

                    result[i, j] = new PixelRGB(r, g, b);
                }
            }

            return result;
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
