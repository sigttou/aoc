using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AdventOfCode
{
    public class Day04 : BaseDay
    {
        private readonly string _input;

        public Day04()
        {
            _input = File.ReadAllText(InputFilePath);
        }

        public override ValueTask<string> Solve_1()
        {
            var records = _input
                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(ParseRecord)
                .OrderBy(record => record.Timestamp)
                .ToList();

            var guardSleepMinutes = new Dictionary<int, int[]>();
            int currentGuard = 0;
            int sleepStart = 0;

            foreach (var record in records)
            {
                if (record.Action.StartsWith("Guard"))
                {
                    currentGuard = int.Parse(record.Action.Split(' ')[1].TrimStart('#'));
                    if (!guardSleepMinutes.ContainsKey(currentGuard))
                    {
                        guardSleepMinutes[currentGuard] = new int[60];
                    }
                }
                else if (record.Action == "falls asleep")
                {
                    sleepStart = record.Timestamp.Minute;
                }
                else if (record.Action == "wakes up")
                {
                    for (int i = sleepStart; i < record.Timestamp.Minute; i++)
                    {
                        guardSleepMinutes[currentGuard][i]++;
                    }
                }
            }

            var sleepiestGuard = guardSleepMinutes
                .OrderByDescending(kvp => kvp.Value.Sum())
                .First()
                .Key;

            var sleepiestMinute = Array.IndexOf(guardSleepMinutes[sleepiestGuard], guardSleepMinutes[sleepiestGuard].Max());

            return new ValueTask<string>((sleepiestGuard * sleepiestMinute).ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var records = _input
                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(ParseRecord)
                .OrderBy(record => record.Timestamp)
                .ToList();

            var guardSleepMinutes = new Dictionary<int, int[]>();
            int currentGuard = 0;
            int sleepStart = 0;

            foreach (var record in records)
            {
                if (record.Action.StartsWith("Guard"))
                {
                    currentGuard = int.Parse(record.Action.Split(' ')[1].TrimStart('#'));
                    if (!guardSleepMinutes.ContainsKey(currentGuard))
                    {
                        guardSleepMinutes[currentGuard] = new int[60];
                    }
                }
                else if (record.Action == "falls asleep")
                {
                    sleepStart = record.Timestamp.Minute;
                }
                else if (record.Action == "wakes up")
                {
                    for (int i = sleepStart; i < record.Timestamp.Minute; i++)
                    {
                        guardSleepMinutes[currentGuard][i]++;
                    }
                }
            }

            var mostFrequentGuardMinute = guardSleepMinutes
                .SelectMany(kvp => kvp.Value.Select((count, minute) => new {Guard = kvp.Key, Minute = minute, Count = count}))
                .OrderByDescending(x => x.Count)
                .First();

            return new ValueTask<string>((mostFrequentGuardMinute.Guard * mostFrequentGuardMinute.Minute).ToString());
        }

        private (DateTime Timestamp, string Action) ParseRecord(string record)
        {
            var split = record.Split(new[] {'[', ']'}, StringSplitOptions.RemoveEmptyEntries);
            var timestamp = DateTime.Parse(split[0]);
            var action = split[1].Trim();
            return (timestamp, action);
        }
    }
}