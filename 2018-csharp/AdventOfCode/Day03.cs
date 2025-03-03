using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class Day03 : BaseDay
    {
        private readonly string _input;

        public Day03()
        {
            _input = File.ReadAllText(InputFilePath);
        }

        public override ValueTask<string> Solve_1()
        {
            var claims = _input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var fabric = new int[1000, 1000];

            foreach (var claim in claims)
            {
                var parts = claim.Split(' ');
                var id = int.Parse(parts[0].TrimStart('#'));
                var coords = parts[2].Split(',');
                var left = int.Parse(coords[0]);
                var top = int.Parse(coords[1].TrimEnd(':'));
                var size = parts[3].Split('x');
                var width = int.Parse(size[0]);
                var height = int.Parse(size[1]);

                for (var i = left; i < left + width; i++)
                {
                    for (var j = top; j < top + height; j++)
                    {
                        fabric[i, j]++;
                    }
                }
            }

            var overlapCount = 0;
            for (var i = 0; i < 1000; i++)
            {
                for (var j = 0; j < 1000; j++)
                {
                    if (fabric[i, j] > 1)
                    {
                        overlapCount++;
                    }
                }
            }

            return new ValueTask<string>(overlapCount.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var claims = _input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var fabric = new int[1000, 1000];
            var claimIds = new HashSet<int>();

            foreach (var claim in claims)
            {
                var parts = claim.Split(' ');
                var id = int.Parse(parts[0].TrimStart('#'));
                claimIds.Add(id);
                var coords = parts[2].Split(',');
                var left = int.Parse(coords[0]);
                var top = int.Parse(coords[1].TrimEnd(':'));
                var size = parts[3].Split('x');
                var width = int.Parse(size[0]);
                var height = int.Parse(size[1]);

                for (var i = left; i < left + width; i++)
                {
                    for (var j = top; j < top + height; j++)
                    {
                        if (fabric[i, j] != 0)
                        {
                            claimIds.Remove(fabric[i, j]);
                            claimIds.Remove(id);
                        }
                        fabric[i, j] = id;
                    }
                }
            }

            var nonOverlappingClaimId = claimIds.Single();
            return new ValueTask<string>(nonOverlappingClaimId.ToString());
        }
    }
}