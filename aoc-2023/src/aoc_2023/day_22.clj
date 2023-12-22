(ns aoc-2023.day-22
  (:require [aoc-2023.helpers :as helpers]
            [clojure.string :as string]
            [clojure.set :as set]))

(def input-file-path "inputs/day_22/input")
(def sample-file-path "inputs/day_22/sample-1")

(defn parse-input
  [filename]
  (->> (string/split (slurp filename) #"\n")
       (mapv (fn [line] (vec (flatten (map #(map parse-long
                                                 (string/split % #","))
                                      (string/split line #"~"))))))))

(defn get-positions
  [brick]
  (let [[sx sy sz ex ey ez] brick]
    (vec (apply concat (mapv (fn [x]
                               (apply concat (mapv (fn [y]
                                                     (mapv (fn [z]
                                                             [x y z])
                                                           (range sz (inc ez))))
                                                   (range sy (inc ey)))))
                             (range sx (inc ex)))))))

(defn step-down
  [brick]
  (update (update brick 2 dec) 5 dec))

(defn fall
  [bricks]
  (reduce (fn [[fallen occupied] brick]
            (loop [cur brick
                   nxt (step-down cur)]
              (if (and (> (nth nxt 2) 0)
                       (not (some #(get occupied %) (get-positions nxt))))
                (recur nxt (step-down nxt))
                [(conj fallen cur)
                 (reduce #(assoc %1 %2 cur) occupied (get-positions cur))])))
          [[] {}]
          bricks))

(defn check-field
  [fallen occupied]
  (reduce (fn [[above below] brick]
            (let [curposs (get-positions brick)]
              (reduce (fn [[a b] pos]
                        (if (and (get occupied pos)
                                 (not (some #{pos} curposs)))
                          [(assoc a (get occupied pos)
                                  (conj (get a (get occupied pos) (set []))
                                        brick))
                           (assoc b brick (conj (get b brick (set []))
                                                (get occupied pos)))]
                          [a b]))
                      [above below]
                      (get-positions (step-down brick)))))
          [{} {}]
          fallen))

(defn check-fall
  [above below wouldfall brick]
  (if (some #{brick} wouldfall)
    wouldfall
    (reduce (fn [newfall abovebrick]
              (if (set/subset? (get below abovebrick) newfall)
                (check-fall above below newfall abovebrick)
                newfall))
            (conj wouldfall brick)
            (get above brick))))

(defn part-one
  ([] (part-one input-file-path))
  ([filename]
   (let [bricks (sort-by #(nth % 2) (parse-input filename))
         [fallen occupied] (fall bricks)
         [above below] (check-field fallen occupied)]
     (count (filter #(= 1 (count %))
                    (map #(check-fall above below (set []) %) fallen))))))

(defn part-two
  ([] (part-two input-file-path))
  ([filename]
   (let [bricks (sort-by #(nth % 2) (parse-input filename))
         [fallen occupied] (fall bricks)
         [above below] (check-field fallen occupied)]
     (reduce #(+ %1 (dec (count %2)))
             0 (map #(check-fall above below (set []) %) fallen)))))

(defn run
  []
  (println (part-one))
  (println (part-two)))