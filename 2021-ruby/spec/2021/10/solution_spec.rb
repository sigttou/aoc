# frozen_string_literal: true

require "spec_helper"

RSpec.describe Year2021::Day10 do
  let(:input) { File.read(File.join(File.dirname(__FILE__), "../../../challenges/2021/10/input.txt")) }
  let(:example_input) {
    <<~EOF
      [({(<(())[]>[[{[]{<()<>>
      [(()[<>])]({[<{<<[]>>(
      {([(<{}[<>[]}>{[]{[(<()>
      (((({<>}<{<{<>}{[]{[]{}
      [[<[([]))<([[{}[[()]]]
      [{[{({}]{}}([{[{{{}}([]
      {<[[]]>}<{[{[{[]{()[[[]
      [<(<(<(<{}))><([]([]()
      <{([([[(<>()){}]>(<<{{
      <{([{{}}[<[[[<>{}]]]>[]]
    EOF
  }

  describe "part 1" do
    it "returns 26397 for the example input" do
      expect(described_class.part_1(example_input)).to eq(26397)
    end

    it "returns an Integer for my input" do
      expect(described_class.part_1(input)).to eq(462693)
    end
  end

  describe "part 2" do
    it "returns 288957 for the example input" do
      expect(described_class.part_2(example_input)).to eq(288957)
    end

    it "returns an Integer for my input" do
      expect(described_class.part_2(input)).to eq(3094671161)
    end
  end
end
