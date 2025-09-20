# frozen_string_literal: true

require "spec_helper"

RSpec.describe Year2021::Day09 do
  let(:input) { File.read(File.join(File.dirname(__FILE__), "../../../challenges/2021/09/input.txt")) }
  let(:example_input) {
    <<~EOF
      2199943210
      3987894921
      9856789892
      8767896789
      9899965678
    EOF
  }

  describe "part 1" do
    it "returns 15 for the example input" do
      expect(described_class.part_1(example_input)).to eq(15)
    end

    it "returns an Integer for my input" do
      expect(described_class.part_1(input)).to eq(425)
    end
  end

  describe "part 2" do
    it "returns 1134 for the example input" do
      expect(described_class.part_2(example_input)).to eq(1134)
    end

    it "returns an Integer for my input" do
      expect(described_class.part_2(input)).to eq(1135260)
    end
  end
end
