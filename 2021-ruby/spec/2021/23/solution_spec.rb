# frozen_string_literal: true

require "spec_helper"

RSpec.describe Year2021::Day23 do
  let(:input) { File.read(File.join(File.dirname(__FILE__), "../../../challenges/2021/23/input.txt")) }
  let(:example_input) {
    <<~EOF
      #############
      #...........#
      ###B#C#B#D###
        #A#D#C#A#
        #########
    EOF
  }

  describe "part 1" do
    it "returns 12521 for the example input" do
      expect(described_class.part_1(example_input)).to eq(12521)
    end

    it "returns an integer for my input" do
      expect(described_class.part_1(input)).to eq(14148)
    end
  end

  describe "part 2" do
    it "returns 44169 for the example input" do
      expect(described_class.part_2(example_input)).to eq(44169)
    end

    it "returns an integer for my input" do
      expect(described_class.part_2(input)).to eq(43814)
    end
  end
end
