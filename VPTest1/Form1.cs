using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Patagames.Ocr;
using Patagames.Ocr.Enums;

namespace VPTest1
{
    public partial class Form1 : Form
    {
        string path = "";

        public Form1()
        {
            InitializeComponent();
            Font f = new Font("Consolas", 11, FontStyle.Bold);
            button2.Font = f;
            button1.Font = f;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var api = OcrApi.Create())
            {
                OpenFileDialog op = new OpenFileDialog();
                
                api.Init(Languages.English);
                Image test = Image.FromFile(path);
                

                string plainText = api.GetTextFromImage(ResizeImage(test,499,499));
                MessageBox.Show(plainText);
            }
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image Files (*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
            if(open.ShowDialog()==DialogResult.OK)
            {
                pictureBox1.Image = new Bitmap(open.FileName);
                textBox1.Text = open.FileName;
                path = open.FileName;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            
        }
    }
}
