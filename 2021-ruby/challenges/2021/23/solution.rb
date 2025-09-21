# frozen_string_literal: true

module Year2021
  class Day23 < Solution
    class Burrow
      AMPHIPOD_TYPES = {"A" => 1, "B" => 10, "C" => 100, "D" => 1000}

      # Paths from each hallway position to each room
      PATHS = {
        0 => {"A" => [0, 1], "B" => [0, 1, 2], "C" => [0, 1, 2, 3], "D" => [0, 1, 2, 3, 4]},
        1 => {"A" => [1], "B" => [1, 2], "C" => [1, 2, 3], "D" => [1, 2, 3, 4]},
        2 => {"A" => [2], "B" => [2], "C" => [2, 3], "D" => [2, 3, 4]},
        3 => {"A" => [3, 2], "B" => [3], "C" => [3], "D" => [3, 4]},
        4 => {"A" => [4, 3, 2], "B" => [4, 3], "C" => [4], "D" => [4]},
        5 => {"A" => [5, 4, 3, 2], "B" => [5, 4, 3], "C" => [5, 4], "D" => [5]},
        6 => {"A" => [6, 5, 4, 3, 2], "B" => [6, 5, 4, 3], "C" => [6, 5, 4], "D" => [6, 5]}
      }

      attr_accessor :state, :cost, :state_hash

      def initialize(state = nil, cost = 0)
        @state = state ? Marshal.load(Marshal.dump(state)) : {}
        @cost = cost
        @state_hash = calculate_state_hash
      end

      def <=>(other)
        @cost <=> other.cost
      end

      def self.move_cost(hallway_position, room_type, room_position, amphipod_type)
        distance = 2 * PATHS[hallway_position][room_type].length + room_position
        distance -= 1 if [0, 6].include?(hallway_position)
        distance * AMPHIPOD_TYPES[amphipod_type]
      end

      def calculate_state_hash
        sorted_state = @state.sort.map { |k, v| [k, v] }
        sorted_state.hash
      end

      def winner?
        AMPHIPOD_TYPES.keys.all? do |room_type|
          @state[room_type].all? { |amphipod| amphipod == room_type }
        end
      end

      def possible_moves
        next_possible_states = []

        # First priority: move amphipods from hallway to their target rooms
        (0...@state["H"].length).each do |hall_pos|
          amphipod_type = @state["H"][hall_pos]
          next unless amphipod_type

          if room_open?(amphipod_type) && path_to_room_clear?(hall_pos, amphipod_type)
            room_pos = next_spot_in_room(amphipod_type)
            new_burrow = Burrow.new(@state, @cost)
            new_burrow.move_amphipod("H", hall_pos, amphipod_type, room_pos)
            return [new_burrow]  # High priority move, return immediately
          end
        end

        # Second priority: move amphipods from rooms to hallway
        AMPHIPOD_TYPES.keys.each do |room|
          next if room_open?(room)

          # Find the first amphipod in the room
          amphipod_type = @state[room].find { |a| a }
          room_pos = @state[room].index(amphipod_type)

          PATHS.keys.each do |hall_pos|
            if !@state["H"][hall_pos] && path_to_room_clear?(hall_pos, room)
              new_burrow = Burrow.new(@state, @cost)
              new_burrow.move_amphipod(room, room_pos, "H", hall_pos)
              next_possible_states << new_burrow
            end
          end
        end

        next_possible_states
      end

      def move_amphipod(from_room, from_pos, to_room, to_pos)
        if from_room == "H"
          # Moving from hallway to room
          @cost += self.class.move_cost(from_pos, to_room, to_pos, to_room)
        else
          # Moving from room to hallway
          amphipod_type = @state[from_room][from_pos]
          @cost += self.class.move_cost(to_pos, from_room, from_pos, amphipod_type)
        end

        @state[to_room][to_pos] = @state[from_room][from_pos]
        @state[from_room][from_pos] = nil
        @state_hash = calculate_state_hash
      end

      def room_open?(room)
        @state[room].all? { |amphipod| amphipod.nil? || amphipod == room }
      end

      def next_spot_in_room(room)
        # Find the deepest available spot
        @state[room].length - 1 - @state[room].reverse.index(nil)
      end

      def path_to_room_clear?(hallway_start, end_room_type)
        PATHS[hallway_start][end_room_type].each do |position|
          next if position == hallway_start
          return false if @state["H"][position]
        end
        true
      end
    end

    def part_1(input = nil)
      start_state = input ? parse_input(input) : data
      burrow = Burrow.new(start_state)
      result = find_path(burrow)
      result.cost
    end

    def part_2(input = nil)
      start_state = input ? parse_input(input) : data

      # Insert the additional amphipods for part 2
      insertions = {
        "A" => ["D", "D"],
        "B" => ["C", "B"],
        "C" => ["B", "A"],
        "D" => ["A", "C"]
      }

      insertions.each do |room, additional|
        start_state[room] = [start_state[room][0]] + additional + [start_state[room][1]]
      end

      burrow = Burrow.new(start_state)
      result = find_path(burrow)
      result.cost
    end

    private

    def find_path(burrow)
      queue = [burrow]
      visited = Set.new

      until queue.empty?
        # Simple priority queue - find minimum cost
        current_idx = queue.each_with_index.min_by { |b, _| b.cost }[1]
        current_burrow = queue.delete_at(current_idx)

        return current_burrow if current_burrow.winner?

        unless visited.include?(current_burrow.state_hash)
          visited.add(current_burrow.state_hash)
          current_burrow.possible_moves.each { |move| queue << move }
        end
      end

      nil
    end

    def parse_input(input)
      lines = input.strip.split("\n")

      # Extract amphipod positions from the input format
      row1_match = lines[2].match(/#([A-D])#([A-D])#([A-D])#([A-D])#/)
      row2_match = lines[3].match(/#([A-D])#([A-D])#([A-D])#([A-D])#/)

      {
        "H" => [nil] * 7,  # 7 hallway positions
        "A" => [row1_match[1], row2_match[1]],
        "B" => [row1_match[2], row2_match[2]],
        "C" => [row1_match[3], row2_match[3]],
        "D" => [row1_match[4], row2_match[4]]
      }
    end

    # Processes each line of the input file and stores the result in the dataset
    def process_input(line)
      line
    end

    # Processes the dataset as a whole
    def process_dataset(set)
      parse_input(set.join("\n"))
    end
  end
end
