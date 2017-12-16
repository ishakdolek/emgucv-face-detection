using System;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;


namespace Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private Capture _capture;
        private bool _captureInProgress;
        private CascadeClassifier _cascadeClassifier;
        private void Form1_Load(object sender, EventArgs e)
        {


            if (_capture == null)
            {
                try
                {
                    _capture = new Capture();
                }
                catch (NullReferenceException excpt)
                {
                    MessageBox.Show(excpt.Message);
                }
            }

            if (_capture == null) return;
            if (_captureInProgress)
            { 
                btnStart.Text = @"Start!"; //
                Application.Idle -= ProcessFrame;
            }
            else
            {
                btnStart.Text = @"Stop";
                Application.Idle += ProcessFrame;
            }
            _captureInProgress = !_captureInProgress;
        }
        
        private void ProcessFrame(object sender, EventArgs arg)
        {
            Image<Bgr, Byte> imgeOrigenal = _capture.QueryFrame().ToImage<Bgr, Byte>();  //line 1
            pictureBox1.Image = imgeOrigenal.Bitmap;
            try
            {
                _cascadeClassifier = new CascadeClassifier("haarcascade_frontalface_alt2.xml");
                //var imageFrame = _capture.QueryFrame().ToImage<Bgr, Byte>();
                {
                    var grayframe = imgeOrigenal.Convert<Gray, byte>();
                    var faces = _cascadeClassifier.DetectMultiScale(grayframe, 1.1,3, maxSize: new Size(200, 200)); //the actual face detection happens here

                    foreach (var face in faces)
                    {
                        imgeOrigenal.Draw(face, new Bgr(Color.Green), 3); //the detected face(s) is highlighted here using a box that is drawn around it/them
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            //pictureBox1.Image = imgeOrigenal.Bitmap;  //line 2
        }

        private void btnStart_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
