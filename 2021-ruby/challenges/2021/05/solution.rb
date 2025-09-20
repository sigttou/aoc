# frozen_string_literal: true

module Year2021
  class Day05 < Solution
    # @input is available if you need the raw data input
    # Call `data` to access either an array of the parsed data, or a single record for a 1-line input file

    def part_1
      lines = parse_lines(data)
      count_overlaps(lines, diagonals: false)
    end

    def part_2
      lines = parse_lines(data)
      count_overlaps(lines, diagonals: true)
    end

    private

    def parse_lines(lines)
      lines.map do |l|
        l =~ /^(\d+),(\d+) -> (\d+),(\d+)$/
        [$1.to_i, $2.to_i, $3.to_i, $4.to_i]
      end
    end

    def count_overlaps(lines, diagonals: false)
      points = Hash.new(0)
      lines.each do |x1, y1, x2, y2|
        if x1 == x2
          range = (y1 <= y2) ? (y1..y2) : (y2..y1)
          range.each { |y| points[[x1, y]] += 1 }
        elsif y1 == y2
          range = (x1 <= x2) ? (x1..x2) : (x2..x1)
          range.each { |x| points[[x, y1]] += 1 }
        elsif diagonals && (x1 - x2).abs == (y1 - y2).abs
          dx = (x2 > x1) ? 1 : -1
          dy = (y2 > y1) ? 1 : -1
          len = (x1 - x2).abs
          (0..len).each { |i| points[[x1 + i * dx, y1 + i * dy]] += 1 }
        end
      end
      points.values.count { |v| v > 1 }
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
