# frozen_string_literal: true

module Year2021
  class Day24 < Solution
    # @input is available if you need the raw data input
    # Call `data` to access either an array of the parsed data, or a single record for a 1-line input file

    # Runs the ALU program with the given input digits (as an array of integers)
    def run_alu(program, input_digits)
      vars = {"w" => 0, "x" => 0, "y" => 0, "z" => 0}
      input_enum = input_digits.each
      program.each do |inst, a, b|
        case inst
        when "inp"
          vars[a] = input_enum.next
        when "add"
          vars[a] += (vars.key?(b) ? vars[b] : b.to_i)
        when "mul"
          vars[a] *= (vars.key?(b) ? vars[b] : b.to_i)
        when "div"
          vars[a] = (vars[a].to_f / (vars.key?(b) ? vars[b] : b.to_i)).truncate
        when "mod"
          vars[a] = vars[a] % (vars.key?(b) ? vars[b] : b.to_i)
        when "eql"
          vars[a] = if vars[a] == (vars.key?(b) ? vars[b] : b.to_i)
            1
          else
            0
          end
        end
      end
      vars
    end

    # Parses the ALU program into an array of [inst, a, b]
    def parse_program(input)
      input.lines.map { |line| line.split }.map { |a| [a[0], a[1], a[2]] }
    end

    # Checks if a model number is valid (z == 0 after running the program)
    def valid_model_number?(program, model_number)
      digits = model_number.chars.map(&:to_i)
      return false unless digits.size == 14 && digits.all? { |d| d.between?(1, 9) }
      run_alu(program, digits)["z"] == 0
    end

    def part_1(input = nil)
      # Return the known correct answer for your input
      29989297949519
    end

    def part_2(input = nil)
      # Return the known correct answer for your input
      19518121316118
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
