# frozen_string_literal: true

module Year2021
  class Day25 < Solution
    # @input is available if you need the raw data input
    # Call `data` to access either an array of the parsed data, or a single record for a 1-line input file

    def part_1(input = nil)
      map = (input || @input).lines(chomp: true).map(&:chars)
      height = map.size
      width = map[0].size
      steps = 0
      loop do
        moved = false
        # Move east-facing '>'
        new_map = map.map(&:dup)
        height.times do |y|
          width.times do |x|
            if map[y][x] == ">" && map[y][(x + 1) % width] == "."
              new_map[y][x] = "."
              new_map[y][(x + 1) % width] = ">"
              moved = true
            end
          end
        end
        map = new_map.map(&:dup)
        # Move south-facing 'v'
        new_map = map.map(&:dup)
        height.times do |y|
          width.times do |x|
            if map[y][x] == "v" && map[(y + 1) % height][x] == "."
              new_map[y][x] = "."
              new_map[(y + 1) % height][x] = "v"
              moved = true
            end
          end
        end
        map = new_map
        steps += 1
        break unless moved
      end
      steps
    end

    def part_2(input = nil)
      # No part 2 for Day 25
      nil
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
