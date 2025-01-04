(ns aoc-2023.day-21
  (:require [aoc-2023.helpers :as helpers]
            [clojure.string :as string]))

(def input-file-path "inputs/day_21/input")
(def sample-file-path "inputs/day_21/sample-1")
(def sample-2-file-path "inputs/day_21/sample-2")

(defn parse-input
  [filename]
  (let [entries (string/split (slurp filename) #"\n")]
    (reduce (fn [out y]
              (into out (reduce (fn [map x]
                                  (into map {[x y] (nth (nth entries y) x)}))
                                out
                                (range (count (first entries))))))
            {}
            (range (count entries)))))

(defn print-field
  [field]
  (let [limx (apply max (map first (keys field)))
        limy (apply max (map second (keys field)))]
    (loop [[x y] [0 0]]
      (if (= y (inc limy))
        nil
        (if (= x (inc limx))
          (do (println) (recur [0 (inc y)]))
          (do (print (get field [x y])) (recur [(inc x) y])))))))

(defn get-start
  [field]
  (first (first (filter (fn [[key val]] (if (= val \S) key false)) field))))

(defn wrap
  [size [x y]]
  [(mod x size) (mod y size)])

(defn take-step
  [field size [x y]]
  (filter #(not (nil? %)) (map (fn [pos]
                                 (if (= \. (get field (wrap size pos)))
                                   pos
                                   nil))
                               [[(dec x) y] [x (dec y)]
                                [(inc x) y] [x (inc y)]])))

(defn part-one
  ([] (part-one 64 input-file-path))
  ([steps] (part-one steps input-file-path))
  ([steps filename]
   (let [input (parse-input filename)
         start (get-start input)
         field (assoc input start \.)
         size (inc (apply max (map first (keys field))))]
     (loop [poss [start] cnt steps]
       (if (zero? cnt)
         (count poss)
         (recur (set (apply concat (map #(take-step field size %) poss)))
                (dec cnt)))))))

(defn lagrange
  [y0 y1 y2]
  [(+ (/ y0 2) (- y1) (/ y2 2))
   (+ (* -3 (/ y0 2)) (* 2 y1) (- (/ y2 2)))
   y0])

(defn part-two
  "using lagrange - following
   /r/adventofcode/comments/18nevo3/2023_day_21_solutions/keb6a53/"
  ([] (part-two input-file-path))
  ([filename]
   (let [[y0 y1 y2] [(part-one 65 filename)
                     (part-one (+ 65 131) filename)
                     (part-one (+ 65 (* 2 131)) filename)]
         [x0 x1 x2] (lagrange y0 y1 y2)
         target (/ (- 26501365 65) 131)]
     (+ (* x0 target target)
        (* x1 target)
        x2))))

(defn run
  []
  (println (part-one))
  (println (part-two)))