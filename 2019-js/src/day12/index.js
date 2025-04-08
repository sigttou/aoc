import run from "aocrunner";

const parseInput = (rawInput) => {
  return rawInput
    .trim()
    .split("\n")
    .map((line) => {
      const [, x, y, z] = line
        .match(/<x=(-?\d+), y=(-?\d+), z=(-?\d+)>/)
        .map(Number);
      return { pos: [x, y, z], vel: [0, 0, 0] };
    });
};

const applyGravity = (moons) => {
  for (let i = 0; i < moons.length; i++) {
    for (let j = 0; j < moons.length; j++) {
      if (i === j) continue;
      for (let axis = 0; axis < 3; axis++) {
        if (moons[i].pos[axis] < moons[j].pos[axis]) moons[i].vel[axis]++;
        else if (moons[i].pos[axis] > moons[j].pos[axis]) moons[i].vel[axis]--;
      }
    }
  }
};

const applyVelocity = (moons) => {
  moons.forEach((moon) => {
    for (let axis = 0; axis < 3; axis++) {
      moon.pos[axis] += moon.vel[axis];
    }
  });
};

const totalEnergy = (moon) => {
  const potential = moon.pos.reduce((a, b) => a + Math.abs(b), 0);
  const kinetic = moon.vel.reduce((a, b) => a + Math.abs(b), 0);
  return potential * kinetic;
};

const part1 = (rawInput) => {
  const moons = parseInput(rawInput);
  for (let step = 0; step < 1000; step++) {
    applyGravity(moons);
    applyVelocity(moons);
  }
  return moons.reduce((sum, moon) => sum + totalEnergy(moon), 0);
};

const lcm = (a, b) => {
  const gcd = (x, y) => (y === 0 ? x : gcd(y, x % y));
  return (a * b) / gcd(a, b);
};

const part2 = (rawInput) => {
  const original = parseInput(rawInput);
  const steps = [0, 0, 0];
  for (let axis = 0; axis < 3; axis++) {
    const moons = original.map((m) => ({
      pos: [...m.pos],
      vel: [...m.vel],
    }));
    let step = 0;
    while (true) {
      step++;
      // Apply gravity
      for (let i = 0; i < moons.length; i++) {
        for (let j = 0; j < moons.length; j++) {
          if (i === j) continue;
          if (moons[i].pos[axis] < moons[j].pos[axis]) moons[i].vel[axis]++;
          else if (moons[i].pos[axis] > moons[j].pos[axis])
            moons[i].vel[axis]--;
        }
      }
      // Apply velocity
      moons.forEach((m) => (m.pos[axis] += m.vel[axis]));

      // Check if we're back to the original state
      if (
        moons.every(
          (m, i) => m.pos[axis] === original[i].pos[axis] && m.vel[axis] === 0,
        )
      ) {
        steps[axis] = step;
        break;
      }
    }
  }
  return lcm(steps[0], lcm(steps[1], steps[2]));
};

run({
  part1: {
    tests: [
      {
        input: `<x=-1, y=0, z=2>
<x=2, y=-10, z=-7>
<x=4, y=-8, z=8>
<x=3, y=5, z=-1>`,
        expected: 183,
      },
    ],
    solution: part1,
  },
  part2: {
    tests: [
      {
        input: `<x=-1, y=0, z=2>
<x=2, y=-10, z=-7>
<x=4, y=-8, z=8>
<x=3, y=5, z=-1>`,
        expected: 2772,
      },
    ],
    solution: part2,
  },
  trimTestInputs: true,
  onlyTests: false,
});
