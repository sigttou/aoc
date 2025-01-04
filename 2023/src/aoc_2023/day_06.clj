(ns aoc-2023.day-06
  (:require [aoc-2023.helpers :as helpers]
            [clojure.string :as string]))

(def input-file-path "inputs/day_06/input")

(defn get-part-2-nums
  [num-str]
  (helpers/get-numbers (apply str (filter #(not (= \space %)) num-str))))

(defn parse-input
  [filename get-num-fun]
  (let [entries (map #(second (string/split % #":"))
                     (string/split (slurp filename) #"\n"))
        race-times (get-num-fun (first entries))
        record-distances (get-num-fun (second entries))]
    (map vector race-times record-distances)))

(defn get-distances
  [[time _]]
  (map (fn [press-time]
         (* (- time press-time) press-time)) (range (inc time))))

(defn part-one
  ([] (part-one input-file-path helpers/get-numbers))
  ([filename get-num-fun]
   (let [races (parse-input filename get-num-fun)]
     (reduce (fn [winmul race]
               (* winmul (count (filter #(> % (second race))
                                        (get-distances race)))))
             1
             races))))

(defn part-two
  ([] (part-two input-file-path))
  ([filename]
   (part-one filename get-part-2-nums)))

(defn run
  []
  (println (part-one))
  (println (part-two)))