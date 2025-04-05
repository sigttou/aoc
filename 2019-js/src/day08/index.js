import run from "aocrunner";

const parseInput = (rawInput) => rawInput.trim();

const getLayers = (data, width, height) => {
  const layerSize = width * height;
  const layers = [];
  for (let i = 0; i < data.length; i += layerSize) {
    layers.push(data.slice(i, i + layerSize));
  }
  return layers;
};

const part1 = (rawInput) => {
  const data = parseInput(rawInput);
  const width = 25;
  const height = 6;
  const layers = getLayers(data, width, height);

  let minZeroLayer = layers[0];
  for (const layer of layers) {
    if (
      (layer.match(/0/g) || []).length < (minZeroLayer.match(/0/g) || []).length
    ) {
      minZeroLayer = layer;
    }
  }

  const ones = (minZeroLayer.match(/1/g) || []).length;
  const twos = (minZeroLayer.match(/2/g) || []).length;
  return ones * twos;
};

const renderImage = (layers, width, height) => {
  const finalImage = new Array(width * height).fill("2");

  for (const layer of layers) {
    for (let i = 0; i < layer.length; i++) {
      if (finalImage[i] === "2" && layer[i] !== "2") {
        finalImage[i] = layer[i];
      }
    }
  }

  return finalImage;
};

const part2 = (rawInput) => {
  const data = parseInput(rawInput);
  const width = 25;
  const height = 6;
  const layers = getLayers(data, width, height);
  const image = renderImage(layers, width, height);

  return image
    .map((px, i) => (px === "1" ? "█" : px === "0" ? " " : " "))
    .reduce((acc, val, i) => {
      return acc + val + ((i + 1) % width === 0 ? "\n" : "");
    }, "")
    .trim();
};

run({
  part1: {
    tests: [
      {
        input: `123456789012`,
        expected: 1 * 2,
      },
    ],
    solution: part1,
  },
  part2: {
    tests: [
      {
        input: `0222112222120000`,
        expected: `█ \n █`,
      },
    ],
    solution: part2,
  },
  trimTestInputs: true,
  onlyTests: false,
});
