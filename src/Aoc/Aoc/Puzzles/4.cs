using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Aoc.Puzzles
{
    public class _4
    {
        public int PartOneResult { get; set; }
        public int PartTwoResult { get; set; }


        public void Run()
        {
            var inputLines = File.ReadAllLines("Puzzles/4.txt");

            var logs = inputLines
                .Select(Parse).OrderBy(x => x.Time);


            var guards = new List<Guard>();
            Guard currentGuard = null;

            var logger = new StringBuilder();

            // simulate sleeping and log times
            foreach (var log in logs)
            {
                logger.Append($"[{log.Time.ToString("yyyy-MM-dd HH:mm")}]");

                if (log.GuardId != 0 && currentGuard?.Id != log.GuardId)
                {
                    if (guards.FirstOrDefault(x => x.Id == log.GuardId) != null)
                        currentGuard = guards.FirstOrDefault(x => x.Id == log.GuardId);
                    else
                    {
                        currentGuard = new Guard() {Id = Convert.ToInt32(log.GuardId)};
                        guards.Add(currentGuard);
                    }

                    logger.Append($" Guard #{log.GuardId} begins shift");
                    logger.Append(currentGuard);
                }
                else if (log.FallsAsleep.HasValue && log.FallsAsleep.Value)
                {
                    currentGuard.Sleep(log.Time);

                    logger.Append(" falls asleep");
                }
                else if (log.WakesUp.HasValue && log.WakesUp.Value)
                {
                    currentGuard.WakeUp(log.Time);

                    logger.Append(" wakes up " + currentGuard);
                }

                logger.AppendLine();
            }
            // generate debug txt, saves hair
            File.WriteAllText("Puzzles/4_Log.txt", logger.ToString());

            var sleepiestGuard = guards
                .OrderByDescending(x => x.SleepTimes.Sum(y => y.IncludedMinutes.Count()))
                .FirstOrDefault();
            var sleepiestMinute = sleepiestGuard.SleepTimes
                .SelectMany(x => x.IncludedMinutes)
                .GroupBy(x => x)
                .Select(x => new {Minute = x.Key, Count = x.Count()})
                .OrderByDescending(x => x.Count)
                .FirstOrDefault()
                .Minute;

            PartOneResult = sleepiestGuard.Id * sleepiestMinute;

            // same linq detected, maybe compute once 
            var popularMinutesByGuards = guards.GroupBy(x => x.Id)
                .Select(x => new
                {
                    GuardId = x.Key,
                    FavoriteMinute = x
                        .SelectMany(y => y.SleepTimes)
                        .SelectMany(s => s.IncludedMinutes)
                        .GroupBy(g => g)
                        .Select(u => new {Minute = u.Key, Count = u.Count()})
                        .OrderByDescending(u => u.Count)
                        .FirstOrDefault()
                });

            var popularMinuteByGuard = popularMinutesByGuards
                .Where(x => x.FavoriteMinute != null)
                .OrderByDescending(x => x.FavoriteMinute.Count)
                .FirstOrDefault();

            PartTwoResult = popularMinuteByGuard.GuardId * popularMinuteByGuard.FavoriteMinute.Minute;
        }


        [DebuggerDisplay("{DebuggerDisplay,nq}")]
        public class Guard
        {
            private SomeTime _tempSleepTime;

            public int Id { get; set; }

            public List<SomeTime> SleepTimes { get; set; }

            public Guard()
            {
                SleepTimes = new List<SomeTime>();
            }

            public void Sleep(DateTime from)
            {
                _tempSleepTime = new SomeTime() {From = from};
            }

            public void WakeUp(DateTime to)
            {
                _tempSleepTime.To = to;
                SleepTimes.Add(_tempSleepTime);
            }

            public override string ToString()
            {
                return DebuggerDisplay;
            }

            private string DebuggerDisplay =>
                $"Guard: {Id} SleepTime:{SleepTimes.Sum(x => x.Duration.Minutes)}|IncludedMinutes:{string.Join(",", SleepTimes.SelectMany(x => x.IncludedMinutes))}";
        }

        public class SomeTime
        {
            public DateTime From { get; set; }
            public DateTime To { get; set; }

            public IEnumerable<int> IncludedMinutes
            {
                get
                {
                    var fromMinute = From.Minute;
                    var range = Enumerable.Range(fromMinute, Duration.Minutes);
                    return range;
                }
            }

            public TimeSpan Duration => To.Subtract(From);
        }


        // don't look at me
        private GuardLog Parse(string inputLine)
        {
            var split = inputLine.Split(new string[] {"] "}, StringSplitOptions.RemoveEmptyEntries);
            var time = DateTime.ParseExact(split[0].TrimStart('['), "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);

            bool? fallAsleep = null;
            var guardId = 0;

            if (split[1].IndexOf('#') != -1)
            {
                guardId =
                    Convert.ToInt32(split[1].Split(new[] {" #"}, StringSplitOptions.None)[1].Split(' ')[0]);
            }
            else
            {
                fallAsleep = split[1].Contains("falls asleep");
            }

            return new GuardLog {Time = time, GuardId = guardId, FallsAsleep = fallAsleep};
        }

        /// <summary>
        /// Parse helper 
        /// </summary>
        public class GuardLog
        {
            public int GuardId { get; set; }
            public DateTime Time { get; set; }

            public bool? FallsAsleep { get; set; }
            public bool? WakesUp => !FallsAsleep;
            public bool StartsShift => !FallsAsleep.HasValue;
        }


        public override string ToString()
        {
            return $"Part One:{PartOneResult}, Part Two:{PartTwoResult}";
        }
    }
}