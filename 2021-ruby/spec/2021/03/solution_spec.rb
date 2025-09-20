# frozen_string_literal: true

require "spec_helper"

RSpec.describe Year2021::Day03 do
  let(:input) { File.read(File.join(File.dirname(__FILE__), "../../../challenges/2021/03/input.txt")) }
  let(:example_input) {
    <<~EOF
      00100
      11110
      10110
      10111
      10101
      01111
      00111
      11100
      10000
      11001
      00010
      01010
    EOF
  }

  describe "part 1" do
    it "returns 198 for the example input" do
      expect(described_class.part_1(example_input)).to eq(198)
    end

    it "returns an Integer for my input" do
      expect(described_class.part_1(input)).to eq(1025636)
    end
  end

  describe "part 2" do
    it "returns 230 for the example input" do
      expect(described_class.part_2(example_input)).to eq(230)
    end

    it "returns an Integer for my input" do
      expect(described_class.part_2(input)).to eq(793873)
    end
  end
end
