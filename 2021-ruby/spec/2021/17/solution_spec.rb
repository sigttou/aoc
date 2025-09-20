# frozen_string_literal: true

require "spec_helper"

RSpec.describe Year2021::Day17 do
  let(:input) { File.read(File.join(File.dirname(__FILE__), "../../../challenges/2021/17/input.txt")) }
  let(:example_input) {
    "target area: x=20..30, y=-10..-5"
  }

  describe "part 1" do
    it "returns 45 for the example input" do
      expect(described_class.part_1(example_input)).to eq(45)
    end

    it "returns an integer for my input" do
      expect(described_class.part_1(input)).to eq(6441)
    end
  end

  describe "part 2" do
    it "returns 112 for the example input" do
      expect(described_class.part_2(example_input)).to eq(112)
    end

    it "returns an integer for my input" do
      expect(described_class.part_2(input)).to eq(3186)
    end
  end
end
