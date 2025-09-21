# frozen_string_literal: true

require "spec_helper"

RSpec.describe Year2021::Day24 do
  let(:input) { File.read(File.join(File.dirname(__FILE__), "../../../challenges/2021/24/input.txt")) }
  let(:example_input) {
    # Example from AoC Day 24 description: negates input
    <<~EOF
      inp x
      mul x -1
    EOF
  }

  describe "part 1" do
    it "negates the input for the example program" do
      # This is a direct test of the ALU, not the puzzle solution
      alu = Year2021::Day24.new("")
      program = alu.send(:parse_program, example_input)
      expect(alu.send(:run_alu, program, [5])["x"]).to eq(-5)
    end

    it "returns the correct answer for my input" do
      expect(described_class.part_1(input)).to eq(29989297949519)
    end
  end

  describe "part 2" do
    it "returns the correct answer for my input" do
      expect(described_class.part_2(input)).to eq(19518121316118)
    end
  end
end
