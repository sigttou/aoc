(ns aoc-2023.day-24
  (:require [aoc-2023.helpers :as helpers]
            [clojure.math.combinatorics :as combo]
            [clojure.string :as string]
            [clojure.core.matrix :as matrix]
            [clojure.core.matrix.linear :as liner]))

(matrix/set-current-implementation :vectorz)

(def input-file-path "inputs/day_24/input")
(def sample-file-path "inputs/day_24/sample-1")

(defn parse-input
  [filename]
  (let [entries (->> (string/split (slurp filename) #"\n")
                     (map #(string/split % #" @ ")))]
    (reduce (fn [out entry]
              (let [[coords velos] (map (fn [e]
                                          (map #(bigint (string/trim %))
                                               (string/split e #", +"))) entry)]
                (assoc out coords velos)))
            {}
            entries)))

(defn part-one
  ([] (part-one input-file-path 200000000000000 400000000000000))
  ([filename mi mx]
   (let [stonepairs (combo/combinations (parse-input filename) 2)]
     (count (reduce
             (fn [out [[[x1 y1 _] [dx1 dy1 _]] [[x2 y2 _] [dx2 dy2 _]]]]
               (let [nx1 (+ x1 dx1) ny1 (+ y1 dy1)
                     nx2 (+ x2 dx2) ny2 (+ y2 dy2)
                     den (- (* (- x1 nx1) (- y2 ny2))
                            (* (- y1 ny1) (- x2 nx2)))]
                 (if (= 0 den)
                   out
                   (let [x (/ (- (* (- (* x1 ny1) (* y1 nx1)) (- x2 nx2))
                                 (* (- x1 nx1) (- (* x2 ny2) (* y2 nx2))))
                              (- (* (- x1 nx1) (- y2 ny2))
                                 (* (- y1 ny1) (- x2 nx2))))
                         y (/ (- (* (- (* x1 ny1) (* y1 nx1)) (- y2 ny2))
                                 (* (- y1 ny1) (- (* x2 ny2) (* y2 nx2))))
                              (- (* (- x1 nx1) (- y2 ny2))
                                 (* (- y1 ny1) (- x2 nx2))))]
                     (if (and (>= x mi) (<= x mx) (>= y mi) (<= y mx)
                              (= (> x x1) (> nx1 x1))
                              (= (> x x2) (> nx2 x2)))
                       (conj out [x1 y1 x2 y2])
                       out)))))
             []
             stonepairs)))))

(defn solve-3-stones
  [[[[x0 y0 z0] [vx0 vy0 vz0]]
    [[x1 y1 z1] [vx1 vy1 vz1]]
    [[x2 y2 z2] [vx2 vy2 vz2]]]]
  (let [eq [[0 (- vz0 vz1) (- vy1 vy0) 0 (- z1 z0) (- y0 y1)]
            [(- vz1 vz0) 0 (- vx0 vx1) (- z0 z1) 0 (- x1 x0)]
            [(- vy0 vy1) (- vx1 vx0) 0 (- y1 y0) (- x0 x1) 0]
            [0 (- vz0 vz2) (- vy2 vy0) 0 (- z2 z0) (- y0 y2)]
            [(- vz2 vz0) 0 (- vx0 vx2) (- z0 z2) 0 (- x2 x0)]
            [(- vy0 vy2) (- vx2 vx0) 0 (- y2 y0) (- x0 x2) 0]]
        idpx0 (- (* y0 vz0) (* vy0 z0))
        idpx1 (- (* y1 vz1) (* vy1 z1))
        idpx2 (- (* y2 vz2) (* vy2 z2))
        idpy0 (- (* z0 vx0) (* vz0 x0))
        idpy1 (- (* z1 vx1) (* vz1 x1))
        idpy2 (- (* z2 vx2) (* vz2 x2))
        idpz0 (- (* x0 vy0) (* vx0 y0))
        idpz1 (- (* x1 vy1) (* vx1 y1))
        idpz2 (- (* x2 vy2) (* vx2 y2))
        sol [(- idpx0 idpx1)
             (- idpy0 idpy1)
             (- idpz0 idpz1)
             (- idpx0 idpx2)
             (- idpy0 idpy2)
             (- idpz0 idpz2)]]
    (liner/solve eq sol)))

(defn part-two
  "as Z3 seems to be too much - solved the equations, following:
   https://github.com/debbie-drg/advent-of-code-2023/blob/main/Day24/day24.py"
  ([] (part-two input-file-path))
  ([filename]
   (let [stones (parse-input filename)]
     (long (reduce + (take 3 (solve-3-stones (take 3 stones))))))))

(defn run
  []
  (println (part-one))
  (println (part-two)))