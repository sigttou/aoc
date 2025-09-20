# frozen_string_literal: true

module Year2021
  class Day15 < Solution
    # @input is available if you need the raw data input
    # Call `data` to access either an array of the parsed data, or a single record for a 1-line input file

    def part_1
      grid = @input.lines.map { |line| line.strip.chars.map(&:to_i) }
      dijkstra(grid)
    end

    def part_2
      grid = @input.lines.map { |line| line.strip.chars.map(&:to_i) }
      big_grid = expand_grid(grid, 5)
      dijkstra(big_grid)
    end

    private

    def dijkstra(grid)
      rows = grid.size
      cols = grid[0].size
      target = [rows - 1, cols - 1]
      dist = Array.new(rows) { Array.new(cols, Float::INFINITY) }
      dist[0][0] = 0
      visited = Set.new
      # Min-heap: [risk, row, col]
      heap = [[0, 0, 0]]
      until heap.empty?
        risk, r, c = heap.shift
        next if visited.include?([r, c])
        visited << [r, c]
        return risk if target == [r, c]
        [[0, 1], [1, 0], [-1, 0], [0, -1]].each do |dr, dc|
          nr, nc = r + dr, c + dc
          if nr.between?(0, rows - 1) && nc.between?(0, cols - 1)
            new_risk = risk + grid[nr][nc]
            if new_risk < dist[nr][nc]
              dist[nr][nc] = new_risk
              # Insert in order (simple, not optimal, but fine for AoC size)
              idx = heap.bsearch_index { |x| x[0] > new_risk } || heap.size
              heap.insert(idx, [new_risk, nr, nc])
            end
          end
        end
      end
      nil
    end

    def expand_grid(grid, times)
      rows = grid.size
      cols = grid[0].size
      big = Array.new(rows * times) { Array.new(cols * times) }
      (0...(rows * times)).each do |r|
        (0...(cols * times)).each do |c|
          val = grid[r % rows][c % cols] + (r / rows) + (c / cols)
          val = (val - 1) % 9 + 1
          big[r][c] = val
        end
      end
      big
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
