using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace TheConfectionRebirth.Util
{
    public static class FloodFindFuncs
    {
        public static List<Tuple<int, int>> FloodFind(Point start, int minDistance, int maxDistance)
        {
            List<Tuple<int, int>> rv = new();

            Dictionary<Point, bool> closedSet = new Dictionary<Point, bool>();
            BinaryHeap<Tuple<Point, float>> openSet = new((i, j) => i.Item2 > j.Item2);


            Dictionary<Point, float> dist = new Dictionary<Point, float>();

            openSet.Add(new(start, 0));
            closedSet[start] = true;
            while (openSet.items.Count > 0)
            {
                Tuple<Point, float> next = openSet.Pop();

                Point[] nextSet = new Point[]{
                    new(next.Item1.X, next.Item1.Y - 1),
                    new(next.Item1.X + 1, next.Item1.Y),
                    new(next.Item1.X - 1, next.Item1.Y),
                    new(next.Item1.X, next.Item1.Y + 1),
                };
                for (int i = 0; i < 4; i++)
                {
                    float lenSqr = LenSqr(nextSet[i], start);
                    if (!closedSet.TryGetValue(nextSet[i], out bool _) && Collision.IsClearSpotTest(nextSet[i].ToVector2() * 16, 1, 1, 1, true, true))
                    {
                        if (lenSqr >= minDistance * minDistance)
                            rv.Add(new(nextSet[i].X, nextSet[i].Y));
                        if (lenSqr <= maxDistance * maxDistance)
                            openSet.Add(new(nextSet[i], lenSqr));
                        closedSet[nextSet[i]] = true;
                    }
                }
            }
            return rv;
        }

        static float LenSqr(Point point1, Point point2)
        {
            Point dist = (point1 - point2);
            return dist.X * dist.X + dist.Y * dist.Y;
        }
    }
}
