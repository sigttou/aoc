# frozen_string_literal: true

require "spec_helper"

RSpec.describe Year2021::Day16 do
  let(:input) { File.read(File.join(File.dirname(__FILE__), "../../../challenges/2021/16/input.txt")) }
  let(:example_input) {
    "8A004A801A8002F478"
  }

  describe "part 1" do
    it "returns 16 for the example input" do
      expect(described_class.part_1(example_input)).to eq(16)
    end

    it "returns an integer for my input" do
      expect(described_class.part_1(input)).to eq(860)
    end
  end

  describe "part 2" do
    it "returns the correct value for the example input C200B40A82" do
      expect(described_class.part_2("C200B40A82")).to eq(3)
    end
    it "returns the correct value for the example input 04005AC33890" do
      expect(described_class.part_2("04005AC33890")).to eq(54)
    end
    it "returns the correct value for the example input 880086C3E88112" do
      expect(described_class.part_2("880086C3E88112")).to eq(7)
    end
    it "returns the correct value for the example input CE00C43D881120" do
      expect(described_class.part_2("CE00C43D881120")).to eq(9)
    end
    it "returns the correct value for the example input D8005AC2A8F0" do
      expect(described_class.part_2("D8005AC2A8F0")).to eq(1)
    end
    it "returns the correct value for the example input F600BC2D8F" do
      expect(described_class.part_2("F600BC2D8F")).to eq(0)
    end
    it "returns the correct value for the example input 9C005AC2F8F0" do
      expect(described_class.part_2("9C005AC2F8F0")).to eq(0)
    end
    it "returns the correct value for the example input 9C0141080250320F1802104A08" do
      expect(described_class.part_2("9C0141080250320F1802104A08")).to eq(1)
    end
    it "returns an integer for my input" do
      expect(described_class.part_2(input)).to eq(470949537659)
    end
  end
end
