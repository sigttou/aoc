# frozen_string_literal: true

require "spec_helper"

RSpec.describe Year2021::Day07 do
  let(:input) { File.read(File.join(File.dirname(__FILE__), "../../../challenges/2021/07/input.txt")) }
  let(:example_input) {
    <<~EOF
      16,1,2,0,4,2,7,1,2,14
    EOF
  }

  describe "part 1" do
    it "returns 37 for the example input" do
      expect(described_class.part_1(example_input)).to eq(37)
    end

    it "returns an Integer for my input" do
      expect(described_class.part_1(input)).to eq(348664)
    end
  end

  describe "part 2" do
    it "returns 168 for the example input" do
      expect(described_class.part_2(example_input)).to eq(168)
    end

    it "returns an Integer for my input" do
      expect(described_class.part_2(input)).to eq(100220525)
    end
  end
end
