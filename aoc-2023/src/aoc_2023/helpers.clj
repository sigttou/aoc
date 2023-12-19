(ns aoc-2023.helpers
  (:require [clojure.string :as string]))

(defn reload "reload file" [] (require (ns-name *ns*) :reload-all))

(defn index-of
  "https://stackoverflow.com/a/8092016"
  [coll e]
  (first (keep-indexed #(if (= e %2) %1) coll)))

(defn get-numbers
  [num-str]
  (map #(parse-long %) (filter #(not (= "" %)) (string/split num-str #" "))))

(defn manhattan-distance
  "calculates the distance between the points a, b ([x, y])"
  [a b]
  (+ (abs (- (first a) (first b)))
     (abs (- (second a) (second b)))))