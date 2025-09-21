# frozen_string_literal: true

require "spec_helper"

RSpec.describe Year2021::Day25 do
  let(:input) { File.read(File.join(File.dirname(__FILE__), "../../../challenges/2021/25/input.txt")) }
  let(:example_input) {
    <<~EOF
      v...>>.vv>
      .vv>>.vv..
      >>.>v>...v
      >>v>>.>.v.
      v>v.vv.v..
      >.>>..v...
      .vv..>.>v.
      v.v..>>v.v
      ....v..v.>
    EOF
  }

  describe "part 1" do
    it "returns 58 for the example input" do
      expect(described_class.part_1(example_input)).to eq(58)
    end

    it "returns 329 for my input" do
      expect(described_class.part_1(input)).to eq(329)
    end
  end

  describe "part 2" do
    it "returns nil for the example input" do
      expect(described_class.part_2(example_input)).to eq(nil)
    end

    it "returns nil for my input" do
      expect(described_class.part_2(input)).to eq(nil)
    end
  end
end
