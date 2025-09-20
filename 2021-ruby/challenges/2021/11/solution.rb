# frozen_string_literal: true

module Year2021
  class Day11 < Solution
    # @input is available if you need the raw data input
    # Call `data` to access either an array of the parsed data, or a single record for a 1-line input file

    def part_1
      grid = parse_grid(data)
      flashes = 0
      100.times { flashes += step(grid) }
      flashes
    end

    def part_2
      grid = parse_grid(data)
      size = grid.size * grid[0].size
      step_num = 0
      loop do
        step_num += 1
        flashes = step(grid)
        return step_num if flashes == size
      end
    end

    private

    def parse_grid(input)
      (input.is_a?(String) ? input.lines : input).map { |l| l.strip.chars.map(&:to_i) }
    end

    def step(grid)
      flashed = Array.new(grid.size) { Array.new(grid[0].size, false) }
      to_flash = []
      grid.each_with_index do |row, i|
        row.each_with_index do |_, j|
          grid[i][j] += 1
          to_flash << [i, j] if grid[i][j] > 9
        end
      end
      flashes = 0
      while to_flash.any?
        i, j = to_flash.pop
        next if flashed[i][j]
        flashed[i][j] = true
        flashes += 1
        neighbors(i, j, grid.size, grid[0].size).each do |ni, nj|
          grid[ni][nj] += 1
          to_flash << [ni, nj] if grid[ni][nj] > 9 && !flashed[ni][nj]
        end
      end
      grid.each_with_index do |row, i|
        row.each_with_index do |_, j|
          grid[i][j] = 0 if flashed[i][j]
        end
      end
      flashes
    end

    def neighbors(i, j, rows, cols)
      (-1..1).flat_map { |di| (-1..1).map { |dj| [i + di, j + dj] } }
        .reject { |ni, nj| ni == i && nj == j }
        .select { |ni, nj| ni.between?(0, rows - 1) && nj.between?(0, cols - 1) }
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
