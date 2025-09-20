# frozen_string_literal: true

require "spec_helper"

RSpec.describe Year2021::Day04 do
  let(:input) { File.read(File.join(File.dirname(__FILE__), "../../../challenges/2021/04/input.txt")) }
  let(:example_input) {
    <<~EOF
      7,4,9,5,11,17,23,2,0,14,21,24,10,16,13,6,15,25,12,22,18,20,8,19,3,26,1

      22 13 17 11  0
       8  2 23  4 24
      21  9 14 16  7
       6 10  3 18  5
       1 12 20 15 19

       3 15  0  2 22
       9 18 13 17  5
      19  8  7 25 23
      20 11 10 24  4
      14 21 16 12  6

      14 21 17 24  4
      10 16 15  9 19
      18  8 23 26 20
      22 11 13  6  5
       2  0 12  3  7
    EOF
  }

  describe "part 1" do
    it "returns 4512 for the example input" do
      expect(described_class.part_1(example_input)).to eq(4512)
    end

    it "returns an Integer for my input" do
      expect(described_class.part_1(input)).to eq(39984)
    end
  end

  describe "part 2" do
    it "returns 1924 for the example input" do
      expect(described_class.part_2(example_input)).to eq(1924)
    end

    it "returns an Integer for my input" do
      expect(described_class.part_2(input)).to eq(8468)
    end
  end
end
