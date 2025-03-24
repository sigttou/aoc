using AoCHelper;
using System.Text.RegularExpressions;

namespace AdventOfCode;

public class Day24 : BaseDay
{
    private readonly string _input;

    public Day24()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var battle = new ImmuneBattle(_input);
        var (winner, units, stalemate) = battle.Fight();
        return new(units.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        int boost = 1;
        while (true)
        {
            var battle = new ImmuneBattle(_input, boost);
            var (winner, units, stalemate) = battle.Fight();

            if (winner == "Immune System" && !stalemate)
                return new(units.ToString());

            boost++;
        }
    }

    private class Group
    {
        public string Army;
        public int Units;
        public int HP;
        public List<string> Weaknesses = new();
        public List<string> Immunities = new();
        public int AttackDamage;
        public string AttackType;
        public int Initiative;
        public int Id;

        public int EffectivePower => Units * AttackDamage;

        public int CalculateDamage(Group defender)
        {
            if (defender.Immunities.Contains(AttackType)) return 0;
            int damage = EffectivePower;
            if (defender.Weaknesses.Contains(AttackType)) damage *= 2;
            return damage;
        }
    }

    private class ImmuneBattle
    {
        private readonly List<Group> _groups = new();
        private readonly int _boost;
        private int _idCounter = 1;

        public ImmuneBattle(string input, int boost = 0)
        {
            _boost = boost;
            ParseInput(input);
        }

        private void ParseInput(string input)
        {
            var sections = input.Split("\n\n");
            foreach (var section in sections)
            {
                var lines = section.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                string army = lines[0].StartsWith("Immune") ? "Immune System" : "Infection";

                foreach (var line in lines.Skip(1))
                {
                    var match = Regex.Match(line, @"(\d+) units each with (\d+) hit points (?:\(([^)]+)\) )?with an attack that does (\d+) (\w+) damage at initiative (\d+)");
                    var g = new Group
                    {
                        Army = army,
                        Units = int.Parse(match.Groups[1].Value),
                        HP = int.Parse(match.Groups[2].Value),
                        AttackDamage = int.Parse(match.Groups[4].Value) + (army == "Immune System" ? _boost : 0),
                        AttackType = match.Groups[5].Value,
                        Initiative = int.Parse(match.Groups[6].Value),
                        Id = _idCounter++
                    };

                    if (match.Groups[3].Success)
                    {
                        foreach (var part in match.Groups[3].Value.Split("; "))
                        {
                            if (part.StartsWith("weak to "))
                                g.Weaknesses.AddRange(part[8..].Split(", "));
                            else if (part.StartsWith("immune to "))
                                g.Immunities.AddRange(part[10..].Split(", "));
                        }
                    }
                    _groups.Add(g);
                }
            }
        }

        public (string Winner, int RemainingUnits, bool Stalemate) Fight()
        {
            var groups = _groups.Select(g => new Group
            {
                Army = g.Army,
                Units = g.Units,
                HP = g.HP,
                Weaknesses = new List<string>(g.Weaknesses),
                Immunities = new List<string>(g.Immunities),
                AttackDamage = g.AttackDamage,
                AttackType = g.AttackType,
                Initiative = g.Initiative,
                Id = g.Id
            }).ToList();

            while (groups.Select(g => g.Army).Distinct().Count() > 1)
            {
                var targeting = new Dictionary<Group, Group>();
                var selected = new HashSet<Group>();

                // Target selection
                foreach (var attacker in groups
                    .OrderByDescending(g => g.EffectivePower)
                    .ThenByDescending(g => g.Initiative))
                {
                    var target = groups
                        .Where(g => g.Army != attacker.Army && !selected.Contains(g))
                        .OrderByDescending(g => attacker.CalculateDamage(g))
                        .ThenByDescending(g => g.EffectivePower)
                        .ThenByDescending(g => g.Initiative)
                        .FirstOrDefault(g => attacker.CalculateDamage(g) > 0);

                    if (target != null)
                    {
                        targeting[attacker] = target;
                        selected.Add(target);
                    }
                }

                // Attacking
                var attackOrder = groups.OrderByDescending(g => g.Initiative).ToList();
                bool anyKilled = false;

                foreach (var attacker in attackOrder)
                {
                    if (attacker.Units <= 0 || !targeting.ContainsKey(attacker)) continue;
                    var defender = targeting[attacker];
                    int damage = attacker.CalculateDamage(defender);
                    int killed = Math.Min(defender.Units, damage / defender.HP);
                    if (killed > 0) anyKilled = true;
                    defender.Units -= killed;
                }

                groups = groups.Where(g => g.Units > 0).ToList();

                if (!anyKilled)
                    return ("", 0, true); // Stalemate detected
            }

            var winningArmy = groups.First().Army;
            int totalUnits = groups.Sum(g => g.Units);
            return (winningArmy, totalUnits, false);
        }
    }
}