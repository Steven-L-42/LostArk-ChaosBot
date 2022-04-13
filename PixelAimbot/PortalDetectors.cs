using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace PixelAimbot
{
    internal class PortalDetectors
    {
        private Image<Bgr, byte> _PortalTemplate;
        private Image<Bgr, byte> _PortalMask;
        private float _thresh;
        private readonly Point _mePosition = new Point(1920, 1080);
        public PortalDetectors(Image<Bgr, byte> PortalTemplate,
           Image<Bgr, byte> PortalMask, float thresh)
        {
            this._PortalMask = PortalMask;
            this._PortalTemplate = PortalTemplate;
            this._thresh = thresh;
        }
        private List<Point> DetectPortal(Image<Bgr, byte> screenCapture)
        {
            List<Point> Portals = new List<Point>();
            screenCapture.ROI = new Rectangle(50, 124, 223, 252);
            var minimap = screenCapture.Copy();
            var res = new Mat();
            double minVal = 0, maxVal = 0;
            Point minPoint = new Point();
            Point maxPoint = new Point();
            CvInvoke.MatchTemplate(minimap, this._PortalTemplate, res, TemplateMatchingType.SqdiffNormed, this._PortalMask);

            int h = this._PortalTemplate.Size.Height;
            int w = this._PortalTemplate.Size.Width;

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
                    Portals.Add(minPoint);
                }
            }

            return Portals;
        }
        private double Distance(Point Portal)
        {
            return Math.Sqrt((Math.Pow(Portal.X - _mePosition.X, 2) + Math.Pow(Portal.Y - _mePosition.Y, 2)));
        }
        public Point? GetClosestPortal(Image<Bgr, byte> screenCapture)
        {
            var Portals = DetectPortal(screenCapture);
            var PortalAndPosition = Portals.Select(x => (x, Distance(x)));
            if (PortalAndPosition.Any())
            {
                double minDist = Double.MaxValue;
                Point closestPortal = default;
                foreach (var (Portal, distance) in PortalAndPosition)
                {
                    if (distance < minDist)
                    {
                        minDist = distance;
                        closestPortal = Portal;
                    }
                }

                return closestPortal;
            }
            else
            {
                return null;
            }
        }
    }
}
