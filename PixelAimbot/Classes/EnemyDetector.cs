using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace PixelAimbot.Classes
{
    internal class EnemyDetector
    {
        private Image<Bgr, byte> _enemyTemplate;
        private Image<Bgr, byte> _enemyMask;
        private float _threshold;
        private readonly Point _myPosition = new Point(ChaosBot.recalcRes(150), ChaosBot.recalcRes(128, false));

       




        public EnemyDetector(Image<Bgr, byte> enemyTemplate,
            Image<Bgr, byte> enemyMask, float threshold)
        {
            this._enemyMask = enemyMask;
            this._enemyTemplate = enemyTemplate;
            this._threshold = threshold;
        }

            private List<Point> DetectEnemies(Image<Bgr, byte> screenCapture)
        {
            List<Point> enemies = new List<Point>();
            screenCapture.ROI = new Rectangle(ChaosBot.recalcRes(1593), PixelAimbot.ChaosBot.recalcRes(40, false), ChaosBot.recalcRes(296), ChaosBot.recalcRes(255, false));
            var minimap = screenCapture.Copy();
            var res = new Mat();
            double minVal = 0, maxVal = 0;
            Point minPoint = new Point();
            Point maxPoint = new Point();
            CvInvoke.MatchTemplate(minimap, this._enemyTemplate, res, TemplateMatchingType.SqdiffNormed, this._enemyMask);
            
            int h = this._enemyTemplate.Size.Height;
            int w = this._enemyTemplate.Size.Width;

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
                    enemies.Add(minPoint);
                }
            }

            return enemies;
        }

        private double Distance(Point enemy)
        {
            return Math.Sqrt((Math.Pow(enemy.X - _myPosition.X, 2) + Math.Pow(enemy.Y - _myPosition.Y, 2)));
        }

        public Point? GetClosestEnemy(Image<Bgr, byte> screenCapture)
        {
            var enemies = DetectEnemies(screenCapture);
            var enemyAndPosition = enemies.Select(x => (x, Distance(x)));
            if (enemyAndPosition.Any())
            {
                double minDist = Double.MaxValue;
                Point closestEnemy = default;
                foreach (var (enemy, distance) in enemyAndPosition)
                {
                    if (distance < minDist)
                    {
                        minDist = distance;
                        closestEnemy = enemy;
                    }
                }

                return closestEnemy;
            }
            else
            {
                return null;
            }
        }
    }
}