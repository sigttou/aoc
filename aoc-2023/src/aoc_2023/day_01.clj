(ns aoc-2023.day-01
  (:require [aoc-2023.helpers :as helpers]
            [clojure.string :as string]))

(def input-file-path "inputs/day_01/input")

(defn parse-input 
  [filename]
  (string/split (slurp filename) #"\n"))

(defn part-one
  ([] (part-one input-file-path))
  ([filename]
   (apply + (map #(Integer. (str (first %) (last %)))
                 (map #(re-seq #"\d" %) (parse-input filename))))))

(def replace-lookup-map
  {"one" "1"
   "two" "2"
   "three" "3"
   "four" "4"
   "five" "5"
   "six" "6"
   "seven" "7"
   "eight" "8"
   "nine" "9"})

(def num-regex #"(?=(one|two|three|four|five|six|seven|eight|nine))")

(defn replace-num
  [line replace]
  (if replace
    (clojure.string/replace line replace (get replace-lookup-map replace))
    line))

(defn get-num
  [line]
  (let [to-replace
        (map #(first (filter (complement clojure.string/blank?) %))
             (re-seq num-regex line))
        first-digit (first (re-seq #"\d" (replace-num line (first to-replace))))
        last-digit (last (re-seq #"\d" (replace-num line (last to-replace))))]
  (Integer. (str first-digit last-digit))))

(defn part-two
  ([] (part-two input-file-path))
  ([filename]
   (apply + (map get-num (parse-input filename)))))

(defn run 
  []
  (println (part-one))
  (println (part-two)))