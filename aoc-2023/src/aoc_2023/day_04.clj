(ns aoc-2023.day-04
  (:require [aoc-2023.helpers :as helpers]
            [clojure.string :as string]))

(def input-file-path "inputs/day_04/input")

(defn get-card
  [line]
  (let [card-id (Integer. (last
                           (string/split (first (string/split line #":"))
                                         #" ")))
        win-nums (helpers/get-numbers
                  (first (string/split (second (string/split line #":"))
                                       #"\|")))
        my-nums (helpers/get-numbers
                 (second (string/split (second (string/split line #":"))
                                       #"\|")))]
    {:id card-id
     :win win-nums
     :my my-nums
     :cnt 1})
  )

(defn parse-input
  [filename]
  (map get-card (string/split (slurp filename) #"\n")))

(defn get-wins
  [card]
  (reduce (fn [my-wins num]
            (if (some #{num} (:win card))
              (conj my-wins num)
              my-wins))
          []
          (:my card)
  ))

(defn get-score
  [card]
  (let [my-wins (get-wins card)]
    (reduce (fn [res _]
              (if (= 0 res)
                1
                (* res 2)))
            0
            my-wins)))

(defn part-one
  ([] (part-one input-file-path))
  ([filename]
   (reduce + (map get-score (parse-input filename)))))

(defn process-card
  [cards card]
  (reduce (fn [upd-cards copy-pos]
            (if (get upd-cards copy-pos)
              (update-in upd-cards [copy-pos :cnt] #(+ (:cnt card) %))
              upd-cards))
          cards
          (range (inc (:id card))
                 (inc (+ (:id card) (count (get-wins card)))))))

(defn part-two
  ([] (part-two input-file-path))
  ([filename]
   (let [card-map (reduce (fn [m card]
                            (assoc m (:id card) card))
                          {}
                          (parse-input filename))
         copied-cards (reduce (fn [cards cur-id]
                                (process-card cards (get cards cur-id)))
                              card-map
                              (sort (map #(get (second %) :id) card-map)))]
     (reduce #(+ %1 (get (second %2) :cnt))
             0
             copied-cards))))

(defn run
  []
  (println (part-one))
  (println (part-two)))