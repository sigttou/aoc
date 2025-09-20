# frozen_string_literal: true

require "spec_helper"

RSpec.describe Year2021::Day18 do
  let(:input) { File.read(File.join(File.dirname(__FILE__), "../../../challenges/2021/18/input.txt")) }
  let(:example_input) {
    <<~EOF
      [[[0,[5,8]],[[1,7],[9,6]]],[[4,[1,2]],[[1,4],2]]]
      [[[5,[2,8]],4],[5,[[9,9],0]]]
      [6,[[[6,2],[5,6]],[[7,6],[4,7]]]]
      [[[6,[0,7]],[0,9]],[4,[9,[9,0]]]]
      [[[7,[6,4]],[3,[1,3]]],[[[5,5],1],9]]
      [[6,[[7,3],[3,2]]],[[[3,8],[5,7]],4]]
      [[[[5,4],[7,7]],8],[[8,3],8]]
      [[9,3],[[9,9],[6,[4,9]]]]
      [[2,[[7,7],7]],[[5,8],[[9,3],[0,2]]]]
      [[[[5,2],5],[8,[3,7]]],[[5,[7,5]],[4,4]]]
    EOF
  }

  describe "part 1" do
    it "returns 4140 for the example input" do
      expect(described_class.part_1(example_input)).to eq(4140)
    end

    it "returns an integer for my input" do
      expect(described_class.part_1(input)).to eq(3892)
    end
  end

  describe "part 2" do
    it "returns 3993 for the example input" do
      expect(described_class.part_2(example_input)).to eq(3993)
    end

    it "returns an integer for my input" do
      expect(described_class.part_2(input)).to eq(4909)
    end
  end
end
