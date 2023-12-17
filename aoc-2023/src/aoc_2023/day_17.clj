(ns aoc-2023.day-17
  (:require [aoc-2023.helpers :as helpers]
            [clojure.string :as string]
            [clojure.data.priority-map :refer [priority-map]]))

(def input-file-path "inputs/day_17/input")
(def sample-file-path "inputs/day_17/sample-1")
(def sample-2-file-path "inputs/day_17/sample-2")

(defn parse-input
  [filename]
  (->> (string/split (slurp filename) #"\n")
       reverse
       (map #(mapv parse-long (string/split % #"")))
       (map-indexed vector)
       (map (fn [e] (map-indexed #(identity [[%1 (first e)] %2]) (second e))))
       (apply concat)
       (into {})))

(defn print-field
  [field]
  (let [limit (reduce max (map first (keys field)))]
    (loop [x 0 y limit]
      (if (= -1 y)
        nil
        (if (= limit x)
          (do (println (get field [x y])) (recur 0 (dec y)))
          (do (print (get field [x y])) (recur (inc x) y)))))))

;; dijkstra from - https://ummels.de/2014/06/08/dijkstra-in-clojure/
(defn map-vals [m f]
  (into {} (for [[k v] m] [k (f v)])))

(defn remove-keys [m pred]
  (select-keys m (filter (complement pred) (keys m))))

(defn dijkstra
  "Computes single-source shortest path distances in a directed graph.
  Given a node n, (f n) should return a map with the successors of n
  as keys and their (non-negative) distance from n as vals.
  Returns a map from nodes to their distance from start."
  [start target f]
  (loop [q (priority-map start 0) r {}]
    (if-let [[v d] (peek q)]
      (if (= target (take 2 v))
        d
        (let [dist (-> (f v) (remove-keys r) (map-vals (partial + d)))]
          (recur (merge-with min (pop q) dist) (assoc r v d))))
        r)))

(def DIRS [[0 1] [1 0] [0 -1] [-1 0]])
(def DIR_IDXS {-1 [0 1 2 3]
               0 [1 3]
               1 [0 2]
               2 [1 3]
               3 [0 2]})

(defn get-targets
  [field mindist maxdist [x y diridx]]
  (let [dirs (get DIR_IDXS diridx)]
    (reduce (fn [out dist]
              (reduce #(assoc %1 (first %2) (second %2))
                      out
                      (for [dir dirs]
                        (let [[dx dy] (nth DIRS dir)]
                          [[(+ x (* dx dist))
                            (+ y (* dy dist))
                            dir]
                           (reduce + (map #(get field (if (= 0 dx)
                                                        [x (+ y (* dy %))]
                                                        [(+ x (* dx %)) y])
                                                Integer/MAX_VALUE)
                                          (range 1 (inc dist))))]))))
            {}
            (range mindist (inc maxdist)))))

(defn part-one
  ([] (part-one input-file-path))
  ([filename]
   (let [field (parse-input filename)
         start [0 (reduce max (map first (keys field))) -1]
         target [(reduce max (map first (keys field))) 0]]
     (dijkstra start target (partial get-targets field 1 3)))))

(defn part-two
  ([] (part-two input-file-path))
  ([filename]
   (let [field (parse-input filename)
         start [0 (reduce max (map first (keys field))) -1]
         target [(reduce max (map first (keys field))) 0]]
     (dijkstra start target (partial get-targets field 4 10)))))

(defn run
  []
  (println (part-one))
  (println (part-two)))