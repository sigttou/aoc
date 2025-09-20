# frozen_string_literal: true

require "spec_helper"

RSpec.describe Year2021::Day08 do
  let(:input) { File.read(File.join(File.dirname(__FILE__), "../../../challenges/2021/08/input.txt")) }
  let(:example_input) {
    <<~EOF
      be cfbegad cbdgef fgaecd cgeb fdcge agebfd fecdb fabcd edb | fdgacbe cefdb cefbgd gcbe
      edbfga begcd cbg gc gcadebf fbgde acbgfd abcde gfcbed gfec | fcgedb cgb dgebacf gc
      fgaebd cg bdaec gdafb agbcfd gdcbef bgcad gfac gcb cdgabef | cg cg fdcagb cbg
      fbegcd cbd adcefb dageb afcb bc aefdc ecdab fgdeca fcdbega | efabcd cedba gadfec cb
      aecbfdg fbg gf bafeg dbefa fcge gcbea fcaegb dgceab fcbdga | gecf egdcabf bgf bfgea
      fgeab ca afcebg bdacfeg cfaedg gcfdb baec bfadeg bafgc acf | gebdcfa ecba ca fadegcb
      dbcfg fgd bdegcaf fgec aegbdf ecdfab fbedc dacgb gdcebf gf | cefg dcbef fcge gbcadfe
      bdfegc cbegaf gecbf dfcage bdacg ed bedf ced adcbefg gebcd | ed bcgafe cdgba cbgef
      egadfb cdbfeg cegd fecab cgb gbdefca cg fgcdab egfdb bfceg | gbdfcae bgc cg cgb
      gcafb gcf dcaebfg ecagb gf abcdeg gaef cafbge fdbac fegbdc | fgae cfgab fg bagce
    EOF
  }

  describe "part 1" do
    it "returns 26 for the example input" do
      expect(described_class.part_1(example_input)).to eq(26)
    end

    it "returns an Integer for my input" do
      expect(described_class.part_1(input)).to eq(330)
    end
  end

  describe "part 2" do
    it "returns 61229 for the example input" do
      expect(described_class.part_2(example_input)).to eq(61229)
    end

    it "returns an Integer for my input" do
      expect(described_class.part_2(input)).to eq(1010472)
    end
  end
end
