# frozen_string_literal: true

module Year2021
  class Day14 < Solution
    # @input is available if you need the raw data input
    # Call `data` to access either an array of the parsed data, or a single record for a 1-line input file

    def part_1
      template, rules = parse_input(data)
      counts = polymerize(template, rules, 10)
      counts.values.max - counts.values.min
    end

    def part_2
      template, rules = parse_input(data)
      counts = polymerize(template, rules, 40)
      counts.values.max - counts.values.min
    end

    private

    def parse_input(input)
      lines = input.is_a?(String) ? input.lines.map(&:strip) : input.map(&:strip)
      template = lines.first
      rules = lines[2..].map { |l| l.split(" -> ") }.to_h
      [template, rules]
    end

    def polymerize(template, rules, steps)
      pair_counts = Hash.new(0)
      (0..template.size - 2).each { |i| pair_counts[template[i, 2]] += 1 }
      char_counts = Hash.new(0)
      template.chars.each { |c| char_counts[c] += 1 }
      steps.times do
        new_pairs = Hash.new(0)
        pair_counts.each do |pair, count|
          if rules[pair]
            insert = rules[pair]
            new_pairs[pair[0] + insert] += count
            new_pairs[insert + pair[1]] += count
            char_counts[insert] += count
          else
            new_pairs[pair] += count
          end
        end
        pair_counts = new_pairs
      end
      char_counts
    end

    private

    # Processes each line of the input file and stores the result in the dataset
    # def process_input(line)
    #   line.map(&:to_i)
    # end

    # Processes the dataset as a whole
    # def process_dataset(set)
    #   set
    # end
  end
end
