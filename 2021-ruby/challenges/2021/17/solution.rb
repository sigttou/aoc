# frozen_string_literal: true

module Year2021
  class Day17 < Solution
    # @input is available if you need the raw data input
    # Call `data` to access either an array of the parsed data, or a single record for a 1-line input file

    def part_1(input = nil)
      _, _, y1, _ = parse_target(input || @input)
      # The highest y is achieved by shooting up and falling into the target
      # The optimal vy is abs(y1)-1 (lowest point of target)
      vy = y1.abs - 1
      vy * (vy + 1) / 2
    end

    def part_2(input = nil)
      x1, x2, y1, y2 = parse_target(input || @input)
      count = 0
      # Reasonable bounds for vx and vy
      (1..x2).each do |vx0|
        (y1..y1.abs).each do |vy0|
          count += 1 if hits_target?(vx0, vy0, x1, x2, y1, y2)
        end
      end
      count
    end

    private

    def parse_target(input)
      m = input.match(/x=(-?\d+)\.\.(-?\d+), y=(-?\d+)\.\.(-?\d+)/)
      [m[1].to_i, m[2].to_i, m[3].to_i, m[4].to_i]
    end

    def hits_target?(vx0, vy0, x1, x2, y1, y2)
      x = y = 0
      vx = vx0
      vy = vy0
      loop do
        x += vx
        y += vy
        vx -= 1 if vx > 0
        vx += 1 if vx < 0
        vy -= 1
        return true if x.between?(x1, x2) && y1 <= y && y <= y2
        break if x > x2 || y < y1
      end
      false
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
