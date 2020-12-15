using System;
using System.Drawing;
using System.Windows.Forms;
using Kristers_Dugels_181RDB024.helpers;

namespace Kristers_Dugels_181RDB024
{
    public partial class Form1 : Form {
        public ImageClass imageClass = new ImageClass();
        public Form1() {
            InitializeComponent();
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e) {
            if (openFileDialog1.ShowDialog() == DialogResult.OK) {
                pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
                Bitmap bmp = (Bitmap)pictureBox1.Image.Clone();
                imageClass.ReadImage(bmp);
                imageClass.ClonePixelArray(imageClass.img1, imageClass.img2);
                pictureBox2.Image = imageClass.DrawImage(imageClass.img2);
            }
        }

        private void InvertToolStripMenuItem_Click(object sender, EventArgs e){
            if (pictureBox1.Image != null) {
                imageClass.img2 = OutlierTechniqueHelper.OutlierTechnique(imageClass.img1, imageClass.img2);
                imageClass.img2 = GuassianBlurHelper.GuassianBlur(imageClass.img1, imageClass.img2, 19, 9.25);
                pictureBox2.Image = imageClass.DrawImage(imageClass.img2);
            }
        }
    }
}
