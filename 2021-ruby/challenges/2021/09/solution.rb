# frozen_string_literal: true

module Year2021
  class Day09 < Solution
    # @input is available if you need the raw data input
    # Call `data` to access either an array of the parsed data, or a single record for a 1-line input file

    def part_1
      grid = parse_grid(data)
      low_points(grid).sum { |i, j| grid[i][j] + 1 }
    end

    def part_2
      grid = parse_grid(data)
      lows = low_points(grid)
      basins = lows.map { |i, j| basin_size(grid, i, j) }
      basins.max(3).reduce(:*)
    end

    private

    def parse_grid(input)
      (input.is_a?(String) ? input.lines : input).map { |l| l.strip.chars.map(&:to_i) }
    end

    def low_points(grid)
      points = []
      rows = grid.size
      cols = grid[0].size
      rows.times do |i|
        cols.times do |j|
          val = grid[i][j]
          adj = [[i - 1, j], [i + 1, j], [i, j - 1], [i, j + 1]].select { |x, y| x.between?(0, rows - 1) && y.between?(0, cols - 1) }
          points << [i, j] if adj.all? { |x, y| grid[x][y] > val }
        end
      end
      points
    end

    def basin_size(grid, i, j)
      seen = Set.new
      stack = [[i, j]]
      rows = grid.size
      cols = grid[0].size
      while stack.any?
        x, y = stack.pop
        next if seen.include?([x, y]) || grid[x][y] == 9
        seen << [x, y]
        [[x - 1, y], [x + 1, y], [x, y - 1], [x, y + 1]].each do |nx, ny|
          stack << [nx, ny] if nx.between?(0, rows - 1) && ny.between?(0, cols - 1)
        end
      end
      seen.size
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
