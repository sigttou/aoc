# frozen_string_literal: true

module Year2021
  class Day06 < Solution
    # @input is available if you need the raw data input
    # Call `data` to access either an array of the parsed data, or a single record for a 1-line input file

    def part_1
      fish = parse_fish(data)
      simulate(fish, 80)
    end

    def part_2
      fish = parse_fish(data)
      simulate(fish, 256)
    end

    private

    def parse_fish(input)
      if input.is_a?(String)
        input.strip.split(",").map(&:to_i)
      elsif input.is_a?(Array)
        input.join(",").strip.split(",").map(&:to_i)
      else
        []
      end
    end

    private

    def simulate(fish, days)
      counts = Array.new(9, 0)
      fish.each { |f| counts[f] += 1 }
      days.times do
        new_fish = counts.shift
        counts[6] += new_fish
        counts << new_fish
      end
      counts.sum
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
