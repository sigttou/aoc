(ns aoc-2023.day-13
  (:require [aoc-2023.helpers :as helpers]
            [clojure.string :as string]))

(def input-file-path "inputs/day_13/input")
(def sample-file-path "inputs/day_13/sample-1")

(defn parse-input
  [filename]
  (let [patterns (string/split (slurp filename) #"\n\n")]
    (map (fn [entry] (map #(apply vector %) (string/split entry #"\n")))
         patterns)))

(defn get-diff-cnt
  [a b]
  (count (filter false? (map #(= %1 %2) a b))))

(defn check-broken
  ([pattern] (check-broken pattern 0))
  ([pattern cnt]
   (reduce (fn [_ idx]
             (let [start (reverse (take idx pattern))
                   end (drop idx pattern)]
               (if (= cnt (apply + (map #(get-diff-cnt %1 %2) start end)))
                 (reduced idx)
                 0)))
           0
           (range 1 (count pattern)))))

(defn part-one
  ([] (part-one input-file-path))
  ([filename]
   (let [patterns (parse-input filename)]
     (+ (* 100 (apply + (map check-broken patterns)))
        (apply + (map #(check-broken (apply map vector %)) patterns))))))

(defn part-two
  ([] (part-two input-file-path))
  ([filename]
   (let [patterns (parse-input filename)]
     (+ (* 100 (apply + (map #(check-broken % 1) patterns)))
        (apply + (map #(check-broken (apply map vector %) 1) patterns))))))

(defn run 
  []
  (println (part-one))
  (println (part-two)))