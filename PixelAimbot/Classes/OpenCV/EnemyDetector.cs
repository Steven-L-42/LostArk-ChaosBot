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
    internal class EnemyDetector
    {
        private Image<Bgr, byte> _enemyTemplate;
        private Image<Bgr, byte> _enemyMask;
        private float _threshold;
        private readonly Point _myPosition = new Point(ChaosBot.recalc(150), ChaosBot.recalc(128, false));
        private DrawScreen _screenDrawer;

        public EnemyDetector(Image<Bgr, byte> enemyTemplate,
            Image<Bgr, byte> enemyMask, float threshold)
        {
            this._enemyMask = enemyMask;
            this._enemyTemplate = enemyTemplate;
            this._threshold = threshold;
            this._screenDrawer = new DrawScreen();

        }

        private List<Point> DetectEnemies(Image<Bgr, byte> screenCapture)
        {
            List<Point> enemies = new List<Point>();
            screenCapture.ROI = new Rectangle(ChaosBot.recalc(1593), PixelAimbot.ChaosBot.recalc(40, false), ChaosBot.recalc(296), ChaosBot.recalc(255, false));
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

        public Point? GetClosestEnemy(Image<Bgr, byte> screenCapture, bool showDetections = false)
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

                  //  if (showDetections)
                   // {
                        // Draw enemy detection
                        int h = this._enemyTemplate.Size.Height;
                        int w = this._enemyTemplate.Size.Width;
                        _screenDrawer.Draw(ChaosBot.recalc(1593), ChaosBot.recalc(40, false), ChaosBot.recalc(296, false), ChaosBot.recalc(255));
                        _screenDrawer.Draw(enemy.X + ChaosBot.recalc(1593), enemy.Y + ChaosBot.recalc(40, false), w, h);
                    //}
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