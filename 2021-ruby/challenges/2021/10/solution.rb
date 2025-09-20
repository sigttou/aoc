# frozen_string_literal: true

module Year2021
  class Day10 < Solution
    # @input is available if you need the raw data input
    # Call `data` to access either an array of the parsed data, or a single record for a 1-line input file

    def part_1
      lines = parse_lines(data)
      score = 0
      points = {")" => 3, "]" => 57, "}" => 1197, ">" => 25137}
      lines.each do |line|
        stack = []
        line.chars.each do |ch|
          if "([{<".include?(ch)
            stack << ch
          else
            open = stack.pop
            if !match?(open, ch)
              score += points[ch]
              break
            end
          end
        end
      end
      score
    end

    def part_2
      lines = parse_lines(data)
      scores = []
      points = {")" => 1, "]" => 2, "}" => 3, ">" => 4}
      lines.each do |line|
        stack = []
        corrupted = false
        line.chars.each do |ch|
          if "([{<".include?(ch)
            stack << ch
          else
            open = stack.pop
            unless match?(open, ch)
              corrupted = true
              break
            end
          end
        end
        next if corrupted || stack.empty?
        score = 0
        stack.reverse_each do |open|
          close = {"(" => ")", "[" => "]", "{" => "}", "<" => ">"}[open]
          score = score * 5 + points[close]
        end
        scores << score
      end
      scores.sort[scores.size / 2]
    end

    private

    def parse_lines(input)
      input.is_a?(String) ? input.lines.map(&:strip) : input.map(&:strip)
    end

    def match?(open, close)
      {"(" => ")", "[" => "]", "{" => "}", "<" => ">"}[open] == close
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
