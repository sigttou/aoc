# frozen_string_literal: true

require "spec_helper"

RSpec.describe Year2021::Day14 do
  let(:input) { File.read(File.join(File.dirname(__FILE__), "../../../challenges/2021/14/input.txt")) }
  let(:example_input) {
    <<~EOF
      NNCB

      CH -> B
      HH -> N
      CB -> H
      NH -> C
      HB -> C
      HC -> B
      HN -> C
      NN -> C
      BH -> H
      NC -> B
      NB -> B
      BN -> B
      BB -> N
      BC -> B
      CC -> N
      CN -> C
    EOF
  }

  describe "part 1" do
    it "returns 1588 for the example input" do
      expect(described_class.part_1(example_input)).to eq(1588)
    end

    it "returns an Integer for my input" do
      expect(described_class.part_1(input)).to eq(3587)
    end
  end

  describe "part 2" do
    it "returns 2188189693529 for the example input" do
      expect(described_class.part_2(example_input)).to eq(2188189693529)
    end

    it "returns an Integer for my input" do
      expect(described_class.part_2(input)).to eq(3906445077999)
    end
  end
end
