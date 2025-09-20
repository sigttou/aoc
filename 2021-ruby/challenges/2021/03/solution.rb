# frozen_string_literal: true

module Year2021
  class Day03 < Solution
    # @input is available if you need the raw data input
    # Call `data` to access either an array of the parsed data, or a single record for a 1-line input file

    def part_1
      nums = data.map(&:strip)
      len = nums.first.size
      gamma = +""
      epsilon = +""
      len.times do |i|
        ones = nums.count { |n| n[i] == "1" }
        zeros = nums.size - ones
        if ones > zeros
          gamma << "1"
          epsilon << "0"
        else
          gamma << "0"
          epsilon << "1"
        end
      end
      gamma.to_i(2) * epsilon.to_i(2)
    end

    def part_2
      nums = data.map(&:strip)
      oxy = filter_rating(nums, :most)
      co2 = filter_rating(nums, :least)
      oxy.to_i(2) * co2.to_i(2)
    end

    private

    def filter_rating(nums, mode)
      candidates = nums.dup
      i = 0
      while candidates.size > 1 && i < candidates.first.size
        ones = candidates.count { |n| n[i] == "1" }
        zeros = candidates.size - ones
        keep = if mode == :most
          (ones >= zeros) ? "1" : "0"
        else
          (zeros <= ones) ? "0" : "1"
        end
        candidates.select! { |n| n[i] == keep }
        i += 1
      end
      candidates.first
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
