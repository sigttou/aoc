(ns aoc-2023.day-09
  (:require [aoc-2023.helpers :as helpers]
            [clojure.string :as string]))

(def input-file-path "inputs/day_09/input")

(defn parse-input
  [filename]
  (let [entries (string/split (slurp filename) #"\n")]
    (map helpers/get-numbers entries)))

(defn get-differences
  [num-list]
  (map #(- (second %) (first %)) (partition 2 1 num-list)))

(defn get-differences-list
  [diff-list]
  (let [diffs (get-differences (last diff-list))]
    (if (empty? (filter #(not (= 0 %)) diffs))
      (conj diff-list diffs)
      (get-differences-list (conj diff-list diffs)))))

(defn get-next-val
  [diffs]
  (reduce (fn [val aoc-seq]
            (+ val (last aoc-seq)))
          (last (last diffs))
          (reverse (butlast diffs))))

(defn part-one
  ([] (part-one input-file-path))
  ([filename]
   (let [analysed-diffs (map #(get-differences-list [%])
                             (parse-input filename))]
     (apply + (map get-next-val analysed-diffs)))))

(defn get-prev-val
  [diffs]
  (reduce (fn [val aoc-seq]
            (- (first aoc-seq) val))
          (first (last diffs))
          (reverse (butlast diffs))))

(defn part-two
  ([] (part-two input-file-path))
  ([filename]
   (let [analysed-diffs (map #(get-differences-list [%])
                             (parse-input filename))]
     (apply + (map get-prev-val analysed-diffs)))))

(defn run
  []
  (println (part-one))
  (println (part-two)))