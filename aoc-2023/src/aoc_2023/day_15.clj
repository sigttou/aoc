(ns aoc-2023.day-15
  (:require [aoc-2023.helpers :as helpers]
            [clojure.string :as string]))

(def input-file-path "inputs/day_15/input")
(def sample-file-path "inputs/day_15/sample-1")

(defn parse-input
  [filename]
  (let [entries (string/split (slurp filename) #",")]
    entries))

(defn hash-str
  [in]
  (reduce (fn [val entry]
            (mod (* 17 (+ val (int entry))) 256))
          0
          in))

(defn part-one
  ([] (part-one input-file-path))
  ([filename]
   (reduce + (map hash-str (parse-input filename)))))

(defn lens-to-box
  [boxes lens]
  (let [[label val] (string/split lens #"=|-")
        hash (hash-str label)]
    (if val
      (update boxes hash (fn [box]
                           (let [idx (.indexOf (map first box) label)]
                             (if (= -1 idx)
                               (vec (conj box [label val]))
                               (assoc box idx [label val])))))
      (update boxes hash (fn [box] (vec (remove #(= label (first %)) box)))))))

(defn fill-boxes
  [lenses]
  (let [boxes (vec (repeat 256 []))]
    (reduce #(lens-to-box %1 %2)
            boxes
            lenses)))

(defn score-boxes
  [boxes]
  (reduce (fn [score box-idx]
            (+ score (reduce (fn [val [lens-idx entry]]
                               (+ val (* (inc box-idx) (inc lens-idx)
                                         (Integer. (second entry)))))
                             0
                             (keep-indexed #(identity %&)(get boxes box-idx)))))
          0
          (range (count boxes))))

(defn part-two
  ([] (part-two input-file-path))
  ([filename]
   (let [lenses (parse-input filename)]
     (->> lenses
          fill-boxes
          score-boxes))))

(defn run
  []
  (println (part-one))
  (println (part-two)))