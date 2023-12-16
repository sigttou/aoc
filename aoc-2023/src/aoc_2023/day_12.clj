(ns aoc-2023.day-12
  (:require [aoc-2023.helpers :as helpers]
            [clojure.string :as string]
            [clojure.math.combinatorics :as combo]))

(def input-file-path "inputs/day_12/input")
(def sample-file-path "inputs/day_12/sample-1")
(def sample-2-file-path "inputs/day_12/sample-2")

(defn parse-input
  [filename]
  (let [entries (string/split (slurp filename) #"\n")]
    (reduce (fn [out line]
              (let [springs (first (string/split line #" "))
                    broken (map #(Integer. %) (string/split
                                               (second (string/split line #" "))
                                               #","))]
                (conj out [springs broken])))
            []
            entries)))

(defn check-fitting
  [[springs broken]]
  (= (filter #(> % 0) (map count (string/split springs #"\."))) broken))

(defn gen-springs
  [[springs broken]]
  (let [pos-cnt (count (filter #{\?} springs))]
    (reduce (fn [out to-insert]
              (conj out [(reduce (fn [new-spring replacement]
                                   (string/replace-first new-spring #"\?"
                                                         (str replacement)))
                                 springs
                                 to-insert) broken]))
            []
            (combo/selections [\. \#] pos-cnt))))

(defn part-one
  ([] (part-one input-file-path))
  ([filename]
   (apply + (map (fn [entry]
                   (->> entry
                        gen-springs
                        (filter check-fitting)
                        count))
                 (parse-input filename)))))

(defn extend-springs
  [[springs broken] val]
  [(str (string/join \? (repeat val springs)) \.)
   (flatten (repeat val broken))])

(def spring-eater
  (memoize
   (fn [springs broken cur]
     (if (empty? springs)
       (if (and (empty? broken) (= 0 cur)) 1 0)
       (+ (if (some #{(first springs)} '(\# \?))
            (spring-eater (rest springs) broken (inc cur))
            0)
          (if (and (some #{(first springs)} '(\. \?))
                   (or (and (not-empty broken) (= (first broken) cur))
                       (= 0 cur)))
            (spring-eater (rest springs) (if (= 0 cur)
                                           broken
                                           (rest broken)) 0)
            0))))))

(defn part-two
  ([] (part-two input-file-path))
  ([filename]
   (let [extended-springs (map #(extend-springs % 5) (parse-input filename))]
     (apply + (pmap #(spring-eater (first %) (second %) 0) extended-springs)))))

(defn run
  []
  (println (part-one))
  (println (part-two)))