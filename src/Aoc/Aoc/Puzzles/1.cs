using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aoc.Puzzles
{
    public class _1
    {
        public HashSet<int> FrequencyHistory { get; set; }

        public int? FirstRepeated { get; set; }
        public void Run()
        {
            var inputLines = File.ReadAllLines("Puzzles/input.txt").ToList();

            var frequency = 0;
            FrequencyHistory = new HashSet<int>();

            while (FirstRepeated == null)
            {
                frequency = CalculateFrequency(inputLines, frequency);
            }
        }

        private int CalculateFrequency(List<string> inputLines, int frequency = 0)
        {
            foreach (var inputLine in inputLines)
            {
                var previousCount = FrequencyHistory.Count;

                var frequencyChange = Convert.ToInt32(inputLine.Substring(1));
                if (inputLine.Substring(0, 1) == "+")
                {
                    frequency += frequencyChange;
                }
                else
                {
                    frequency -= frequencyChange;
                }

                FrequencyHistory.Add(frequency);
                
                // if hashset count did not change then it is repeated
                if (previousCount == FrequencyHistory.Count && FirstRepeated == null)
                {
                    FirstRepeated = frequency;
                }
            }

            return frequency;
        }
    }
}