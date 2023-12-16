(ns aoc-2023.day-05
  (:require [aoc-2023.helpers :as helpers]
            [clojure.string :as string]))

(def input-file-path "inputs/day_05/input")

(defn get-converter
  [entry]
  (map helpers/get-numbers (rest (string/split entry #"\n"))))

(defn parse-input
  [filename]
  (let [entries (string/split (slurp filename) #"\n\n")
        seeds (helpers/get-numbers (second (string/split (first entries) #":")))
        converters (map get-converter (rest entries))]
    {:seeds seeds
     :converters converters}))

(defn get-ranges
  [entry]
  (let [source-range (list (second entry) (+ (second entry) (last entry)))
        dest-range (list (first entry) (+ (first entry) (last entry)))]
    (list (vec source-range) (vec dest-range))))

(defn check-source
  [seed source]
  (if (and
       (>= seed (first source))
       (<= seed (last source)))
    (- seed (first source))
    -1))

(defn apply-converter
  [seed converter]
  (let [ranges (map get-ranges converter)
        sources (map first ranges)
        [sidx offset] (first (keep-indexed
                              #(if (not (= -1 %2))
                                 (list %1 %2))
                              (map #(check-source seed %) sources)))
        destinations (map last ranges)]
    (if sidx
      (+ (first (nth destinations sidx)) offset)
      seed)))

(defn part-one
  ([] (part-one input-file-path))
  ([filename]
   (let [seeds (:seeds (parse-input filename))
         converters (:converters (parse-input filename))]
     (apply min (map #(reduce (fn [seed converter]
                                (apply-converter seed converter))
                              %
                              converters) seeds)))))

(defn get-seed-ranges
  [seeds]
  (partition 2 seeds))

(defn apply-rev-converter
  [seed converter]
  (let [ranges (map get-ranges converter)
        sources (map first ranges)
        destinations (map last ranges)
        [didx offset] (first (keep-indexed
                              #(if (not (= -1 %2))
                                 (list %1 %2))
                              (map #(check-source seed %) destinations)))]
    (if didx
      (+ (first (nth sources didx)) offset)
      seed)))

(defn check-in-range
  [tochk seed-range]
  (if (and (>= tochk (first seed-range))
           (< tochk (+ (first seed-range) (last seed-range))))
    tochk
    false))

(defn check-in-ranges
  [tochk seed-ranges]
  (if (empty? (filter #(check-in-range tochk %) seed-ranges))
    false
    true))

(defn part-two
  ([] (part-two input-file-path))
  ([filename]
   (let [seed-ranges (get-seed-ranges (:seeds (parse-input filename)))
         converters (:converters (parse-input filename))
         rev-converters (reverse converters)
         min-in (first (filter #(and (check-in-ranges % seed-ranges) %)
                               (map #(reduce
                                      (fn [out rev-conv]
                                               (apply-rev-converter out
                                                                    rev-conv))
                                      %
                                      rev-converters) (range))))]
     (reduce (fn [seed converter]
               (apply-converter seed converter))
             min-in
             converters))))

(defn run
  []
  (println (part-one))
  (println (part-two)))