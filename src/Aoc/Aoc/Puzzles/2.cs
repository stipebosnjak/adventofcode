using System;
using System.IO;
using System.Linq;

namespace Aoc.Puzzles
{
    public class _2
    {
        public int Checksum { get; set; }
        public string CommonLetters { get; set; }

        public void Run()
        {
            var inputLines = File.ReadAllLines("Puzzles/2.txt");
            var matchTwoCount = 0;
            var matchThreeCount = 0;

            foreach (var inputLine in inputLines)
            {
                var charGroup = inputLine.GroupBy(x => x)
                    .Select(c => new
                    {
                        Char = c.Key,
                        Count = c.Count()
                    });

                if (charGroup.Any(x => x.Count == 2))
                {
                    matchTwoCount++;
                }
                if (charGroup.Any(x => x.Count == 3))
                {
                    matchThreeCount++;
                }
            }

            Checksum = matchTwoCount * matchThreeCount;


            var idComparison = (
                    from inputLineA in inputLines
                    from inputLineB in inputLines
                    where !inputLineA.Equals(inputLineB)
                    select new
                    {
                        A = inputLineA,
                        B = inputLineB,
                        Distance = inputLineA.CalcLevenshteinDistance(inputLineB)
                    })
                .OrderBy(x => x.Distance)
                .FirstOrDefault();

            for (var i = 0; i < idComparison.A.Length; i++)
            {
                var aValue = idComparison.A[i];
                var bValue = idComparison.B[i];
                if (aValue != bValue)
                {
                    CommonLetters = idComparison.A.Remove(i, 1);
                    break;
                }
            }



            Console.WriteLine($"2. Checksum:{Checksum}, CommonLetters:{CommonLetters}");
        }
    }
}