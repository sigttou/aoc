# frozen_string_literal: true

module Year2021
  class Day21 < Solution
    # @input is available if you need the raw data input
    # Call `data` to access either an array of the parsed data, or a single record for a 1-line input file

    def part_1(input = nil)
      p1, p2 = parse_input(input || @input)
      scores = [0, 0]
      positions = [p1, p2]
      die = 1
      rolls = 0
      turn = 0
      while scores.max < 1000
        move = 0
        3.times do
          move += die
          die += 1
          die = 1 if die > 100
        end
        rolls += 3
        positions[turn] = (positions[turn] + move - 1) % 10 + 1
        scores[turn] += positions[turn]
        break if scores[turn] >= 1000
        turn = 1 - turn
      end
      scores.min * rolls
    end

    def part_2(input = nil)
      p1, p2 = parse_input(input || @input)
      cache = {}
      w1, w2 = quantum(p1, 0, p2, 0, 0, cache)
      [w1, w2].max
    end

    private

    def parse_input(input)
      lines = input.lines.map(&:strip).reject(&:empty?)
      p1 = lines[0].split(":").last.to_i
      p2 = lines[1].split(":").last.to_i
      [p1, p2]
    end

    # Returns [p1_wins, p2_wins]
    def quantum(p1, s1, p2, s2, turn, cache)
      key = [p1, s1, p2, s2, turn]
      return cache[key] if cache.key?(key)
      if s1 >= 21
        return [1, 0]
      elsif s2 >= 21
        return [0, 1]
      end
      wins = [0, 0]
      [1, 2, 3].product([1, 2, 3], [1, 2, 3]).each do |a, b, c|
        move = a + b + c
        if turn == 0
          np1 = (p1 + move - 1) % 10 + 1
          ns1 = s1 + np1
          w1, w2 = quantum(np1, ns1, p2, s2, 1, cache)
        else
          np2 = (p2 + move - 1) % 10 + 1
          ns2 = s2 + np2
          w1, w2 = quantum(p1, s1, np2, ns2, 0, cache)
        end
        wins[0] += w1
        wins[1] += w2
      end
      cache[key] = wins
      wins
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
