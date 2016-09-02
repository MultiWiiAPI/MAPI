using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Threading;
using System.Threading.Tasks;
using System.Media;



namespace MultiWii
{
    public static class ImageFunctions
    {
        public static Dictionary<string, Image<Bgr, Byte>> templateMap;
        public static List<List<Image<Bgr, byte>>> templateMapList;
        public static Point maxLocationFinal = new Point();
        public static Double maxValueFinal = -1;
        public static Image<Bgr, Byte> templateFinal = new Image<Bgr, byte>(1, 1);
        public static String templateEncountered = "no encountered";

        public static Double bluemin = 0;
        public static Double bluemax = 0;
        public static Double greenmin = 0;
        public static Double greenmax = 0;
        public static Double redmin = 0;
        public static Double redmax = 0;
        public static Bgr colorSelected = new Bgr(0, 0, 0);
        public static Boolean encountered = false;

        public static void initializeImageRecognition(Dictionary<String, Image<Bgr, Byte>> templates, Boolean filterSaltAndPepperOn, Boolean filterGaussianOn, Boolean filterBlurOn, Boolean filterSobelOn, Boolean filterCannyOn, Boolean filterColorOn, Bgr color)
        {
            templateMap = new Dictionary<String, Image<Bgr, Byte>>();
            foreach(String key in templates.Keys){
                templateMap.Add(key, templates[key]);
            }
            templateMapList = new List<List<Image<Bgr, byte>>>();

            if (filterColorOn)
            {
                colorSelected = color;
                bluemin = colorSelected.Blue - 40; if (bluemin < 0) bluemin = 0;
                bluemax = colorSelected.Blue + 40; if (bluemax > 255) bluemax = 255;
                greenmin = colorSelected.Green - 25; if (greenmin < 0) greenmin = 0;
                greenmax = colorSelected.Green + 25; if (greenmax > 255) greenmax = 255;
                redmin = colorSelected.Red - 40; if (redmin < 0) redmin = 0;
                redmax = colorSelected.Red + 40; if (redmax > 255) redmax = 255;
            }

            for (int i = 0; i < templateMap.Keys.Count; i++)
            {
                List<Image<Bgr, byte>> templateIndividualList = new List<Image<Bgr, byte>>();
                Image<Bgr, byte> imageTemplate = templateMap.Values.ElementAt(i);

                if (filterColorOn)
                {
                    imageTemplate = filterColor(imageTemplate);
                }
                if (filterSaltAndPepperOn)
                {
                    imageTemplate = filterRemoveSaltAndPepper(imageTemplate);
                }
                if (filterGaussianOn)
                {
                    imageTemplate = filterGaussian(imageTemplate);
                }
                if (filterBlurOn)
                {
                    imageTemplate = filterBlur(imageTemplate);
                }
                if (filterSobelOn)
                {
                    imageTemplate = filterSobel(imageTemplate);
                }
                if (filterCannyOn)
                {
                    imageTemplate = filterCanny(imageTemplate);
                }

                templateIndividualList.Add(imageTemplate);
                for (int j = 10; j >= 3; j--)
                {
                    double scale = 0.1 * j;
                    templateIndividualList.Add(templateIndividualList[0].Resize(scale, INTER.CV_INTER_LINEAR));
                }
                if (filterColorOn)
                {
                    for (int j = 10; j >= 7; j--)
                    {
                        double scale = 0.1 * j;
                        templateIndividualList.Add(templateIndividualList[0].Resize(scale, INTER.CV_INTER_LINEAR).Rotate(15, new Bgr(Color.Black), false));
                        templateIndividualList.Add(templateIndividualList[0].Resize(scale, INTER.CV_INTER_LINEAR).Rotate(-15, new Bgr(Color.Black), false));
                    }
                }
                templateMapList.Add(templateIndividualList);
            }
        }

        public static void deleteImageProcessing()
        {
            templateMap = null;
            templateMapList = null;
            colorSelected = new Bgr(0, 0, 0);
            bluemin = 0;
            bluemax = 0;
            greenmin = 0;
            greenmax = 0;
            redmin = 0;
            redmax = 0;
            maxLocationFinal = new Point();
            maxValueFinal = -1;
            templateFinal = new Image<Bgr, byte>(1, 1);
            templateEncountered = "no encountered";
        }

        public static Boolean templateMatching(Image<Bgr, Byte> source, Image<Bgr, Byte> template, Double threshold)
        {
            Boolean result = false;
            using (Image<Gray, float> imageRecognized = source.MatchTemplate(template.PyrDown().PyrDown(), Emgu.CV.CvEnum.TM_TYPE.CV_TM_CCOEFF_NORMED))
            {
                double[] minValues, maxValues;
                Point[] minLocations, maxLocations;
                imageRecognized.MinMax(out minValues, out maxValues, out minLocations, out maxLocations);
                if (maxValues[0] > threshold)
                {
                    if (maxValues[0] > maxValueFinal)
                    {
                        result = true;
                        maxValueFinal = maxValues[0];
                        maxLocationFinal = maxLocations[0];
                        templateFinal = template;
                    }
                }
            }
            return result;
        }

        public static Image<Bgr, Byte> imageProcessing(Image<Bgr, Byte> capture, Double threshold, int typeOfProcessing, Boolean filterSaltAndPepperOn, Boolean filterGaussianOn, Boolean filterBlurOn, Boolean filterSobelOn, Boolean filterCannyOn, Boolean filterColorOn, Boolean showFilters)
        {
            maxValueFinal = -1;
            templateFinal = new Image<Bgr, byte>(1, 1);
            templateEncountered = "no encountered";
            Image<Bgr, Byte> imageToShow = capture.Copy();
            Image<Bgr, Byte> source = capture.PyrDown().PyrDown();
            encountered = false;

            if (filterColorOn)
            {
                source = filterColor(source);
                if (showFilters)
                {
                    imageToShow = filterColor(imageToShow);
                }
            }

            if (filterSaltAndPepperOn)
            {
                source = filterRemoveSaltAndPepper(source);
                if (showFilters)
                {
                    imageToShow = filterRemoveSaltAndPepper(imageToShow);
                }
            }

            if (filterGaussianOn)
            {
                source = filterGaussian(source);
                if (showFilters)
                {
                    imageToShow = filterGaussian(imageToShow);
                }
            }

            if (filterBlurOn)
            {
                source = filterBlur(source);
                if (showFilters)
                {
                    imageToShow = filterBlur(imageToShow);
                }
            }

            if (filterSobelOn)
            {
                source = filterSobel(source);
                if (showFilters)
                {
                    imageToShow = filterSobel(imageToShow);
                }
            }

            if (filterCannyOn)
            {
                source = filterCanny(source);
                if (showFilters)
                {
                    imageToShow = filterCanny(imageToShow);
                }
            }



            var options = new ParallelOptions()
            {
                MaxDegreeOfParallelism = templateMapList.Count()
            };


            Parallel.For(0, templateMapList.Count(), options, (i) =>
            {
                if (typeOfProcessing == 1)
                {
                    Parallel.ForEach(templateMapList.ElementAt(i), (Image<Bgr, Byte> templateToMatch) =>
                    //foreach (Image<Bgr, Byte> templateToMatch in templateMapList.ElementAt(i))
                    {
                        if (templateMatching(source, templateToMatch, threshold))
                        {
                            encountered = true;
                            templateEncountered = templateMap.Keys.ElementAt(i);
                        }
                    //}
                    });
                }
            });

            return imageToShow;
        }

        public static Image<Bgr, Byte> filterColor(Image<Bgr, Byte> image)
        {
            int i, j;
            for (i = 0; i < image.Height; i++)
            {
                for (j = 0; j < image.Width; j++)
                {
                    Bgr now_color = image[i, j];
                    if (AuxiliarFunctions.isNotInRange(now_color, redmin, redmax, bluemin, bluemax, greenmin, greenmax))
                    {
                        image[i, j] = new Bgr(0, 0, 0);
                    }
                    else
                    {
                        image[i, j] = colorSelected;
                    }
                }
            }
            return image;
        }

        public static Image<Bgr, Byte> filterRemoveSaltAndPepper(Image<Bgr, Byte> image)
        {
            return image.SmoothMedian(3);
        }

        public static Image<Bgr, Byte> filterGaussian(Image<Bgr, Byte> image)
        {
            return image.SmoothGaussian(3);
        }

        public static Image<Bgr, Byte> filterBlur(Image<Bgr, Byte> image)
        {
            return image.SmoothBlur(10, 10);
        }

        public static Image<Bgr, Byte> filterSobel(Image<Bgr, Byte> image)
        {
            return image.Sobel(1, 0, 5).Convert<Bgr, Byte>();
        }

        public static Image<Bgr, Byte> filterCanny(Image<Bgr, Byte> image)
        {
            return image.Convert<Gray, byte>().Canny(new Gray(30.0), new Gray(190.0)).Convert<Bgr, byte>();
        }

    }
}