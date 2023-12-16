(ns aoc-2023.day-07
  (:require [aoc-2023.helpers :as helpers]
            [clojure.string :as string]))

(def input-file-path "inputs/day_07/input")

(def card-values
  {\2 2
   \3 3
   \4 4
   \5 5
   \6 6
   \7 7
   \8 8
   \9 9
   \T 10
   \J 11
   \Q 12
   \K 13
   \A 14 })

(def card-types
  {'(1 1 1 1 1) 1
   '(1 1 1 2) 2
   '(1 2 2) 3
   '(1 1 3) 4
   '(2 3) 5
   '(1 4) 6
   '(5) 7
   })

(defn get-hand-type
  [hand]
  (let [lengths (->> (:cards hand)
                     sort
                     (partition-by identity)
                     (map count)
                     sort)]
    (assoc hand :type (get card-types lengths))))

(defn parse-input
  [filename]
  (let [entries (string/split (slurp filename) #"\n")]
    (map (fn [entry]
            (let [cards (into [] (map #(get card-values %)
                                  (first (string/split entry #" "))))
                  bid (Integer. (second (string/split entry #" ")))
                  hand {:cards cards :bid bid :type nil}]
              (get-hand-type hand))
            ) entries)))

(defn part-one
  ([] (part-one input-file-path))
  ([filename]
   (let [sorted-hands (sort-by
                       (juxt :type
                             #(nth (:cards %) 0) #(nth (:cards %) 1)
                             #(nth (:cards %) 2) #(nth (:cards %) 3)
                             #(nth (:cards %) 4)) (parse-input filename))]
     (reduce (fn [ret [idx hand]]
               (+ ret (* idx (:bid hand))))
             0
             (map vector (drop 1 (range)) sorted-hands)))))

(defn replace-joker
  [hand]
  (assoc hand :cards (replace {11 0} (get hand :cards))))

(defn get-type-joker
  [hand]
  (let [new-type (:type (last (sort-by :type (map #(get-hand-type
                              (assoc hand :cards
                                     (replace {0 %} (:cards hand))))
                            (filter #(not (= 11 %)) (vals card-values))))))]
    (assoc hand :type new-type)))

(defn part-two
  ([] (part-two input-file-path))
  ([filename]
   (let [hands (->> (parse-input filename)
                    (map replace-joker)
                    (map get-type-joker))
         sorted-hands (sort-by
                       (juxt :type
                             #(nth (:cards %) 0) #(nth (:cards %) 1)
                             #(nth (:cards %) 2) #(nth (:cards %) 3)
                             #(nth (:cards %) 4)) hands)]
     (reduce (fn [ret [idx hand]]
               (+ ret (* idx (:bid hand))))
             0
             (map vector (drop 1 (range)) sorted-hands)))))

(defn run
  []
  (println (part-one))
  (println (part-two)))