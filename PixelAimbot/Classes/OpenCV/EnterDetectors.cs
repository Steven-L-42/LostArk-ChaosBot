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
    internal class EnterDetectors
    {
        private Image<Bgr, byte> _EnterTemplate;
        private Image<Bgr, byte> _EnterMask;
        private float _thresh;
        private readonly Point _mePosition = new Point(ChaosBot.Recalc(1920), ChaosBot.Recalc(1080, false));
        public EnterDetectors(Image<Bgr, byte> EnterTemplate,
           Image<Bgr, byte> EnterMask, float thresh)
        {
            this._EnterMask = EnterMask;
            this._EnterTemplate = EnterTemplate;
            this._thresh = thresh;
        }
        
        private List<(Point position, double matchValue)> DetectEnter(Image<Bgr, byte> screenCapture)
        {
            if (!ChaosBot.isWindowed)
            {
                this._EnterTemplate.Resize(ChaosBot.Recalc(this._EnterTemplate.Size.Width),
                    ChaosBot.Recalc(this._EnterTemplate.Size.Height), Inter.Linear);
                this._EnterMask.Resize(ChaosBot.Recalc(this._EnterMask.Size.Width),
                    ChaosBot.Recalc(this._EnterMask.Size.Height), Inter.Linear);
            }

            List<(Point minPoint, double)> Enters = new List<(Point position, double matchValue)>();
            screenCapture.ROI = new Rectangle(ChaosBot.Recalc(1259), ChaosBot.Recalc(430, false), ChaosBot.Recalc(1501), ChaosBot.Recalc(499, false));
            var minimap = screenCapture.Copy();
            var res = new Mat();
            double minVal = 0, maxVal = 0;
            Point minPoint = new Point();
            Point maxPoint = new Point();
            CvInvoke.MatchTemplate(minimap, this._EnterTemplate, res, TemplateMatchingType.SqdiffNormed, this._EnterMask);

            int h = this._EnterTemplate.Size.Height;
            int w = this._EnterTemplate.Size.Width;

            while (1 - minVal > this._thresh)
            {
                CvInvoke.MinMaxLoc(res, ref minVal, ref maxVal, ref minPoint, ref maxPoint);
                if (1 - minVal > this._thresh)
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
                    Enters.Add((minPoint, 1 - minVal));
                }
            }

            return Enters;
        }
        private double Distance(Point Enter)
        {
            return Math.Sqrt((Math.Pow(Enter.X - _mePosition.X, 2) + Math.Pow(Enter.Y - _mePosition.Y, 2)));
        }
        public Point? GetBestEnter(Image<Bgr, byte> screenCapture, bool showDetections = false)
        {
            var enemies = DetectEnter(screenCapture);
            if (enemies.Any())
            {
                double maxValue = Double.MinValue;
                Point bestEnemy = default;
                foreach (var enemy in enemies)
                {
                    if (enemy.matchValue > maxValue)
                    {
                        maxValue = enemy.matchValue;
                        bestEnemy = enemy.position;
                    }
                }

                return bestEnemy;
            }
            else
            {
                return null;
            }
        }
        public Point? GetClosestEnter(Image<Bgr, byte> screenCapture)
        {
            var Enters = DetectEnter(screenCapture);
            var EnterAndPosition = Enters.Select(x => (x, Distance(x.position)));
            if (EnterAndPosition.Any())
            {
                double minDist = Double.MaxValue;
                Point closestEnter = default;
                foreach (var (Enter, distance) in EnterAndPosition)
                {
                    if (distance < minDist)
                    {
                        minDist = distance;
                        closestEnter = Enter.position;
                    }
                }

                return closestEnter;
            }
            else
            {
                return null;
            }
        }
    }
}
