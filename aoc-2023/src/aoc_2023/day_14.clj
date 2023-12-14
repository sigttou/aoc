(ns aoc-2023.day-14
  (:require [aoc-2023.helpers :as helpers]
            [clojure.string :as string]
            [clojure.set :as set]))

(def input-file-path "inputs/day_14/input")
(def sample-file-path "inputs/day_14/sample-1")

(defn parse-input
  [filename]
  (let [entries (string/split (slurp filename) #"\n")]
    (apply vector (map #(apply vector %) entries))))

(defn print-field
  [field]
  (reduce (fn [_ e]
            (println e))
          0
          field))

(defn roll-north
  [field line]
  (reduce (fn [out idx]
            (let [rockidxs (map first (filter
                                       #(= (second %) \O)
                                       (map-indexed vector
                                                    (nth out (inc idx)))))
                  freeidxs (map first (filter
                                       #(= (second %) \.)
                                       (map-indexed vector
                                                    (nth out idx))))
                  to-replace (set/intersection (set rockidxs) (set freeidxs))
                  first-step
                  (reduce (fn [outfield repl-idx]
                            (assoc-in outfield [idx repl-idx] \O))
                          out
                          to-replace)]
              (reduce (fn [outfield repl-idx]
                        (assoc-in outfield [(inc idx) repl-idx] \.))
                      first-step
                      to-replace)))
          field
          (reverse (range 0 line))))

(defn roll-field
  [field]
  (reduce (fn [out idx]
            (roll-north out idx))
          field
          (range 1 (count field))))

(defn score-field
  [field]
  (reduce + (map * (reverse (range 1 (inc (count field))))
                 (map (fn [e] (count (filter #(= \O %) e)))
                      field))))

(defn part-one
  ([] (part-one input-file-path))
  ([filename]
   (let [field (parse-input filename)]
     (score-field (roll-field field)))))

(def rotate-field
  (memoize (fn [field]
             (apply vector (map #(apply vector (reverse %))
                                (apply map vector field))))))

(def cycle-tilt
  (memoize (fn [field]
             (let [north (roll-field field)
                   west (roll-field (rotate-field north))
                   south (roll-field (rotate-field west))
                   east (roll-field (rotate-field south))]
               (rotate-field east)))))

(defn part-two
  ([] (part-two input-file-path))
  ([filename]
   (let [field (parse-input filename)]
     (score-field (reduce (fn [out _]
                            (cycle-tilt out))
                          field
                          (range 1000000000))))))


  (defn run
    []
    (println (part-one))
    (println (part-two)))