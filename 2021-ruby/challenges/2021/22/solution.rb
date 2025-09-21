# frozen_string_literal: true

module Year2021
  class Day22 < Solution
    # @input is available if you need the raw data input
    # Call `data` to access either an array of the parsed data, or a single record for a 1-line input file

    def part_1(input = nil)
      steps = parse_input(input || @input)
      on = Set.new
      steps.each do |(state, xr, yr, zr)|
        next if xr.min < -50 || xr.max > 50 || yr.min < -50 || yr.max > 50 || zr.min < -50 || zr.max > 50
        (xr.min..xr.max).each do |x|
          (yr.min..yr.max).each do |y|
            (zr.min..zr.max).each do |z|
              if state == "on"
                on << [x, y, z]
              else
                on.delete([x, y, z])
              end
            end
          end
        end
      end
      on.size
    end

    def part_2(input = nil)
      steps = parse_input(input || @input)
      cuboids = []
      steps.each do |(state, xr, yr, zr)|
        new_cuboids = []
        cuboids.each do |(oxr, oyr, ozr, sign)|
          ix = intersect(xr, oxr)
          iy = intersect(yr, oyr)
          iz = intersect(zr, ozr)
          if ix && iy && iz
            new_cuboids << [ix, iy, iz, -sign]
          end
        end
        if state == "on"
          new_cuboids << [xr, yr, zr, 1]
        end
        cuboids.concat(new_cuboids)
      end
      cuboids.sum { |xr, yr, zr, sign| sign * (xr.max - xr.min + 1) * (yr.max - yr.min + 1) * (zr.max - zr.min + 1) }
    end

    private

    def parse_input(input)
      input.lines.map(&:strip).reject(&:empty?).map do |line|
        state, rest = line.split(" ")
        xr, yr, zr = rest.split(",").map { |r| Range.new(*r[2..].split("..").map(&:to_i)) }
        [state, xr, yr, zr]
      end
    end

    def intersect(r1, r2)
      lo = [r1.min, r2.min].max
      hi = [r1.max, r2.max].min
      (lo <= hi) ? (lo..hi) : nil
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
