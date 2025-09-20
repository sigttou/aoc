# frozen_string_literal: true

require "spec_helper"

RSpec.describe Year2021::Day12 do
  let(:input) { File.read(File.join(File.dirname(__FILE__), "../../../challenges/2021/12/input.txt")) }
  let(:example_input) {
    <<~EOF
      start-A
      start-b
      A-c
      A-b
      b-d
      A-end
      b-end
    EOF
  }

  describe "part 1" do
    it "returns 10 for the example input" do
      expect(described_class.part_1(example_input)).to eq(10)
    end

    it "returns an Integer for my input" do
      expect(described_class.part_1(input)).to eq(3510)
    end
  end

  describe "part 2" do
    it "returns 36 for the example input" do
      expect(described_class.part_2(example_input)).to eq(36)
    end

    it "returns an Integer for my input" do
      expect(described_class.part_2(input)).to eq(122880)
    end
  end
end
