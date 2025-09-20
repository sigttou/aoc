# frozen_string_literal: true

module Year2021
  class Day08 < Solution
    # @input is available if you need the raw data input
    # Call `data` to access either an array of the parsed data, or a single record for a 1-line input file

    def part_1
      entries = parse_entries(data)
      # Unique segment counts for 1, 4, 7, 8
      unique_lengths = [2, 4, 3, 7]
      entries.sum do |patterns, output|
        output.count { |o| unique_lengths.include?(o.length) }
      end
    end

    def part_2
      entries = parse_entries(data)
      entries.sum { |patterns, output| decode_output(patterns, output) }
    end

    private

    def parse_entries(input)
      (input.is_a?(String) ? input.lines : input).map do |line|
        patterns, output = line.strip.split(" | ")
        [patterns.split.map { |p| p.chars.sort.join }, output.split.map { |o| o.chars.sort.join }]
      end
    end

    def decode_output(patterns, output)
      # Map pattern string to digit
      digits = {}
      patterns_by_len = patterns.group_by(&:length)
      digits[1] = patterns_by_len[2].first
      digits[4] = patterns_by_len[4].first
      digits[7] = patterns_by_len[3].first
      digits[8] = patterns_by_len[7].first

      # 3: length 5 and contains all of 1
      digits[3] = patterns_by_len[5].find { |p| (digits[1].chars - p.chars).empty? }
      # 9: length 6 and contains all of 4
      digits[9] = patterns_by_len[6].find { |p| (digits[4].chars - p.chars).empty? }
      # 0: length 6, not 9, and contains all of 1
      digits[0] = patterns_by_len[6].find { |p| p != digits[9] && (digits[1].chars - p.chars).empty? }
      # 6: length 6, not 9 or 0
      digits[6] = patterns_by_len[6].find { |p| p != digits[9] && p != digits[0] }
      # 5: length 5, is subset of 6
      digits[5] = patterns_by_len[5].find { |p| (p.chars - digits[6].chars).empty? }
      # 2: length 5, not 3 or 5
      digits[2] = patterns_by_len[5].find { |p| p != digits[3] && p != digits[5] }

      pattern_to_digit = digits.invert
      output.map { |o| pattern_to_digit[o] }.join.to_i
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
