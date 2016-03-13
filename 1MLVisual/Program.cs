using BrainNet.NeuralFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1MLVisual
{
    class Program
    {
        public static bool running = true;

        public static INeuralNetwork network;
        public static NetworkHelper helper;
        public static ImageProcessingHelper imagehelper = new ImageProcessingHelper();
        public static UI ui = new UI();
        public static Color white = Color.FromArgb(255, 255, 255);

        static void LoadTrainingData()
        {
            Console.WriteLine("Loading Training Data from previous runs");
            FileInfo[] files = new DirectoryInfo("TrainingData").GetFiles("*.json");
            ui.progressBar1.Minimum = 0;
            ui.progressBar1.Maximum = files.Length;
            foreach (var file in files)
            {
                string json = File.ReadAllText(file.FullName);
                List<TrainingData> tds = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TrainingData>>(json);
                foreach (TrainingData td in tds)
                {
                    helper.AddTrainingData(td);
                }
                helper.Train(7);
                helper.ClearTrainingData();
                Console.WriteLine("Loaded: " + file.Name);
                ui.progressBar1.Increment(1);
            }
            ui.progressBar1.Value = 0;
        }
        static void Main(string[] args)
        {
            new System.Threading.Thread(() => ui.ShowDialog()).Start();
            System.Threading.Thread.Sleep(1000);
            Console.Write("Setting up neural network with 625 neurons...");
            ArrayList layers = new ArrayList();
            layers.Add(25 * 25);//one neuron per pixel
            layers.Add(25 * 25);
            layers.Add(1);

            network = new BackPropNetworkFactory().CreateNetwork(layers);
            helper = new NetworkHelper(network);
            Console.WriteLine("[OK!]");

            LoadTrainingData();

            Lib1MLAI.Bluestacks.Start();
            //Lib1MLAI.BmpViewer bmpviewer = new Lib1MLAI.BmpViewer(new System.Drawing.Bitmap(1, 1));
            // Lib1MLAI.BmpViewer bmpviewer2 = new Lib1MLAI.BmpViewer(new System.Drawing.Bitmap(1, 1));
          //  new System.Threading.Thread(() =>
            {
                decimal last = 0m;
                Bitmap lastbmp = new Bitmap(25, 25);
                int no = 0;

                int consectss = 0;
                int lastwhites = 0;
                while (running)
                {
                    Bitmap b = Lib1MLAI.Bluestacks.MainLoop();
                   // b = (Bitmap)cropImage(b, new Rectangle(100, 100, 100, 100));
                    Bitmap bscale = ScaleImage(b, 25, 25);
                    Filter(bscale);

                    Bitmap score = (Bitmap)cropImage(b, new Rectangle(570, 75, 100, 50));
                    int whites = CountPixels(score, white, 0);
                    if (Math.Abs(whites - lastwhites) > 80)
                    {
                        consectss = 0;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Score Changed!");
                        if (last >= 0.5m)
                        {
                            //reward NN!
                            Teach(lastbmp, 1, 5);
                            Console.WriteLine("Rewarded NN");
                        }
                        Console.ResetColor();
                    }
                    else
                    {
                        consectss++;
                        if (consectss >= 10)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Score hasn't changed for 10 consec ticks!");
                                //tell off NN
                            Teach(lastbmp, -2, 5);
                                Console.WriteLine("Told off NN");
                            Console.ResetColor();
                            consectss = 0;
                        }
                    }
                    lastwhites = whites;
                    ui.bmpViewerControl2.LoadImage(score);

                    if (Lib1MLAI.Bluestacks.udied)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("I died!");
                        Lib1MLAI.Bluestacks.udied = false;
                        //u just died, so that means - doing the opposite of what you just did will be better
                        //long learn = (long)(last * -2);
                        //bool lb = last >= 0.5m;
                        if (last >= 0.5m)
                        {
                            Teach(lastbmp, -5, 50);
                        }
                        else
                        {
                            Teach(lastbmp, 5, 50);
                        }
                        Console.ResetColor();
                    }

                    ArrayList input = imagehelper.ArrayListFromImage(bscale);
                    ArrayList output;
                    output = network.RunNetwork(input);
                    decimal d = 0m;
                    try
                    {
                        d = (decimal)((double)output[0]);
                    }
                    catch { Console.WriteLine("Unkown output from the NN: "+output[0].GetType()); }
                    Console.Write(d);

                    if (d >= 0.5m)
                    {
                        Lib1MLAI.Bluestacks.actionqueue.Add(true);
                        Console.WriteLine(" = True");
                    }
                    else
                    {
                        Lib1MLAI.Bluestacks.actionqueue.Add(false);
                        Console.WriteLine(" = False");
                    }

                    last = d;
                    lastbmp = bscale;
                    ui.bmpViewerControl1.LoadImage(b);
                    ui.UpdateTree(network);
                    no++;
                }
            }//).Start();
            // new System.Threading.Thread(() => bmpviewer2.ShowDialog()).Start();
            // bmpviewer.ShowDialog();
            //new UI().ShowDialog();
        }
        public static void Teach(Bitmap b, long output, int amount)
        {
            DirectoryInfo d = new DirectoryInfo("TrainingData");
            if (!d.Exists)
            {
                d.Create();
            }

            helper.ClearTrainingData();
            helper.AddTrainingData(b, output);
            using (StreamWriter sw = new StreamWriter("TrainingData\\"+b.GetHashCode() + Path.GetRandomFileName().Replace(".","")+DateTime.Now.Ticks+".json"))
            {
                sw.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(helper.TrainingDataQueue));
            }
            helper.Train(amount);
            Console.WriteLine("\tTaught NN: " + output + " " + amount + "x");
            helper.ClearTrainingData();
        }

        private static Image cropImage(Image img, Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(img);
            return bmpImage.Clone(cropArea, bmpImage.PixelFormat);
        }

        public static int CountPixels(Bitmap bmp, Color c,double tolerance)
        {
            int no = 0;
            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    Color color = bmp.GetPixel(x, y);

                    double rdif = 0;
                    double gdif = 0;
                    double bdif = 0;

                    rdif = color.R - c.R;
                    gdif = color.G - c.G;
                    bdif = color.B - c.B;

                    if (Math.Abs(rdif + gdif + bdif) <= tolerance)
                    {
                        no++;
                    }
                }
            }
            return no;
        }
        public static void Filter(Image b)
        {
            using (var g = Graphics.FromImage(b))
            {
                g.FillRectangle(Brushes.Black, new Rectangle(0, 23, 50, 2));//bottom
                g.FillRectangle(Brushes.Black, new Rectangle(0, 0, 50, 1));
            }
        }
        public static Bitmap ScaleImage(Image image, int newWidth, int newHeight)
        {
            Bitmap newImage = new Bitmap(newWidth, newHeight);
            Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight);
            Bitmap bmp = new Bitmap(newImage);
            return bmp;
        }
    }
}
