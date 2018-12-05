using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aoc.Puzzles
{
    public class _3
    {
        public int OverlapFabric { get; set; }
        public Claim IntactClaim { get; set; }

        public void Run()
        {
            var inputLines = File.ReadAllLines("Puzzles/3.txt");
            var claims = new List<Claim>();

            foreach (var inputLine in inputLines)
            {
                // TODO: Regex #48 @ 165,673: 23x14
                var split = inputLine.Split(' ');
                var id = split[0].TrimStart('#');
                var left = split[2].Split(',')[0];
                var top = split[2].Split(',')[1].TrimEnd(':');
                var width = split[3].Split('x')[0];
                var height = split[3].Split('x')[1];

                claims.Add(new Claim
                {
                    Id = id,

                    Rectangle = new Rectangle(
                        Convert.ToInt32(left),
                        Convert.ToInt32(top),
                        Convert.ToInt32(width),
                        Convert.ToInt32(height))
                });
            }

            var overlapPoints = new HashSet<Point>(new PointComparer());
            var overlappingClaims = new List<Claim>();
            foreach (var claimA in claims)
            {
                foreach (var claimB in claims)
                {
                    if (claimA.Id == claimB.Id)
                        continue;

                    var overlapRect = Rectangle.Intersect(claimA.Rectangle, claimB.Rectangle);
                    if (overlapRect.IsEmpty)
                        continue;

                    overlappingClaims.Add(claimA);

                    foreach (var point in overlapRect.Points())
                    {
                        overlapPoints.Add(point);
                    }
                }
            }
            OverlapFabric = overlapPoints.Count;
            IntactClaim = claims.Except(overlappingClaims).FirstOrDefault();
        }
    }

    public class Claim
    {
        public string Id { get; set; }
        public Rectangle Rectangle { get; set; }
    }
}