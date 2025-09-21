# frozen_string_literal: true

module Year2021
  class Day19 < Solution
    def part_1(input = nil)
      solve(input)[:beacon_count]
    end

    def part_2(input = nil)
      solve(input)[:max_distance]
    end

    private

    def solve(input)
      return @solution if @solution

      scanners = input ? parse_input(input) : data

      # Start with scanner 0 as the field (reference frame)
      field = Set.new(scanners.shift)
      scanner_coords = []

      # Create neighbor maps for remaining scanners
      scanner_maps = scanners.map { |scanner| [scanner, create_neighbor_map(scanner)] }

      # Process scanners until all are oriented
      until scanner_maps.empty?
        field_map = create_neighbor_map(field.to_a)
        scanner, field_neighbors, scanner_neighbors = find_matching_neighbors(field_map, scanner_maps)

        # Remove the matched scanner
        scanner_maps.reject! { |s, _| s == scanner }

        # Calculate orientation and scanner position
        orientation = calculate_orientation(field_neighbors, scanner_neighbors)
        scanner_coord, _, _ = orientation
        scanner_coords << scanner_coord

        # Add reoriented beacons to field
        reoriented_beacons = reorient_coords(orientation, scanner)
        field.merge(reoriented_beacons)
      end

      # Calculate maximum Manhattan distance between scanners
      max_distance = find_max_distance(scanner_coords)

      @solution = {beacon_count: field.size, max_distance: max_distance}
    end

    def parse_input(input)
      input.strip.split("\n\n").map do |group|
        lines = group.split("\n")
        lines[1..].map { |line| line.split(",").map(&:to_i) }
      end
    end

    def manhattan_distance(coord_a, coord_b)
      coord_a.zip(coord_b).sum { |a, b| (a - b).abs }
    end

    def euclidean_distance(coord_a, coord_b)
      Math.sqrt(coord_a.zip(coord_b).sum { |a, b| (a - b)**2 })
    end

    def find_max_distance(coords)
      max_distance = 0
      coords.combination(2) do |coord_a, coord_b|
        distance = manhattan_distance(coord_a, coord_b)
        max_distance = [max_distance, distance].max
      end
      max_distance
    end

    def create_neighbor_map(coords)
      neighbor_map = {}

      coords.each do |coord|
        # Calculate distances to all other coordinates
        peer_distances = {}
        coords.each do |peer|
          next if peer == coord
          peer_distances[euclidean_distance(coord, peer)] = peer
        end

        # Get two nearest neighbors
        distances = peer_distances.keys.sort
        next if distances.length < 2

        distance_a, distance_b = distances[0, 2]
        neighbor_a = peer_distances[distance_a]
        neighbor_b = peer_distances[distance_b]

        # Create unique hash based on distances between the three points
        hash = (distance_a + distance_b) * euclidean_distance(neighbor_a, neighbor_b)
        neighbor_map[hash] = [coord, neighbor_a, neighbor_b]
      end

      neighbor_map
    end

    def find_matching_neighbors(field_map, scanner_maps)
      field_map.each do |field_hash, field_neighbors|
        scanner_maps.each do |scanner, scanner_map|
          scanner_map.each do |scanner_hash, scanner_neighbors|
            if (field_hash - scanner_hash).abs < 0.0001  # Float comparison
              return [scanner, field_neighbors, scanner_neighbors]
            end
          end
        end
      end

      raise "No matching neighbors found"
    end

    def calculate_orientation(field_neighbors, scanner_neighbors)
      offset = [nil, nil, nil]
      direction = [nil, nil, nil]
      rotation = [nil, nil, nil]

      # Try each coordinate position (x, y, z)
      3.times do |pos|
        next if offset[pos]

        # Try each rotation possibility
        3.times do |rotation_pos|
          # Try both direction flips
          [-1, 1].each do |direction_flip|
            # Calculate offsets for all three neighbor pairs
            offsets = Set.new
            3.times do |index|
              field_coord = field_neighbors[index][pos]
              scanner_coord = scanner_neighbors[index][rotation_pos] * direction_flip
              offsets.add(field_coord - scanner_coord)
            end

            # If all offsets are the same, we found the correct orientation
            if offsets.size == 1
              offset[pos] = offsets.first
              direction[pos] = direction_flip
              rotation[pos] = rotation_pos
              break
            end
          end
          break if offset[pos]
        end
      end

      [offset, direction, rotation]
    end

    def reorient_coords(orientation, coords)
      offset, direction, rotation = orientation

      coords.map do |coord|
        3.times.map do |index|
          # Apply rotation, direction flip, and offset
          coord[rotation[index]] * direction[index] + offset[index]
        end
      end
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
