# frozen_string_literal: true

module Year2021
  class Day20 < Solution
    # @input is available if you need the raw data input
    # Call `data` to access either an array of the parsed data, or a single record for a 1-line input file

    def part_1(input = nil)
      algo, image = parse_input(input || @input)
      enhance(algo, image, 2)
    end

    def part_2(input = nil)
      algo, image = parse_input(input || @input)
      enhance(algo, image, 50)
    end

    private

    def parse_input(input)
      lines = input.lines.map(&:strip).reject(&:empty?)
      algo = lines.shift.chars
      image = lines.map(&:chars)
      [algo, image]
    end

    def enhance(algo, image, steps)
      lit = Set.new
      image.each_with_index do |row, y|
        row.each_with_index { |c, x| lit << [x, y] if c == "#" }
      end
      inf = "."
      steps.times do |step|
        new_lit = Set.new
        min_x, max_x = lit.map(&:first).minmax
        min_y, max_y = lit.map(&:last).minmax
        (min_x - 2..max_x + 2).each do |x|
          (min_y - 2..max_y + 2).each do |y|
            idx = 0
            (-1..1).each do |dy|
              (-1..1).each do |dx|
                idx <<= 1
                xx, yy = x + dx, y + dy
                if lit.include?([xx, yy])
                  idx |= 1
                elsif xx < min_x || xx > max_x || yy < min_y || yy > max_y
                  idx |= 1 if inf == "#"
                end
              end
            end
            new_lit << [x, y] if algo[idx] == "#"
          end
        end
        # Infinite area flips if algo[0] == '#' and algo[511] == '.'
        if algo[0] == "#" && algo[511] == "."
          inf = ((inf == ".") ? "#" : ".")
        end
        lit = new_lit
      end
      lit.size
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
