# frozen_string_literal: true

module Year2021
  class Day01 < Solution
    # @input is available if you need the raw data input
    # Call `data` to access either an array of the parsed data, or a single record for a 1-line input file

    def part_1
      # Convert input to array of integers
      depths = data.map(&:to_i)
      # Count increases
      depths.each_cons(2).count { |a, b| b > a }
    end

    def part_2
      depths = data.map(&:to_i)
      # Sliding window of 3, compare sums
      depths.each_cons(3).map(&:sum).each_cons(2).count { |a, b| b > a }
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
