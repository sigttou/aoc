(ns aoc-2023.day-02
  (:require [aoc-2023.helpers :as helpers]
            [clojure.string :as string]))

(def input-file-path "inputs/day_02/input")

(def to-check
  {"red" 12
   "green" 13
   "blue" 14})

(defn get-entry
  [entry]
  (list (re-find #"[a-z]+" entry) (Integer. (re-find #"\d+" entry))))

(defn get-turn
  [entry]
  (map get-entry (string/split entry #",")))

(defn get-turns
  [entry]
  (map get-turn (string/split entry #";")))

(defn parse-input
  [filename]
  (let [lines (string/split (slurp filename) #"\n")]
    (reduce (fn [games line]
              (let [game-id (Integer. (first (re-seq #"\d+" line)))
                    turns (get-turns (second (string/split line #":")))]
                (assoc-in games [game-id] turns)))
            {}
            lines)))

(defn check-entry
  [entry]
  (<= (second entry) (get to-check (first entry))))

(defn check-turn
  [turn]
  (every? check-entry turn))

(defn part-one
  ([] (part-one input-file-path))
  ([filename]
   (let [games (parse-input filename)]
     (reduce + (reduce (fn [possible-games game]
                        (if (every? check-turn (second game))
                          (conj possible-games (first game))
                          possible-games))
                      []
                      games)))))

(defn get-min
  [colour turn]
  (if-let [entry (first (filter #(= colour (first %)) turn))]
    (second entry)
    1))

(defn get-min-val
  [turns colour]
  (apply max (map #(get-min colour %) turns)))

(defn get-game-power
  [game]
  (reduce * (map #(get-min-val (second game) (first %)) to-check)))

(defn part-two
  ([] (part-two input-file-path))
  ([filename]
   (let [games (parse-input filename)]
     (reduce + (map get-game-power games)))))

(defn run
  []
  (println (part-one))
  (println (part-two)))