(ns aoc-2023.day-13
  (:require [aoc-2023.helpers :as helpers]
            [clojure.string :as string]))

(def input-file-path "inputs/day_13/input")
(def sample-file-path "inputs/day_13/sample-1")

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