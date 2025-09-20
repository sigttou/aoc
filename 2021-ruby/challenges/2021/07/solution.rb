# frozen_string_literal: true

module Year2021
  class Day07 < Solution
    # @input is available if you need the raw data input
    # Call `data` to access either an array of the parsed data, or a single record for a 1-line input file

    def part_1
      crabs = parse_crabs(data)
      median = crabs.sort[crabs.size / 2]
      crabs.sum { |c| (c - median).abs }
    end

    def part_2
      crabs = parse_crabs(data)
      min = crabs.min
      max = crabs.max
      (min..max).map { |pos|
        crabs.sum { |c|
          n = (c - pos).abs
          n * (n + 1) / 2
        }
      }.min
    end

    private

    def parse_crabs(input)
      if input.is_a?(String)
        input.strip.split(",").map(&:to_i)
      elsif input.is_a?(Array)
        input.join(",").strip.split(",").map(&:to_i)
      else
        []
      end
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
