# frozen_string_literal: true

module Year2021
  class Day02 < Solution
    # @input is available if you need the raw data input
    # Call `data` to access either an array of the parsed data, or a single record for a 1-line input file

    def part_1
      horiz = 0
      depth = 0
      data.each do |line|
        cmd, n = line.split
        n = n.to_i
        case cmd
        when "forward"
          horiz += n
        when "down"
          depth += n
        when "up"
          depth -= n
        end
      end
      horiz * depth
    end

    def part_2
      horiz = 0
      depth = 0
      aim = 0
      data.each do |line|
        cmd, n = line.split
        n = n.to_i
        case cmd
        when "forward"
          horiz += n
          depth += aim * n
        when "down"
          aim += n
        when "up"
          aim -= n
        end
      end
      horiz * depth
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
