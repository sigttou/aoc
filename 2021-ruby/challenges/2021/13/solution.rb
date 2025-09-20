# frozen_string_literal: true

module Year2021
  class Day13 < Solution
    # @input is available if you need the raw data input
    # Call `data` to access either an array of the parsed data, or a single record for a 1-line input file

    def part_1
      dots, folds = parse_input(data)
      dots = apply_fold(dots, folds.first)
      dots.size
    end

    def part_2
      dots, folds = parse_input(data)
      folds.each { |f| dots = apply_fold(dots, f) }
      print_grid(dots)
    end

    private

    def parse_input(input)
      lines = input.is_a?(String) ? input.lines.map(&:strip) : input.map(&:strip)
      dots = lines.take_while { |l| l =~ /,/ }.map { |l| l.split(",").map(&:to_i) }
      folds = lines.drop_while { |l| l !~ /fold/ }.map { |l|
        m = l.match(/([xy])=(\d+)/)
        m ? [m[1], m[2].to_i] : nil
      }.compact
      [dots, folds]
    end

    def apply_fold(dots, fold)
      axis, n = fold
      dots.map do |x, y|
        if axis == "x" && x > n
          [n - (x - n), y]
        elsif axis == "y" && y > n
          [x, n - (y - n)]
        else
          [x, y]
        end
      end.uniq
    end

    def print_grid(dots)
      max_x = dots.map(&:first).max
      max_y = dots.map(&:last).max
      grid = Array.new(max_y + 1) { Array.new(max_x + 1, " ") }
      dots.each { |x, y| grid[y][x] = "#" }
      grid.each { |row| puts row.join }
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
