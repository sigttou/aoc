# frozen_string_literal: true

require "spec_helper"

RSpec.describe Year2021::Day13 do
  let(:input) { File.read(File.join(File.dirname(__FILE__), "../../../challenges/2021/13/input.txt")) }
  let(:example_input) {
    <<~EOF
      6,10
      0,14
      9,10
      0,3
      10,4
      4,11
      6,0
      6,12
      4,1
      0,13
      10,12
      3,4
      3,0
      8,4
      1,10
      2,14
      8,10
      9,0

      fold along y=7
      fold along x=5
    EOF
  }

  describe "part 1" do
    it "returns 17 for the example input" do
      expect(described_class.part_1(example_input)).to eq(17)
    end

    it "returns an Integer for my input" do
      expect(described_class.part_1(input)).to eq(618)
    end
  end

  describe "part 2" do
    it "prints the code for the example input" do
      expect { described_class.part_2(example_input) }.to output.to_stdout
    end

    it "prints the code for my input" do
      expect { described_class.part_2(input) }.to output.to_stdout
    end
  end
end
