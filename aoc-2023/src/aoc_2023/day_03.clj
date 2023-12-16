(ns aoc-2023.day-03
  (:require [aoc-2023.helpers :as helpers]
            [clojure.string :as string]))

(def input-file-path "inputs/day_03/input")

(defn parse-input
  [filename]
  (string/split (slurp filename) #"\n"))

(defn get-index
  [s value from]
  (if-let [index (string/index-of s value from)]
     (if (or (re-matches #"\d" (str (get s (dec index))))
             (re-matches #"\d" (str (get s (+ index (count value))))))
       (get-index s value (inc index))
       index)
     nil))

(defn get-pos-to-check
  [line num]
  (if-let [index (get-index line num 0)]
    (range (dec index) (+ index (count num) 1))
    nil))

(defn check-line
  [line pos]
  (if line
    (some true?
          (map (fn [index]
                 (if (and (>= index 0) (< index (count line)))
                   (not (re-matches #"\.|\d" (str (get line index))))
                   false))
               pos))
    nil))

(defn part-one
  ([] (part-one input-file-path))
  ([filename]
   (let [lines (parse-input filename)]
     (reduce (fn [part-sum index]
               (reduce +
                       part-sum
                       (map (fn [num]
                              (if (some true?
                                        (map
                                         (fn [chk-idx]
                                           (check-line (get lines chk-idx)
                                                       (get-pos-to-check
                                                        (get lines index) num)))
                                         (list (dec index) index (inc index))))
                                (Integer. num)
                                0)) (re-seq #"\d+" (get lines index)))))
           0
           (range (count lines))))))

(defn get-pos-list
  [line re]
  (reduce (fn [pos entry]
            (if (re-matches re (str (second entry)))
              (conj pos (first entry))
              pos)
            )
          []
          (map-indexed vector line)))

(defn get-gear-idx
  [line]
  (get-pos-list line #"\*"))

(defn get-digit-idx
  [line]
  (get-pos-list line #"\d"))

(defn get-num-pos
  [line]
  (let [digit-idx (get-digit-idx line)]
    (reduce (fn [ret n]
              (if-let [prev (peek (peek ret))]
                (if (= prev (dec n))
                  (update ret (dec (count ret)) conj n)
                  (conj ret [n]))
                (conj ret [n])))
            []
            digit-idx)))

(defn get-gear-ratio
  [lines lineidx gearpos]
  (let [to-ret
        (reduce (fn [gearnums line-to-check]
                  (let [line (get lines line-to-check)
                        num-pos (get-num-pos line)
                  check-nums (map vector (re-seq #"\d+" line) num-pos)
                  checked-nums (filter (fn [entry]
                                         (some true?
                                               (map
                                                #(and (>= %1 (dec gearpos))
                                                      (<= %1 (inc gearpos)))
                                                (second entry))))
                                       check-nums)]
                    (concat gearnums (map #(first %) checked-nums))))
          []
          (range (dec lineidx) (+ 2 lineidx)))]
    (if (= 2 (count to-ret))
      (apply * (map #(Integer. %) to-ret))
      0)))

(defn part-two
  ([] (part-two input-file-path))
  ([filename]
   (let [lines (parse-input filename)]
     (reduce (fn [gearsum lineidx]
               (reduce + gearsum
                       (map #(get-gear-ratio lines lineidx %)
                            (get-gear-idx (get lines lineidx)))))
             0
             (range (count lines))))))

(defn run
  []
  (println (part-one))
  (println (part-two)))