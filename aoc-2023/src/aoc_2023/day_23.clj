(ns aoc-2023.day-23
  (:require [aoc-2023.helpers :as helpers]
            [clojure.string :as string]))

(def input-file-path "inputs/day_23/input")
(def sample-file-path "inputs/day_23/sample-1")

(defn parse-input
  [filename]
  (let [entries (string/split (slurp filename) #"\n")
        field (reduce (fn [out y]
                        (into out (for [x (range (count (first entries)))]
                                    {[x y] (nth (nth entries y) x)})))
                      {}
                      (range (count entries)))
        start (first (first (filter (fn [[[x y] c]]
                                      (if (= 0 y)
                                        (if (= \. c)
                                          [x y]
                                          false)
                                        false)) field)))
        end (first (first (filter (fn [[[x y] c]]
                                    (if (= (dec (count entries)) y)
                                      (if (= \. c)
                                        [x y]
                                        false)
                                      false)) field)))]
    [field start end]))

(defn get-nexts
  [field visited pos]
  (let [dirs (case (get field pos)
               \> [[1 0]]
               \< [[-1 0]]
               \^ [[0 -1]]
               \v [[0 1]]
               \. [[0 1] [0 -1] [1 0] [-1 0]])
        [x y] pos]
    (filter some? (for [[dx dy] dirs]
                    (let [newpos [(+ x dx) (+ y dy)]]
                      (if (or (= \# (get field newpos \#))
                              (some #{newpos} visited))
                        nil
                        [newpos (conj visited pos)]))))))

(defn walk
  [field start end]
  (loop [[[pos visited] & pipeline] [[start []]]
         paths []]
    (if pos
      (let [nxts (get-nexts field visited pos)
            npaths (if (= pos end) (conj paths (conj visited pos)) paths)]
        (recur (into pipeline nxts) npaths))
      paths)))

(defn part-one
  ([] (part-one input-file-path))
  ([filename]
   (let [[field start end] (parse-input filename)]
     (reduce max (map #(dec (count %)) (walk field start end))))))

(defn get-walkable
  [field]
  (reduce (fn [out [pos c]]
            (if (some #{(get field pos)} [\> \< \v \^ \.])
              (conj out pos)
              out))
          []
          field))

(defn get-neighbours
  [wps]
  (reduce (fn [out [x y]]
            (assoc out [x y] (filter some?
                                     (for [[dx dy] [[0 1] [0 -1] [1 0] [-1 0]]]
                                       (if (some  #{[(+ x dx) (+ y dy)]} wps)
                                         [(+ x dx) (+ y dy)]
                                         nil)))))
          {}
          wps))

(defn collapse
  [wps]
  (let [neighs (get-neighbours wps)]
    (reduce
     (fn [out wp]
       (assoc out wp
              (reduce
               (fn [nlist neigh]
                 (conj nlist (loop [point wp
                                    nxt neigh
                                    cost 1]
                               (if (= 2 (count (get neighs nxt)))
                                 (recur nxt
                                        (first (filter #(not (= point %))
                                                       (get neighs nxt)))
                                        (inc cost))
                                 [nxt cost]))))
               []
               (get neighs wp))))
     {}
     wps)))

(defn longest-way
  [neighs end point dist best seen]
  (if (= point end)
    dist
    (reduce max (for [[nxt c] (get neighs point)]
                  (if (contains? seen nxt)
                    best
                    (longest-way neighs end nxt
                                 (+ dist c) best (conj seen point)))))))

(defn part-two
  "quite slow - tried to minimize following:
   /r/adventofcode/comments/18oy4pc/2023_day_23_solutions/kelin3v/"
  ([] (part-two input-file-path))
  ([filename]
   (let [[field start end] (parse-input filename)
         collapsed (collapse (get-walkable field))]
     (longest-way collapsed end start 0 0 (set [])))))

(defn run
  []
  (println (part-one))
  (println (part-two)))