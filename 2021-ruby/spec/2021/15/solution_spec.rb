# frozen_string_literal: true

require "spec_helper"

RSpec.describe Year2021::Day15 do
  let(:input) { File.read(File.join(File.dirname(__FILE__), "../../../challenges/2021/15/input.txt")) }
  let(:example_input) {
    <<~EOF
      1163751742
      1381373672
      2136511328
      3694931569
      7463417111
      1319128137
      1359912421
      3125421639
      1293138521
      2311944581
    EOF
  }

  describe "part 1" do
    it "returns 40 for the example input" do
      expect(described_class.part_1(example_input)).to eq(40)
    end

    it "returns the correct answer for my input" do
      expect(described_class.part_1(input)).to eq(540)
    end
  end

  describe "part 2" do
    it "returns 315 for the example input" do
      expect(described_class.part_2(example_input)).to eq(315)
    end

    it "returns the correct answer for my input" do
      expect(described_class.part_2(input)).to eq(2879)
    end
  end
end
