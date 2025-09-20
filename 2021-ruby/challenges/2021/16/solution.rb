# frozen_string_literal: true

module Year2021
  class Day16 < Solution
    # @input is available if you need the raw data input
    # Call `data` to access either an array of the parsed data, or a single record for a 1-line input file

    def part_1(input = nil)
      @version_sum = 0
      bits = hex_to_bits((input || @input).gsub(/\s+/, ""))
      parse_packet(bits)
      @version_sum
    end

    def part_2(input = nil)
      @version_sum = 0
      bits = hex_to_bits((input || @input).gsub(/\s+/, ""))
      result, _ = parse_packet(bits)
      result
    end

    private

    def hex_to_bits(hex)
      hex.chars.map { |c| c.hex.to_s(2).rjust(4, "0").chars }.flatten
    end

    def parse_packet(packet)
      packet_version = packet.shift(3).join.to_i(2)
      @version_sum += packet_version
      type_id = packet.shift(3).join.to_i(2)

      if type_id == 4 # literal packet
        msg = []
        while packet.shift == "1"
          msg.concat(packet.shift(4))
        end
        msg.concat(packet.shift(4))
        [msg.join.to_i(2), packet]

      else # operator

        # length type == bits
        if packet.shift == "0"
          packet_len = packet.shift(15).join.to_i(2)
          sub_packet = packet.shift(packet_len)
          result = []
          until sub_packet.empty?
            sub_result, sub_packet = parse_packet(sub_packet)
            result << sub_result
          end

        # length type == packet count
        else
          packet_count = packet.shift(11).join.to_i(2)
          result = []
          packet_count.times do
            sub_result, packet = parse_packet(packet)
            result << sub_result
          end
        end

        case type_id
        when 0 then [result.sum, packet]
        when 1 then [result.reduce(&:*), packet]
        when 2 then [result.min, packet]
        when 3 then [result.max, packet]
        when 5 then [result.reduce(&:>) ? 1 : 0, packet]
        when 6 then [result.reduce(&:<) ? 1 : 0, packet]
        when 7 then [result.reduce(&:==) ? 1 : 0, packet]
        end
      end
    end

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
