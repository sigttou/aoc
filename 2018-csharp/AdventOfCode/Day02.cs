using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class Day02 : BaseDay
    {
        private readonly string _input;

        public Day02()
        {
            _input = File.ReadAllText(InputFilePath);
        }

        public override ValueTask<string> Solve_1()
        {
            var boxIds = _input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            int countTwo = 0;
            int countThree = 0;

            foreach (var id in boxIds)
            {
                var letterCounts = id.GroupBy(c => c).Select(g => g.Count()).ToList();
                if (letterCounts.Contains(2)) countTwo++;
                if (letterCounts.Contains(3)) countThree++;
            }

            int checksum = countTwo * countThree;
            return new ValueTask<string>(checksum.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var boxIds = _input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            
            for (int i = 0; i < boxIds.Length; i++)
            {
                for (int j = i + 1; j < boxIds.Length; j++)
                {
                    string commonLetters = GetCommonLetters(boxIds[i], boxIds[j]);
                    if (commonLetters.Length == boxIds[i].Length - 1)
                    {
                        return new ValueTask<string>(commonLetters);
                    }
                }
            }
            
            return new ValueTask<string>("No matching box IDs found");
        }

        private string GetCommonLetters(string id1, string id2)
        {
            var commonLetters = new System.Text.StringBuilder();
            for (int i = 0; i < id1.Length; i++)
            {
                if (id1[i] == id2[i])
                {
                    commonLetters.Append(id1[i]);
                }
            }
            return commonLetters.ToString();
        }
    }
}