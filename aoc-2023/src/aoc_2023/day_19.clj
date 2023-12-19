(ns aoc-2023.day-19
  (:require [aoc-2023.helpers :as helpers]
            [clojure.string :as string]))

(def input-file-path "inputs/day_19/input")
(def sample-file-path "inputs/day_19/sample-1")

(defn parse-wf
  [line]
  (let [[name entry] (string/split line #"\{")
        opentries (string/split (apply str (butlast entry)) #",")
        ops (reduce (fn [out e]
                      (let [[opstr des] (string/split e #":")]
                        (conj out
                              (if (nil? des)
                                {:des opstr}
                                {:op (second opstr)
                                 :val (parse-long
                                       (apply str (drop 2 opstr)))
                                 :target (first opstr)
                                 :des des}))))
                    []
                    opentries)]
    [name ops]))

(defn parse-shape
  [line]
  (reduce (fn [out entry]
            (let [[k v] (string/split entry #"=")]
              (assoc out (first k) (parse-long v))))
          {}
          (string/split (apply str (butlast (rest line))) #",")))

(defn parse-input
  [filename]
  (let [[wflines shapelines] (map #(string/split % #"\n")
                                  (string/split (slurp filename) #"\n\n"))
        workflows (into {} (map parse-wf wflines))
        shapes (map parse-shape shapelines)]
    [workflows shapes]))

(defn apply-wf
  [wfs name shape]
  (some (fn [op]
          (if (:op op)
            (if ((eval (read-string (str (:op op))))
                 (get shape (:target op)) (:val op))
              (if (get wfs (:des op))
                (apply-wf wfs (:des op) shape)
                (:des op))
              false)
            (if (get wfs (:des op))
              (apply-wf wfs (:des op) shape)
              (:des op)))) (get wfs name)))

(defn part-one
  ([] (part-one input-file-path))
  ([filename]
   (let [[wfs shapes] (parse-input filename)]
     (reduce + (map #(if (= "A" (apply-wf wfs "in" %))
                       (reduce + (vals %))
                       0) shapes)))))

(defn get-ranges
  [wfs name curanges]
  (if (= name "A")
    [curanges]
    (if (= name "R")
      []
      (first (reduce (fn [[out part] op]
                       (if (:op op)
                         (let [[s e] (get part (:target op))]
                           (if (= (:op op) \>)
                             (if (and (<= s (:val op)) (< (:val op) e))
                               [(into out
                                      (get-ranges
                                       wfs (:des op)
                                       (assoc part (:target op)
                                              [(inc (:val op))
                                               e])))
                                (assoc part (:target op) [s (:val op)])]
                               [out part])
                             (if (and (< s (:val op)) (<= (:val op) e))
                               [(into out
                                      (get-ranges
                                       wfs (:des op)
                                       (assoc part (:target op)
                                              [s
                                               (dec (:val op))])))
                                (assoc part (:target op) [(:val op) e])]
                               [out part])))
                         (reduced
                          [(into out (get-ranges wfs (:des op) part)) []])))
                     [[] curanges]
                     (get wfs name))))))

(defn part-two
  ([] (part-two input-file-path))
  ([filename]
   (let [[wfs _] (parse-input filename)
         ranges (get-ranges wfs "in" {\x [1 4000]
                                      \m [1 4000]
                                      \a [1 4000]
                                      \s [1 4000]})]
     (reduce + (map (fn [entries]
                      (reduce (fn [out [s e]]
                                (* out (- (inc e) s)))
                              1 (vals entries))) ranges)))))

(defn run
  []
  (println (part-one))
  (println (part-two)))