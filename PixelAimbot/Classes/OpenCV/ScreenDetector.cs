using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace PixelAimbot.Classes.OpenCV
{
    internal class ScreenDetector
    {
        private Image<Bgr, byte> _pictureTemplate;
        private Image<Bgr, byte> _pictureMask;
        private float _threshold;
        private int x, y, width, height;
        private readonly Point _myPosition = new Point(ChaosBot.recalc(1920), ChaosBot.recalc(1080, false));

        public ScreenDetector(Image<Bgr, byte> pictureTemplate,
            Image<Bgr, byte> pictureMask, float threshold, int x, int y, int width, int height)
        {
            this._pictureMask = pictureMask;
            this._pictureTemplate = pictureTemplate;
            this._threshold = threshold;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

            private List<Point> DetectItems(Image<Bgr, byte> screenCapture)
        {
            List<Point> items = new List<Point>();
            screenCapture.ROI = new Rectangle(x, y, width, height);
            var fullscreen = screenCapture.Copy();
            var res = new Mat();
            double minVal = 0, maxVal = 0;
            Point minPoint = new Point();
            Point maxPoint = new Point();
            CvInvoke.MatchTemplate(fullscreen, this._pictureTemplate, res, TemplateMatchingType.SqdiffNormed, this._pictureMask);
            
            int h = this._pictureTemplate.Size.Height;
            int w = this._pictureTemplate.Size.Width;

            while (1 - minVal > this._threshold)
            {
                CvInvoke.MinMaxLoc(res, ref minVal, ref maxVal, ref minPoint, ref maxPoint);
                if (1 - minVal > this._threshold)
                {
                    var lowerLeft = new Point(minPoint.X - w / 4, minPoint.Y - h / 4);
                    var upperLeft = new Point(minPoint.X - w / 4, minPoint.Y + h / 4);
                    var lowerRight = new Point(minPoint.X + w / 4, minPoint.Y - h / 4);
                    var upperRight = new Point(minPoint.X + w / 4, minPoint.Y + h / 4);
                    var points = new Point[]
                    {
                                lowerLeft,
                                lowerRight,
                                upperRight,
                                upperLeft
                    };
                    var vector = new VectorOfPoint(points);

                    CvInvoke.FillConvexPoly(res, vector, new MCvScalar(255));
                    items.Add(minPoint);
                }
            }

            return items;
        }

        private double Distance(Point item)
        {
            return Math.Sqrt((Math.Pow(item.X - _myPosition.X, 2) + Math.Pow(item.Y - _myPosition.Y, 2)));
        }

        public Point? GetClosestItem(Image<Bgr, byte> screenCapture)
        {
            var items = DetectItems(screenCapture);
            var itemsAndPosition = items.Select(x => (x, Distance(x)));
            if (itemsAndPosition.Any())
            {
                double minDist = Double.MaxValue;
                Point closestItem = default;
                foreach (var (item, distance) in itemsAndPosition)
                {
                    if (distance < minDist)
                    {
                        minDist = distance;
                        closestItem = item;
                    }
                }

                return closestItem;
            }
            else
            {
                return null;
            }
        }
    }
}