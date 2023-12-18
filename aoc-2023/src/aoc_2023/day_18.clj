(ns aoc-2023.day-18
  (:require [aoc-2023.helpers :as helpers]
            [clojure.string :as string]
            [clojure.core.matrix :as matrix]))

(def input-file-path "inputs/day_18/input")
(def sample-file-path "inputs/day_18/sample-1")

(def DIRS {\R [1 0]
           \D [0 -1]
           \L [-1 0]
           \U [0 1]})

(defn parse-input
  [filename]
  (->> (string/split (slurp filename) #"\n")
       (mapv #(string/split % #" "))
       (mapv #(identity
              [(get DIRS (first (first %)))
               (parse-long (second %))
               [(read-string (apply str "0x" (take 5 (drop 2 (nth % 2)))))
                (get DIRS (nth (keys DIRS)
                     (read-string (str (last (take 8 (nth % 2)))))))]]))))

(defn gen-field
  [digplan part2]
  (second (reduce
           (if part2
             (fn [[[x y] out] [_ _ [cnt [dx dy]]]]
               (let [np [(+ x (* cnt dx))
                         (+ y (* cnt dy))]]
                 [np (conj out np)]))
             (fn [[[x y] out] [[dx dy] cnt _]]
               (let [np [(+ x (* cnt dx))
                         (+ y (* cnt dy))]]
                 [np (conj out np)])))
           [[0 0]
            [[0 0]]]
           digplan)))

(defn roll
  [v]
  (flatten [(last v) (butlast v)]))

(defn poly-area
  [xs ys]
  (* 0.5 (abs (- (matrix/dot xs (roll ys))
                 (matrix/dot ys (roll xs))))))

(defn part-one
  ([] (part-one input-file-path))
  ([filename]
   (let [plan (parse-input filename)
         points (gen-field plan nil)
         poscnt (reduce + (map second plan))
         area (poly-area (map first points) (map second points))]
     (int (+ poscnt (- (inc area) (quot poscnt 2)))))))

(defn part-two
  ([] (part-two input-file-path))
  ([filename]
   (let [plan (parse-input filename)
         points (gen-field plan true)
         poscnt (reduce + (map #(first (nth % 2)) plan))
         area (poly-area (map first points) (map second points))]
     (long (+ poscnt (- (inc area) (quot poscnt 2)))))))

(defn run
  []
  (println (part-one))
  (println (part-two)))