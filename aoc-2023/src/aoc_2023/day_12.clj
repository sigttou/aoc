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
  [(str (apply str (butlast (apply str (repeat val (str springs "?"))))) ".")
   (apply concat (repeat val broken))])

(def gen-with-check
  (memoize
   (fn [springs counts pos current_count countpos]
     (if (= pos (count springs))
       (if (= (count counts) countpos)
         1
         0)
       (if (= (nth springs pos) \#)
         (gen-with-check springs counts (inc pos) (inc current_count) countpos)
         (if (or (= (nth springs pos) \.)
                 (= countpos (count counts)))
           (if (and (< countpos (count counts))
                    (= current_count (nth counts countpos)))
             (gen-with-check springs counts (inc pos) 0 (inc countpos))
             (if (= current_count 0)
               (gen-with-check springs counts (inc pos) 0 countpos)
               0))
           (let [hash_cnt (gen-with-check springs counts (inc pos)
                                          (inc current_count) countpos)]
             (+ hash_cnt
                (if (= current_count (nth counts countpos))
                  (gen-with-check springs counts (inc pos) 0 (inc countpos))
                  (if (= current_count 0)
                    (gen-with-check springs counts (inc pos) 0 countpos)
                    0))))))))))

(defn part-two
  ([] (part-two input-file-path))
  ([filename]
   (let [extended-springs (map #(extend-springs % 5) (parse-input filename))]
     (apply + (map #(gen-with-check (first %) (second %) 0 0 0)
                   extended-springs)))))

(defn run 
  []
  (println (part-one))
  (println (part-two)))