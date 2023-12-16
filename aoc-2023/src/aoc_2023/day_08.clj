(ns aoc-2023.day-08
  (:require [aoc-2023.helpers :as helpers]
            [clojure.string :as string]
            [clojure.math.numeric-tower :as math]))

(def input-file-path "inputs/day_08/input")

(defn get-directions
  [input]
  (reduce (fn [dirs c]
            (conj dirs (get {\L :left \R :right} c)))
          []
          input))

(defn get-nodes
  [input]
  (reduce (fn [node-map entry]
            (let [parts (string/split entry #" = ")
                  id (first parts)
                  entries (string/split (second parts) #", ")]
              (assoc node-map id
                     {:left (apply str (rest (first entries)))
                      :right (apply str (butlast (second entries)))})))
          {}
          (string/split input #"\n")))

(defn parse-input
  [filename]
  (let [entries (string/split (slurp filename) #"\n\n")
        directions (get-directions (first entries))
        nodes (get-nodes (second entries))]
    [directions nodes]))

(defn get-path-len                                                                                                                                                         [start end-test-fn nodes directions]
  (reduce (fn [[cnt id] direction]
            (if (end-test-fn id)
              (reduced [cnt id])
              [(inc cnt) (get (get nodes id) direction)]))
          [0 start]
          (cycle directions)))

(defn part-one-chk
  [str]
  (= "ZZZ" str))

(defn part-one
  ([] (part-one input-file-path))
  ([filename]
   (let [[directions nodes] (parse-input filename)]
     (first (get-path-len "AAA" part-one-chk nodes directions)))))

(defn part-two-chk
  [str]
  (string/ends-with? str "Z"))

(defn part-two
  ([] (part-two input-file-path))
  ([filename]
   (let [[directions nodes] (parse-input filename)
         starts (filter #(string/ends-with? % "A") (keys nodes))
         path-lengths (map #(get-path-len % part-two-chk nodes directions)
                           starts)]
     (reduce math/lcm (map first path-lengths)))))

(defn run
  []
  (println (part-one))
  (println (part-two)))