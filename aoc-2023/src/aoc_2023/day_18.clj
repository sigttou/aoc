(ns aoc-2023.day-18
  (:require [aoc-2023.helpers :as helpers]
            [clojure.string :as string]))

(def input-file-path "inputs/day_18/input")
(def sample-file-path "inputs/day_18/sample-1")

(defn parse-input
  [filename]
  (let [entries (string/split (slurp filename) #"\n")]))

(defn part-one
  ([] (part-one input-file-path))
  ([filename]))

(defn part-two
  ([] (part-two input-file-path))
  ([filename]))

(defn run
  []
  (println (part-one))
  (println (part-two)))