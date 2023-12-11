(ns aoc-2023.day-11
  (:require [aoc-2023.helpers :as helpers]
            [clojure.set :as set]
            [clojure.string :as string]
            [clojure.math.combinatorics :as combo]))

(def input-file-path "inputs/day_11/input")

(defn print-field
  [field]
  (doseq [line (reverse field)]
    (println line)))

(defn expand-field
  [field]
  (reduce (fn [out line]
            (if (some #{\#} line)
              (conj out line)
              (conj out line line)))
          []
          field))

(defn parse-input
  [filename]
  (let [entries (string/split (slurp filename) #"\n")]
    (map #(apply vector %) (reverse entries))))

(defn get-galaxies
  [field]
  (reduce (fn [out [y xs]]
            (reduce (fn [out x]                                                                                                                                                                (assoc out (count out) [x y]))
                    out
                    xs))
          (sorted-map)
          (keep-indexed #(identity [%1 %2])
                        (map (fn [line]
                               (keep-indexed #(if (= \# %2) %1) line))
                             field))))

(defn manhattan
  [[_ gala] [_ galb]]
  (helpers/manhattan-distance gala galb))

(defn part-one
  ([] (part-one input-file-path))
  ([filename]
   (let [field (expand-field
                (apply map vector
                       (expand-field (apply map vector
                                            (parse-input filename)))))
         galaxies (get-galaxies field)
         galaxy-pairs (combo/combinations galaxies 2)]
     (reduce (fn [sum pair]
               (+ sum (manhattan (first pair) (second pair))))
             0
             galaxy-pairs))))

(defn ext-manhattan
  [xs-to-inc ys-to-inc inc-by pair]
  (let [[xa xb] (map #(first (second %)) pair)
        [ya yb] (map #(second (second %)) pair)
        xinc (count (set/intersection
                     xs-to-inc
                     (set (apply range (sort [xa xb])))))
        yinc (count (set/intersection
                     ys-to-inc
                     (set (apply range (sort [ya yb])))))]
    (+ (abs (- xa xb)) (* inc-by xinc)
       (abs (- ya yb)) (* inc-by yinc))))

(defn part-two
  ([] (part-two input-file-path))
  ([filename]
   (let [field (parse-input filename)
         galaxies (get-galaxies field)
         galaxy-pairs (combo/combinations galaxies 2)
         ys-to-inc (set (keep-indexed #(if (nil? %2) %1) (map #(some #{\#} %)
                                                              field)))
         xs-to-inc (set (keep-indexed #(if (nil? %2) %1) (map #(some #{\#} %)
                                                              (apply map vector
                                                                     field))))]
     (reduce (fn [sum pair]
               (+ sum (ext-manhattan xs-to-inc ys-to-inc (- 1000000 1) pair)))
             0
             galaxy-pairs))))

(defn run 
  []
  (println (part-one))
  (println (part-two)))