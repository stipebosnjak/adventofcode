using System.IO;

namespace Aoc.Puzzles
{
    public class _5
    {
        private int _compoundAmount = 'A' - 'a';

        public int PartOneResult { get; set; }
        public int PartTwoResult { get; set; }

        public void Run()
        {
            var units = File.ReadAllText("Puzzles/5.txt");


            var resultPolymerOne = ProduceReaction(units);

            PartOneResult = resultPolymerOne.Length;

            var alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            var shortestPolymerLength = int.MaxValue;
            foreach (var uppercaseLetter in alphabet)
            {
                if (char.IsUpper(uppercaseLetter))
                {
                    var lowerCaseLetter = char.ToLower(uppercaseLetter);

                    var resultPolymer = ProduceReaction(units
                        .Replace(uppercaseLetter.ToString(), string.Empty)
                        .Replace(lowerCaseLetter.ToString(), string.Empty));
                    if (resultPolymer.Length < shortestPolymerLength)
                    {
                        shortestPolymerLength = resultPolymer.Length;
                    }
                }
            }

            PartTwoResult = shortestPolymerLength;
        }

        private string ProduceReaction(string units)
        {
            var reactionHappening = false;
            for (int i = units.Length - 1; i > 0; i--)
            {
                var unitA = units[i];
                var unitB = units[i - 1];

                var reaction = IsReactive(unitA, unitB);

                if (!reaction)
                    continue;

                units = units.Remove(i - 1, 2);
                i--;
                reactionHappening = true;
            }

            return reactionHappening ? ProduceReaction(units) : units;
        }

        private bool IsReactive(char a, char b)
        {
            return a + _compoundAmount == b ||
                   a - _compoundAmount == b ||
                   b + _compoundAmount == a ||
                   b - _compoundAmount == a;
        }

        public override string ToString()
        {
            return $"5. Part one:{PartOneResult}, Part two: {PartTwoResult}";
        }
    }
}