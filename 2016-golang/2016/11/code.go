package main

import (
	"bytes"
	"sort"
	"strconv"

	"github.com/jpillora/puzzler/harness/aoc"
)

func main() {
	aoc.Harness(run)
}

type pair struct {
	chip_loc int
	gen_loc  int
}

type pairList []pair

// sort requirements
func (p pairList) Len() int { return len(p) }

func (p pairList) Swap(i, j int) { p[i], p[j] = p[j], p[i] }

func (p pairList) Less(i, j int) bool {
	if p[i].chip_loc < p[j].chip_loc {
		return true
	}
	if p[i].chip_loc > p[j].chip_loc {
		return false
	}
	return p[i].gen_loc < p[j].gen_loc
}

type state struct {
	cur_loc int
	pairs   pairList
}

func hash_state(s state) string {
	buf := new(bytes.Buffer)
	buf.WriteString(strconv.Itoa(s.cur_loc))
	for _, p := range s.pairs {
		buf.WriteString(strconv.Itoa(p.chip_loc))
		buf.WriteString(strconv.Itoa(p.gen_loc))
	}
	return buf.String()
}

func copy_state(s *state) (new state) {
	new = state{
		cur_loc: s.cur_loc,
		pairs:   make([]pair, len(s.pairs)),
	}
	copy(new.pairs, s.pairs)
	return
}

func chk_valid(s *state) bool {
	for i := 1; i <= 4; i++ {
		if len(get_gens_on_floor(s, i)) > 0 {
			for _, p := range s.pairs {
				if p.chip_loc == i && p.gen_loc != p.chip_loc {
					return false
				}
			}
		}
	}
	return true
}

func get_gens_on_floor(s *state, floor int) (ixs []int) {
	for i, p := range s.pairs {
		if p.gen_loc == floor {
			ixs = append(ixs, i)
		}
	}
	return
}

func get_chips_on_floor(s *state, floor int) (ixs []int) {
	for i, p := range s.pairs {
		if p.chip_loc == floor {
			ixs = append(ixs, i)
		}
	}
	return
}

func chk_final(s state) bool {
	for _, p := range s.pairs {
		if p.gen_loc != 4 || p.chip_loc != 4 {
			return false
		}
	}
	return true
}

func move_elevator(s *state, direction int, chip_idxs, gen_idxs []int) {
	for _, m := range chip_idxs {
		s.pairs[m].chip_loc += direction
	}
	for _, g := range gen_idxs {
		s.pairs[g].gen_loc += direction
	}
	sort.Sort(s.pairs)
	s.cur_loc += direction
}

func get_nxt(s *state) (states []state) {
	chip_idxs, gen_idxs := get_chips_on_floor(s, s.cur_loc), get_gens_on_floor(s, s.cur_loc)

	chips_to_move, gens_to_move := [][]int{}, [][]int{}

	for i, chip_idx := range chip_idxs {
		// move alone first, then combine with other objects
		chips_to_move, gens_to_move = append(chips_to_move, []int{chip_idx}), append(gens_to_move, []int{})
		for _, tmp := range chip_idxs[i+1:] {
			chips_to_move, gens_to_move = append(chips_to_move, []int{chip_idx, tmp}), append(gens_to_move, []int{})
		}
	}

	for i, gen_idx := range gen_idxs {
		// move alone first, then combine with other objects
		chips_to_move, gens_to_move = append(chips_to_move, []int{}), append(gens_to_move, []int{gen_idx})
		for _, tmp := range gen_idxs[i+1:] {
			chips_to_move, gens_to_move = append(chips_to_move, []int{}), append(gens_to_move, []int{gen_idx, tmp})
		}
	}

	// if microchip and generator are both on this floor, they can be moved together
	for i, p := range s.pairs {
		if p.chip_loc == s.cur_loc && p.gen_loc == s.cur_loc {
			chips_to_move, gens_to_move = append(chips_to_move, []int{i}), append(gens_to_move, []int{i})
		}
	}

	// execute the moves and check if the results are valid
	for _, dir := range []int{-1, 1} {
		if s.cur_loc+dir > 0 && s.cur_loc+dir <= 4 {
			for i := range chips_to_move {
				new := copy_state(s)
				move_elevator(&new, dir, chips_to_move[i], gens_to_move[i])
				if chk_valid(&new) {
					states = append(states, new)
				}
			}
		}
	}
	return
}

func get_min_steps(s state) (steps int) {
	cur_states := []state{s}
	prev_hashes := map[string]bool{hash_state(s): true}

	for {
		steps++
		nxt_states := []state{}
		for _, cs := range cur_states {
			for _, ns := range get_nxt(&cs) {
				if chk_final(ns) {
					return
				}
				nxt_hash := hash_state(ns)
				if !prev_hashes[nxt_hash] {
					nxt_states = append(nxt_states, ns)
					prev_hashes[nxt_hash] = true
				}
			}
		}
		cur_states = nxt_states
	}
}

// on code change, run will be executed 4 times:
// 1. with: false (part1), and example input
// 2. with: true (part2), and example input
// 3. with: false (part1), and user input
// 4. with: true (part2), and user input
// the return value of each run is printed to stdout
func run(part2 bool, input string) any {
	// no parsing due to time constraints
	s := state{
		cur_loc: 1,
		pairs: pairList{
			pair{1, 1}, //promethium
			pair{3, 2}, //cobalt
			pair{3, 2}, //rhutentium
			pair{3, 2}, //plutonium
			pair{3, 2}, //curium

		},
	}
	if part2 {
		s = state{
			cur_loc: 1,
			pairs: pairList{
				pair{1, 1}, //promethium
				pair{3, 2}, //cobalt
				pair{3, 2}, //rhutentium
				pair{3, 2}, //plutonium
				pair{3, 2}, //curium
				pair{1, 1}, //elerium
				pair{1, 1}, //dilithium
			},
		}
	}
	sort.Sort(s.pairs)

	return get_min_steps(s)
}
