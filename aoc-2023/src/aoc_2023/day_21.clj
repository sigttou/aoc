(ns aoc-2023.day-21
  (:require [aoc-2023.helpers :as helpers]
            [clojure.string :as string]))

(def input-file-path "inputs/day_21/input")
(def sample-file-path "inputs/day_21/sample-1")
(def sample-2-file-path "inputs/day_21/sample-2")

(defn parse-input
  [filename]
  (let [entries (string/split (slurp filename) #"\n")]
    (reduce (fn [out y]
              (into out (reduce (fn [map x]
                                  (into map {[x y] (nth (nth entries y) x)}))
                                out
                                (range (count (first entries))))))
            {}
            (range (count entries)))))

(defn print-field
  [field]
  (reduce (fn [_ y]
            (reduce (fn [_ x]
                      (print (get field [x y])))
                    nil
                    (range (inc (apply max (map first (keys field))))))
            (println))
          nil
          (range (inc (apply max (map second (keys field)))))))

(defn get-start
  [field]
  (first (first (filter (fn [[key val]] (if (= val \S) key false)) field))))

(defn take-step
  [field [x y]]
  (filter #(not (nil? %)) (map (fn [pos]
                                 (if (= \. (get field pos))
                                   pos
                                   nil))
                               [[(dec x) y] [x (dec y)]
                                [(inc x) y] [x (inc y)]])))

(defn part-one
  ([] (part-one input-file-path))
  ([filename]
   (let [input (parse-input filename)
         start (get-start input)
         field (assoc input start \.)]
     (loop [poss [start] cnt 64]
       (if (zero? cnt)
         (count poss)
         (recur (set (apply concat (map #(take-step field %) poss)))
                (dec cnt)))))))

(defn build-graph
  [field]
  (let [xmax (apply max (map first (keys field)))
        ymax (apply max (map second (keys field)))]
    (reduce (fn [graph y]
              (reduce (fn [out x]
                        (assoc out [x y] (take-step field [x y])))
                      graph
                      (range (inc xmax))))
            {}
            (range (inc ymax)))))

(defn get-destinations
  [graph width height start steps]
  (reduce (fn [poss _]
            (reduce (fn [new-poss [fpos [x y]]]
                      (into (into new-poss
                                  (map #(identity [% [x y]]) (get graph fpos)))
                            (remove nil?
                                    (let [[fx fy] fpos]
                                      (vector
                                       (if (= fx 0)
                                         [[(dec width) fy] [(dec x) y]]
                                         nil)
                                       (if (= fx (dec width))
                                         [[0 fy] [(inc x) y]]
                                         nil)
                                       (if (= fy 0)
                                         [[fx (dec height)] [x (dec y)]]
                                         nil)
                                       (if (= fy (dec height))
                                         [[fx 0] [x (inc y)]]
                                         nil))))))
                    (set [])
                    poss))
          (set [start])
          (range steps)))

(defn get-counts
  [destinations]
  (reduce (fn [out [_ pos]]
            (update out pos #(if (nil? %) 1 (inc %))))
          {}
          destinations))

(defn part-two
  ([] (part-two input-file-path))
  ([filename]
   (let [input (parse-input filename)
         start [(get-start input) [0 0]]
         field (assoc input (first start) \.)
         width (inc (apply max (map first (keys field))))
         height (inc (apply max (map second (keys field))))
         graph (build-graph field)
         steps 26501365
         destinations (get-destinations graph width height start
                                        (+ (mod steps width) (* width 2)))
         counts (get-counts destinations)
         expand (quot steps width)]
     (+ (reduce + (map #(get counts %) [[-2 0] [2 0] [0 2] [0 -2]]))
        (* expand (reduce + (map #(get counts %)
                                 [[-2 -1] [-2 1] [2 1] [2 -1]])))
        (* (dec expand) (reduce + (map #(get counts %)
                                       [[-1 -1] [-1 1] [1 1] [1 -1]])))
        (* expand expand (get counts [0 1]))
        (* (dec expand) (dec expand) (get counts [0 0]))))))

(defn run
  []
  (println (part-one))
  (println (part-two)))