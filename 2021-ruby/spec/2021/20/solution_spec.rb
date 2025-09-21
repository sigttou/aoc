# frozen_string_literal: true

require "spec_helper"

RSpec.describe Year2021::Day20 do
  let(:input) { File.read(File.join(File.dirname(__FILE__), "../../../challenges/2021/20/input.txt")) }
  let(:example_input) {
    <<~EOF
      ..#.#..#####.#.#.#.###.##.....###.##.#..###.####..#####..#....#..#..##..##
      #..######.###...####..#..#####..##..#.#####...##.#.#..#.##..#.#......#.###
      .######.###.####...#.##.##..#..#..#####.....#.#....###..#.##......#.....#.
      .#..#..##..#...##.######.####.####.#.#...#.......#..#.#.#...####.##.#.....
      .#..#...##.#.##..#...##.#.##..###.#......#.#.......#.#.#.####.###.##...#..
      ...####.#..#..#.##.#....##..#.####....##...##..#...#......#.#.......#.....
      ..##..####..#...#.#.#...##..#.#..###..#####........#..####......#..#

      #..#.
      #....
      ##..#
      ..#..
      ..###
    EOF
  }

  describe "part 1" do
    # it "returns 35 for the example input" do
    #   expect(described_class.part_1(example_input)).to eq(35)
    # end

    it "returns an integer for my input" do
      expect(described_class.part_1(input)).to eq(5479)
    end
  end

  describe "part 2" do
    # it "returns 3351 for the example input" do
    #   expect(described_class.part_2(example_input)).to eq(3351)
    # end

    it "returns an integer for my input" do
      expect(described_class.part_2(input)).to eq(19012)
    end
  end
end
