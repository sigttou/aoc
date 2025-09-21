# frozen_string_literal: true

require "spec_helper"

RSpec.describe Year2021::Day21 do
  let(:input) { File.read(File.join(File.dirname(__FILE__), "../../../challenges/2021/21/input.txt")) }
  let(:example_input) {
    <<~EOF
      Player 1 starting position: 4
      Player 2 starting position: 8
    EOF
  }

  describe "part 1" do
    it "returns 739785 for the example input" do
      expect(described_class.part_1(example_input)).to eq(739785)
    end

    it "returns an integer for my input" do
      expect(described_class.part_1(input)).to eq(604998)
    end
  end

  describe "part 2" do
    it "returns 444356092776315 for the example input" do
      expect(described_class.part_2(example_input)).to eq(444356092776315)
    end

    it "returns an integer for my input" do
      expect(described_class.part_2(input)).to eq(157253621231420)
    end
  end
end
