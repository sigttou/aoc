# frozen_string_literal: true

module Year2021
  class Day04 < Solution
    # @input is available if you need the raw data input
    # Call `data` to access either an array of the parsed data, or a single record for a 1-line input file

    def part_1
      draws, boards = parse_input(data)
      play_bingo(draws, boards, :first)
    end

    def part_2
      draws, boards = parse_input(data)
      play_bingo(draws, boards, :last)
    end

    private

    def parse_input(lines)
      lines = lines.join("\n").split(/\n{2,}/)
      draws = lines[0].split(",").map(&:to_i)
      boards = lines[1..].map do |b|
        b.lines.map { |l| l.split.map(&:to_i) }
      end
      [draws, boards]
    end

    def play_bingo(draws, boards, mode)
      marks = boards.map { Array.new(5) { Array.new(5, false) } }
      winners = []
      draws.each do |num|
        boards.each_with_index do |board, bidx|
          5.times do |i|
            5.times do |j|
              marks[bidx][i][j] = true if board[i][j] == num
            end
          end
        end
        boards.each_with_index do |board, bidx|
          next if winners.include?(bidx)
          if bingo?(marks[bidx])
            winners << bidx
            if mode == :first || (mode == :last && winners.size == boards.size)
              unmarked_sum = 0
              5.times do |i|
                5.times do |j|
                  unmarked_sum += board[i][j] unless marks[bidx][i][j]
                end
              end
              return unmarked_sum * num
            end
          end
        end
      end
    end

    def bingo?(mark)
      5.times.any? { |i| mark[i].all? } || 5.times.any? { |j| mark.all? { |row| row[j] } }
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
