# frozen_string_literal: true

require "spec_helper"

RSpec.describe Year2021::Day22 do
  let(:input) { File.read(File.join(File.dirname(__FILE__), "../../../challenges/2021/22/input.txt")) }
  let(:example_input) {
    <<~EOF
      on x=10..12,y=10..12,z=10..12
      on x=11..13,y=11..13,z=11..13
      off x=9..11,y=9..11,z=9..11
      on x=10..10,y=10..10,z=10..10
    EOF
  }

  describe "part 1" do
    it "returns 39 for the example input" do
      expect(described_class.part_1(example_input)).to eq(39)
    end

    it "returns an integer for my input" do
      expect(described_class.part_1(input)).to eq(644257)
    end
  end

  describe "part 2" do
    it "returns an integer for the example input (part 2 AoC example is much larger)" do
      expect(described_class.part_2(example_input)).to be_a(Integer)
    end

    it "returns an integer for my input" do
      expect(described_class.part_2(input)).to eq(1235484513229032)
    end
  end
end
