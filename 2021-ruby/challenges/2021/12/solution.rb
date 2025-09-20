# frozen_string_literal: true

module Year2021
  class Day12 < Solution
    # @input is available if you need the raw data input
    # Call `data` to access either an array of the parsed data, or a single record for a 1-line input file

    def part_1
      graph = parse_graph(data)
      count_paths(graph, allow_twice: false)
    end

    def part_2
      graph = parse_graph(data)
      count_paths(graph, allow_twice: true)
    end

    private

    def parse_graph(input)
      edges = (input.is_a?(String) ? input.lines : input).map { |l| l.strip.split("-") }
      graph = Hash.new { |h, k| h[k] = [] }
      edges.each do |a, b|
        graph[a] << b
        graph[b] << a
      end
      graph
    end

    def count_paths(graph, allow_twice: false)
      dfs = lambda do |node, visited, used_twice|
        return 1 if node == "end"
        total = 0
        graph[node].each do |nbr|
          if nbr == "start"
            next
          elsif /[A-Z]/.match?(nbr)
            total += dfs.call(nbr, visited, used_twice)
          elsif !visited.include?(nbr)
            total += dfs.call(nbr, visited | [nbr], used_twice)
          elsif allow_twice && !used_twice
            total += dfs.call(nbr, visited, true)
          end
        end
        total
      end
      dfs.call("start", Set["start"], false)
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
