(ns aoc-2023.helpers
  (:require [clojure.string :as string]))

(defn reload "reload file" [] (require (ns-name *ns*) :reload-all))

(defn get-numbers
  [num-str]
  (map #(Long. %) (filter #(not (= "" %)) (string/split num-str #" "))))

(defn manhattan-distance
  "calculates the distance between the points a, b ([x, y])"
  [a b]
  (+ (abs (- (first a) (first b)))
     (abs (- (second a) (second b)))))