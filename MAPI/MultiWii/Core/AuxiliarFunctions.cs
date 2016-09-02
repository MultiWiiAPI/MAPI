using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Drawing;
using System.IO;

namespace MultiWii
{
    public static class AuxiliarFunctions
    {
        public static Boolean openPort(SerialPort serialPort, Boolean print){
            Boolean result = false;
            int timeForOpenPort = 2000;
            if(print){Console.WriteLine("Port: "+ serialPort.PortName + " is openning.");}
            try{
                serialPort.Open();
                System.Threading.Thread.Sleep(timeForOpenPort);
                if (print) { Console.WriteLine("Port: " + serialPort.PortName + " opened sucessfully."); }
                result = true;
            } catch (Exception e){
                Console.WriteLine("ERROR 001: " + e.ToString() + ".");
                if (print) { Console.WriteLine("Port: " + serialPort.PortName + " cannot be opened."); }
                serialPort.Close();
                if (print) { Console.WriteLine("Port: " + serialPort.PortName + " closed."); }
            }
            return result;
        }
        
        public static int speedController(int counter)
        {
            switch (counter)
            {
                case 0:
                    {
                        int data = 1000;
                        Console.WriteLine(string.Join(",", data));
                        return data;                   
                    }
                case 1:
                    {
                        int data = 1200;
                        Console.WriteLine(string.Join(",", data));
                        return data;
                    }
                case 2:
                    {
                        int data = 1400;
                        Console.WriteLine(string.Join(",", data));
                        return data;
                    }
                case 3:
                    {
                        int data = 1600;
                        Console.WriteLine(string.Join(",", data));
                        return data;
                    }
                case 4:
                    {
                        int data = 1800;
                        Console.WriteLine(string.Join(",", data));
                        return data;
                    }
                case 5:
                    {
                        int data = 1900;
                        Console.WriteLine(string.Join(",", data));
                        return data;
                    }
                
            }
            return 1000;
        }

        public static int moveController(int counter)
        {
            switch (counter)
            {
                case 0:
                    {
                        int data = 1100;
                        return data;
                    }
                case 1:
                    {
                        int data = 1300;
                        return data;
                    }

                case 2:
                    {
                        int data =1500;
                        return data;
                    }

                case 3:
                    {
                        int data =1700;
                        return data;
                    }

                case 4:
                    {
                        int data =1900;
                        return data;
                    }
            }
            int res = 1500;
            return res;
        }

        public static int normalize(int minimalValue, int maxValue, int actualValue)
        {
            Double min = Convert.ToDouble(minimalValue);
            Double max = Convert.ToDouble(maxValue);
            Double actual = Convert.ToDouble(actualValue);

            if (min > max)
            {
                actual = min - actual;
                max = min - max;
                min = 0;

            }
            double a = (actual - min) / (max - min);
            double b = a * 1000;
            double c = 1000 + b;

            if (c < 1000)
            {
                c = 1000;
            }
            if (c > 2000)
            {
                c = 2000;
            }
            return Convert.ToInt32(c);
        }

        public static Image cropImageByRectangle(Image image, Rectangle rectangle)
        {
            Bitmap bmp = new Bitmap(rectangle.Width, rectangle.Height);
            using (Graphics gr = Graphics.FromImage(bmp))
            {
                gr.DrawImage(image, new Rectangle(0, 0, bmp.Width, bmp.Height), rectangle, GraphicsUnit.Pixel);
            }
            return bmp;
        }

        public static String templateSelection(int counter)
        {
            switch (counter)
            {
                case 0:
                    {
                        return "up";
                    }
                case 1:
                    {
                        return "down";
                    }
                case 2:
                    {
                        return "left";
                    }
                case 3:
                    {
                        return "right";
                    }
                

            }
            return "not encountered";
        }

        public static List<Image<Bgr, byte>> searchTemplates(String path, String fileName, String fileType, int numberOfMaxTemplatesSearched)
        {
            List<Image<Bgr, byte>> result = new List<Image<Bgr, byte>>();
            for (int i = 1; i < numberOfMaxTemplatesSearched; i++)
            {
                String nameTemplate = "//" + fileName + i + "." + fileType;
                if (System.IO.File.Exists(path + nameTemplate))
                {
                    result.Add(new Image<Bgr, byte>(path + nameTemplate));
                }
                else
                {
                    break;
                }
            }
            return result;
        }

        public static Boolean isNotInRange(Bgr now_color, Double redmin, Double redmax, Double bluemin, Double bluemax, Double greenmin, Double greenmax)
        {

            if ((now_color.Red < redmin || now_color.Red > redmax) || (now_color.Blue < bluemin || now_color.Blue > bluemax) || (now_color.Green < greenmin || now_color.Green > greenmax))
            {
                return true;
            }
            return false;
        }
    } 
}
