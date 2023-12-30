(ns aoc-2023.day-16
  (:require [aoc-2023.helpers :as helpers]
            [clojure.string :as string]
            [clojure.set :as set]))

(def input-file-path "inputs/day_16/input")
(def sample-file-path "inputs/day_16/sample-1")

(defn print-field
  [field]
  (reduce #(println %2) nil (reverse field)))

(defn print-energized
  [xs ys energized]
  (for [y (reverse (range ys))]
    (println (apply str (map (fn [x] (if (some #{[x y]} energized)
                                       \#
                                       \.)) (range xs))))))

(defn parse-input
  [filename]
  (let [entries (string/split (slurp filename) #"\n")]
    (reverse entries)))

(defn take-step
  [field energized [x y] [dx dy]]
  (if (some #{[[x y] [dx dy]]} energized)
    energized
    (let [[nx ny] [(+ x dx) (+ y dy)]
          nenergized (conj energized [[x y] [dx dy]])]
      (if (or (>= ny (count field))
              (< ny 0)
              (>= nx (count (first field)))
              (< nx 0))
        nenergized
        (let [nsym (nth (nth field ny) nx)
              [ndx ndy] (case nsym
                          \- (if (#{0} dx)
                               [dy 0]
                               [dx dy])
                          \| (if (#{0} dy)
                               [0 dx]
                               [dx dy])
                          \\ [(- dy) (- dx)]
                          \/ [dy dx]
                          [dx dy])]
          (if (or (and (= \- nsym) (#{0} dx))
                  (and (= \| nsym) (#{0} dy)))
            (recur field (set/union nenergized
                                    (take-step field nenergized [nx ny]
                                               [(- ndx) (- ndy)]))
                   [nx ny] [ndx ndy])
            (recur field nenergized [nx ny] [ndx ndy])))))))

(defn part-one
  ([] (part-one input-file-path))
  ([filename]
   (let [field (parse-input filename)
         energized (take-step field (set []) [0 (dec (count field))] [1 0])]
     (count (set (map first energized))))))

(defn get-starting-pos
  [field]
  (concat (map (fn [x] [[x 0] [0 1]]) (range (count (first field))))
          (map (fn [x] [[x (dec (count field))] [0 -1]])
               (range (count (first field))))
          (map (fn [y] [[0 y] [1 0]]) (range (count field)))
          (map (fn [y] [[(dec (count (first field))) y] [-1 0]])
               (range (count field)))))

(defn part-two
  ([] (part-two input-file-path))
  ([filename]
   (let [field (parse-input filename)
         starting-pos (get-starting-pos field)]
     (reduce max (pmap (fn [[[sx sy] [sdx sdy]]]
                         (count (set
                                 (map first
                                      (take-step field (set [])
                                                 [sx sy] [sdx sdy])))))
                       starting-pos)))))

(defn run
  []
  (println (part-one))
  (println (part-two)))