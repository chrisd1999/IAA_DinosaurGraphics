using System;

namespace Kristers_Dugels_181RDB024
{
    public static class Utilities
    {
        public static System.Drawing.Point ConvertXY(int x, int y, System.Windows.Forms.PictureBox pb)
        {
            System.Drawing.Point p = new System.Drawing.Point();
            int imgWidth = pb.Image.Width;
            int imgHeight = pb.Image.Height;
            int boxWidth = pb.Width;
            int boxHeight = pb.Height;

            double kx = (double)imgWidth / boxWidth;
            double ky = (double)imgHeight / boxHeight;

            double k = Math.Max(kx, ky);

            double nobidex = (boxWidth * k - imgWidth) / 2f;
            double nobidey = (boxHeight * k - imgHeight) / 2f;

            p.X = Convert.ToInt32(Math.Max(Math.Min(Math.Round(x * k - nobidex), imgWidth - 1), 0));
            p.Y = Convert.ToInt32(Math.Max(Math.Min(Math.Round(y * k - nobidey), imgHeight - 1), 0));

            return p;
        }
    }
}
